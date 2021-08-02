using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using WebApiZkteco.Services;
using WebApiZkteco.Models;
using System.IO;

namespace WebApiZkteco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SdkController : ControllerBase
    {
        private readonly SdkService sdk;

        public SdkController()
        {
            sdk = new SdkService("192.168.0.22", 4370);
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
        public ActionResult GetUserInfo()
        {
            try
            {
                List<UserInfo> users = new List<UserInfo>();
                sdk.GetUserInfo(ref users);

                // TODO: save to database
                var fileName = Path.Combine(Environment.CurrentDirectory, "users.json");
                string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(fileName, jsonString);

                return Ok(users);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


        [HttpPost("user-info")]
        public ActionResult SetUserInfo()
        {
            try
            {
                // TODO: read from database
                var fileName = Path.Combine(Environment.CurrentDirectory, "users.json");
                string jsonString = System.IO.File.ReadAllText(fileName);
                List<UserInfo> users = JsonSerializer.Deserialize<List<UserInfo>>(jsonString);

                sdk.SetUserInfo(users);

                return Ok(users);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


    }
}
