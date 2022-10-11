using System.ComponentModel.DataAnnotations;

namespace CookieBased_Authentication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
