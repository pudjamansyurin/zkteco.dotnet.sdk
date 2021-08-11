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
        private readonly ISdkService _sdk;
        private readonly IUserService _user;

        public SdkController(ISdkService sdkService, IUserService userService)
        {
            _sdk = sdkService;
            _user = userService;
        }

        [HttpGet("device")]
        public ActionResult DeviceInfo()
        {
            try
            {
                Device info = new Device();
                _sdk.GetDeviceInfo(ref info);
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
                if (sUserID == null)
                {
                    List<User> users = new List<User>();
                    _sdk.GetUsers(ref users);
                    users.ForEach(u => _user.AddOrUpdate(u));
                    return Ok(_user.GetAll());
                }
                else
                {
                    User user = new User();
                    _sdk.GetUser(sUserID, ref user);
                    _user.AddOrUpdate(user);
                    return Ok(_user.Get(user.sUserID));
                }
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }


        //[HttpPut("user/{sUserID}")]
        //public ActionResult EnableUser(string sUserID)
        //{
        //    try
        //    {
        //        var user = _user.Get(sUserID);
        //        _sdk.SetUser(user);
        //        _user.Enable(user);
        //        return Ok("User " + sUserID + " enabled");
        //    }
        //    catch (Exception e)
        //    {
        //        return Conflict(e.Message);
        //    }
        //}


        //[HttpDelete("user/{sUserID}")]
        //public ActionResult DisableUser(string sUserID, [FromForm] DateTime activeAt)
        //{
        //    try
        //    {
        //        var user = _user.Get(sUserID);
        //        _sdk.DeleteUser(user);
        //        _user.Disable(user, activeAt);
        //        return Ok("User " + sUserID + " finger disabled");
        //    }
        //    catch (Exception e)
        //    {
        //        return Conflict(e.Message);
        //    }
        //}

    }
}
