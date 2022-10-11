using CookieBased_Authentication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var session_store = new InMemoryTickerStore();
builder.Services.AddSingleton(session_store);
builder.Services
       .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
       {
           options.Cookie.Name = "CookiesBased_WebSite";
           options.SessionStore = session_store;

           //�]�w���ҵn�J��}
           options.LoginPath = "/Auth/Login";
           //�]�w�v�����ҾD��
           options.AccessDeniedPath = "/Auth/AccessDenied";
           if (builder.Environment.IsDevelopment())
           {
               options.Cookie.MaxAge = TimeSpan.MaxValue;  // �}�o�Ҧ��]�w���ä��L��, ��K����
           }
       })
       ;

builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorizationHeader();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");// �NLogin�]���_�l��

app.Run();
