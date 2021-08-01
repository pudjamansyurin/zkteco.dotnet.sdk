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
            try
            {
                sdk.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [HttpGet("connected")]
        public ActionResult Connected()
        {
            return Ok(sdk.GetConnectState());
        }

        [HttpGet("device-info")]
        public ActionResult DeviceInfo()
        {
            try
            {
                DeviceInfo info = new DeviceInfo();
                sdk.GetDeviceInfo(ref info);
                return Ok(info);
            } catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("user-info")]
        public ActionResult UserInfo()
        {
            try
            {
                List<UserInfo> users = new List<UserInfo>();
                sdk.GetUserInfo(ref users);
                return Ok(users);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

    }
}
