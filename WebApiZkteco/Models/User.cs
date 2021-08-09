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

        public DateTime activeStart { get; set; }
        public DateTime activeStop { get; set; }
        public bool active { get; set; }
    }
}
