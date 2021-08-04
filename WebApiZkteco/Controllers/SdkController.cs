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
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("user-info/{sUserID?}")]
        public ActionResult GetUserInfo(string sUserID)
        {
            try
            {
                List<UserInfo> users = new List<UserInfo>();

                if (sUserID == null)
                {
                    sdk.GetUsersInfo(ref users);
                }
                else
                {
                    UserInfo user = new UserInfo();
                    sdk.GetUserInfo(sUserID, ref user);
                    users.Add(user);
                }

                // TODO: save to database
                //string fileName = Path.Combine(Environment.CurrentDirectory, "users.json");
                //string jsonString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                //System.IO.File.WriteAllText(fileName, jsonString);

                return Ok(users);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


        [HttpPut("user-info/{sUserID}")]
        public ActionResult SetUserInfo(string sUserID)
        {
            try
            {
                var user = FindUserInDB(sUserID);

                sdk.SetUserInfo(user);

                return Ok("User " + sUserID + " uploaded");
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


        [HttpDelete("user-info/{sUserID}")]
        public ActionResult DeleteUserInfo(string sUserID)
        {
            try
            {
                var user = FindUserInDB(sUserID);

                sdk.DeleteUserInfo(user);

                return Ok("User " + sUserID + " deleted");
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        private UserInfo FindUserInDB(string sUserID)
        {
            // TODO: read from database
            string fileName = Path.Combine(Environment.CurrentDirectory, "users.json");
            string jsonString = System.IO.File.ReadAllText(fileName);
            List<UserInfo> users = JsonSerializer.Deserialize<List<UserInfo>>(jsonString);

            // Check is user exist
            try
            {
                return users.Where(u => u.sUserID == sUserID).First();
            }
            catch (Exception e)
            {
                throw new Exception("User not found: ", e);
            }
        }

    }
}
