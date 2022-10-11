using CookieBased_Authentication.DB;
using CookieBased_Authentication.Models;
using CookieBased_Authentication.Models.VM;
using System.Security.Claims;

namespace CookieBased_Authentication.Services
{
    public class AuthService
    {
        public AuthService()
        {
            #region 初始化範例資料(Demo)
            using (var context = new ApplicationDBContext())
            {

                User user = new User()
                {
                    Id = 1,
                    Account = "ironman",
                    Password = "abc123",
                    CreateTime = DateTime.Now
                };

                var sampleData = context.Users.Where(o => o.Id == 1).FirstOrDefault();

                if (sampleData == null)
                {
                    context.Users.AddRange(user);
                    context.SaveChanges();
                }
            }
            #endregion
        }

        /// <summary>
        /// 簡易的使用者驗證(Demo用)
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        public User IsUserAuth(LoginVM loginVM)
        {
            using (var context = new ApplicationDBContext())
            {
              var obj = context.Users.Where(o => o.Account == loginVM.Account.ToLower())
                    .Where(o => o.Password == loginVM.Password)
                    .FirstOrDefault();
                return obj;
            }
        }

        /// <summary>
        /// Create Claims Principal
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ClaimsPrincipal CreateClaimsPrincipal(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(nameof(user.Account), user.Account));

            var identity = new ClaimsIdentity(claims, "Basic");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
