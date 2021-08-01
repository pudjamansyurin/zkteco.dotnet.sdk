using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiZkteco.Models
{
    public class UserInfo
    {

        public string sdwEnrollNumber { get; set; }
        public string sName { get; set; }
        public int idwFingerIndex { get; set; }
        public string sData { get; set; }
        public int iPrivilege { get; set; }
        public string sPassword { get; set; }
        public bool bEnabled { get; set; }
        public int iFlag { get; set; }
    }
}
