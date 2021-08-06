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
        private readonly ISdkService sdk;

        public SdkController(ISdkService _sdk)
        {
            sdk = _sdk;
        }

        [HttpGet("device")]
        public ActionResult DeviceInfo()
        {
            try
            {
                Device info = new Device();
                sdk.GetDeviceInfo(ref info);
                return Ok(info);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet("user/{sUserID?}")]
        public ActionResult GetUser(string sUserID)
        {
            try
            {
                List<User> users = new List<User>();

                if (sUserID == null)
                {
                    sdk.GetUsers(ref users);
                }
                else
                {
                    User user = new User();
                    sdk.GetUser(sUserID, ref user);
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


        [HttpPut("user/{sUserID}")]
        public ActionResult SetUser(string sUserID)
        {
            try
            {
                var user = FindUserInDB(sUserID);

                sdk.SetUser(user);

                return Ok("User " + sUserID + " uploaded");
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


        [HttpDelete("user/{sUserID}")]
        public ActionResult DeleteUser(string sUserID)
        {
            try
            {
                var user = FindUserInDB(sUserID);

                sdk.DeleteUser(user);

                return Ok("User " + sUserID + " deleted");
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        private User FindUserInDB(string sUserID)
        {
            // TODO: read from database
            string fileName = Path.Combine(Environment.CurrentDirectory, "users.json");
            string jsonString = System.IO.File.ReadAllText(fileName);
            List<User> users = JsonSerializer.Deserialize<List<User>>(jsonString);

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
