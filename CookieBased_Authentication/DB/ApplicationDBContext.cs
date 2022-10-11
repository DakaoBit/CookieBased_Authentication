using CookieBased_Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace CookieBased_Authentication.DB
{
    public class ApplicationDBContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ApplicationDb");
        }
        public DbSet<User> Users { get; set; }
    }
}
