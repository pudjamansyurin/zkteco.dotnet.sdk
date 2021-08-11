using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiZkteco.Models;
using WebApiZkteco.Services;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiZkteco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> log;
        private readonly IUserService _user;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            log = logger;
            _user = userService;
        }

        [HttpGet("{sUserID?}")]
        public ActionResult Get(string sUserID)
        {
            try
            {
                if (sUserID == null)
                {
                    List<User> users = new List<User>();
                    users.ForEach(u => _user.AddOrUpdate(u));
                    return Ok(_user.GetAll());
                }
                else
                {
                    User user = new User();
                    _user.AddOrUpdate(user);
                    return Ok(_user.Get(user.sUserID));
                }
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("schedule/{sUserID}")]
        public ActionResult Schedule(string sUserID, [FromBody] Schedule schedule)
        {
            try
            {
                var user = _user.Get(sUserID);
                _user.Schedule(user, schedule.start, schedule.stop);
                return Ok("User " + sUserID + " scheduled at " + schedule.start + " = " + schedule.stop);
            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }
    }
}
