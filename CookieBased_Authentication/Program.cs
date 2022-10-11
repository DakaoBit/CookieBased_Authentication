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

           //設定驗證登入位址
           options.LoginPath = "/Auth/Login";
           //設定權限驗證遭拒
           options.AccessDeniedPath = "/Auth/AccessDenied";
           if (builder.Environment.IsDevelopment())
           {
               options.Cookie.MaxAge = TimeSpan.MaxValue;  // 開發模式設定為永不過期, 方便測試
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
    pattern: "{controller=Auth}/{action=Login}/{id?}");// 將Login設為起始頁

app.Run();
