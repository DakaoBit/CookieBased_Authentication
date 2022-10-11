using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CookieBased_Authentication.Models.VM
{
    public class LoginVM
    {

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "請輸入帳號!")]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "請輸入密碼!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
