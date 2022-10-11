using CookieBased_Authentication.Models.VM;
using CookieBased_Authentication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookieBased_Authentication.Controllers
{
    public class AuthController : Controller
    {
        private AuthService _authService;
        public AuthController()
        {
            _authService = new AuthService();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            else
            {
                var user = _authService.IsUserAuth(loginVM);
                if (user != null)
                {
                    var principal = _authService.CreateClaimsPrincipal(user);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.User = principal;
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }

        }

        /// <summary>
        /// 存取權限拒絕
        /// </summary>
        /// <returns></returns>

        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
