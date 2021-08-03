using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiZkteco.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FingerIdx { get; set; }
        public int Priviledge { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
    }
}
