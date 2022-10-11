using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.AspNetCore.Http
{
    public class InMemoryTickerStore : ITicketStore
    {
        private Dictionary<string, AuthenticationTicket> _tickets = new Dictionary<string, AuthenticationTicket>();

        async Task ITicketStore.RemoveAsync(string key)
        {
            lock (_tickets)
            {
                if (_tickets.ContainsKey(key))
                    _tickets.Remove(key);
            }
            await Task.CompletedTask;
        }

        async Task ITicketStore.RenewAsync(string key, AuthenticationTicket ticket)
        {
            lock (_tickets)
            {
                _tickets[key] = ticket;
            }
            await Task.CompletedTask;
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            lock (_tickets)
            {
                _tickets.TryGetValue(key, out ticket);
            }
            await Task.CompletedTask;
            return ticket;
        }

        async Task<string> ITicketStore.StoreAsync(AuthenticationTicket ticket)
        {
            string key = Guid.NewGuid().ToString();
            lock (_tickets)
            {
                _tickets[key] = ticket;
            }
            await Task.CompletedTask;
            return key;
        }
    }

    public static class HttpContextExtensions
    {
        public static IServiceCollection AddAuthorizationHeader(this IServiceCollection services)
        {
            services.TryAddTransient(GetToken);
            return services;
        }

        private static string GetToken(IServiceProvider service)
        {
            HttpContext context = service.GetService<IHttpContextAccessor>()?.HttpContext;
            var token = context.GetAuthorizationHeader();
            return token;
        }

        public static void UseAuthorizationHeader(this IApplicationBuilder buildr)
        {
            buildr.Use(async (httpContext, next) =>
            {
                var token = httpContext.GetAuthorizationHeader();
                if (!string.IsNullOrEmpty(token))
                {
                    var ticketStore = httpContext.RequestServices.GetService<InMemoryTickerStore>();
                    var ticket = await ticketStore.RetrieveAsync(token);
                    if (ticket != null)
                    {
                        httpContext.User = ticket.Principal;
                    }
                }
                await next();
            });
        }

        public static string GetAuthorizationHeader(this HttpContext context)
        {
            return context.Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
                .ToString()?.Replace("Bearer ", "")?.Trim();
        }
    }
}