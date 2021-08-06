using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiZkteco.Models
{
    public class ZkContext : DbContext
    {
        public ZkContext(DbContextOptions<ZkContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPending> UserPendings { get; set; }
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string sUserID { get; set; }
        public string sName { get; set; }
        public string sPassword { get; set; }
        public int iPrivilege { get; set; }
        public bool bEnabled { get; set; }
        public int idwFingerIndex { get; set; }
        public int iFingerFlag { get; set; }
        public string sFingerData { get; set; }
        public int iFingerLen { get; set; }
        public int iFaceIndex { get; set; }
        public string sFaceData { get; set; }
        public int iFaceLen { get; set; }
    }

    public class UserPending
    {
        [Key]
        public int id { get; set; }
        public User user { get; set; }
        public DateTime activeAt { get; set; }

        public UserPending(User _user, DateTime _activeAt)
        {
            user = _user;
            activeAt = _activeAt;
        }
    }
}
