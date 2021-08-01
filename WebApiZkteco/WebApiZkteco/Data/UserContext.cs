using Microsoft.EntityFrameworkCore;
using WebApiZkteco.Models;

namespace WebApiZkteco.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}