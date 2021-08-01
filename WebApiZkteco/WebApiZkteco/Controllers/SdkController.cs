using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiZkteco.Services;
using WebApiZkteco.Models;

namespace WebApiZkteco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SdkController : ControllerBase
    {
        private readonly SdkService sdk;

        public SdkController()
        {
            sdk = new SdkService("192.168.0.102", 4370);
            sdk.Connect();
        }

        [HttpGet("connected")]
        public bool Connected()
        {
            return sdk.GetConnectState();
        }

        [HttpGet("devinfo")]
        public ActionResult DeviceInfo()
        {
            DeviceInfo info = new DeviceInfo();
            sdk.sta_GetDeviceInfo(ref info);
            return Ok(info);
        }

    }
}
