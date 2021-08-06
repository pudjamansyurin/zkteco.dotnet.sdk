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
        private readonly ZkContext ctx;

        public SdkController(ISdkService sdkService, ZkContext context)
        {
            sdk = sdkService;
            ctx = context;
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

                // save to database
                users.ForEach(u =>
                {
                    if (ctx.Users.Any(e => e.sUserID == u.sUserID))
                    {
                        ctx.Users.Update(u);
                    }
                    else
                    {
                        ctx.Users.Add(u);
                    }
                    ctx.SaveChanges();
                });

                // save to JSON
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
        public ActionResult EnableUser(string sUserID)
        {
            try
            {
                var user = FindUser(sUserID);

                if (!ctx.UserPendings.Any(u => u.user == user))
                {
                    return Conflict("User " + user.sUserID + " already active");
                }

                sdk.SetUser(user);

                // delete from pending user
                var pending = ctx.UserPendings.First(u => u.user == user);
                ctx.UserPendings.Remove(pending);
                ctx.SaveChanges();

                return Ok("User " + sUserID + " finger enabled");
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


        [HttpDelete("user/{sUserID}")]
        public ActionResult DisableUser(string sUserID, [FromBody] DateTime activeAt)
        {
            // TODO: parse datetime from user request
            return Ok(sUserID + "" + activeAt);

            try
            {
                var user = FindUser(sUserID); // check is user exist

                if (ctx.UserPendings.Any(u => u.user == user))
                {
                    return Conflict("User " + user.sUserID + " already disabled");
                }

                sdk.DeleteUser(user);

                // insert to pending user
                ctx.UserPendings.Add(new UserPending(user, activeAt));
                ctx.SaveChanges();

                return Ok("User " + sUserID + " finger disabled");
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        private User FindUser(string sUserID)
        {
            //// read from JSON
            //string fileName = Path.Combine(Environment.CurrentDirectory, "users.json");
            //string jsonString = System.IO.File.ReadAllText(fileName);
            //List<User> users = JsonSerializer.Deserialize<List<User>>(jsonString);

            // Check is user exist
            try
            {
                //return users.Where(u => u.sUserID == sUserID).First();
                return ctx.Users.First(u => u.sUserID == sUserID);
            }
            catch (Exception e)
            {
                throw new Exception("User not found: ", e);
            }
        }
    }
}
