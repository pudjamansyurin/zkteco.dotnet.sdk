using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiZkteco.Models
{
    public class UserInfo
    {

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
        public int iFaceLen { get; set;  } 
    }
}
