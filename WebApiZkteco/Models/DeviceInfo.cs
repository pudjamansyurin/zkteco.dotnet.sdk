using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiZkteco.Models
{
    public class DeviceInfo
    {
        public string sFirmver { get; set;}
        public string sMac { get; set; }
        public string sPlatform { get; set; }
        public string sSN  { get; set; }
        public string sProductTime  { get; set; }
        public string sDeviceName  { get; set; }
        public int iFPAlg { get; set; }
        public int iFaceAlg { get; set; }
        public string sProducter  { get; set; }
    }
}
