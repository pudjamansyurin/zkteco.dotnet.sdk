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
        private readonly ZkContext ctx;

        public UserController(ILogger<UserController> logger, ZkContext context)
        {
            log = logger;
            ctx = context;
        }

        // GET: api/<UserController>
        [HttpGet]
        //public ActionResult<List<User>> Get()
        //{
        //    return UserService.GetAll();
        //}
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await ctx.Users.ToListAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        //public ActionResult<User> Get(int id)
        //{
        //    var user = UserService.Get(id);
        //    if (user == null)
        //        return NotFound();
        //    return user;
        //}
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await ctx.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return user;
        }

        //// POST api/<UserController>
        //[HttpPost]
        ////public IActionResult Post([FromBody] User user)
        ////{
        ////    UserService.Add(user);
        ////    return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        ////}
        //public async Task<ActionResult<User>> Post(User user)
        //{
        //    var newUser = new User
        //    {
        //        Name = user.Name,
        //        FingerIdx = user.FingerIdx,
        //        Password = user.Password,
        //        Priviledge = user.Priviledge,
        //        Enabled = user.Enabled,
        //    };

        //    ctx.Users.Add(newUser);
        //    await ctx.SaveChangesAsync();

        //    return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        //}

        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        ////public IActionResult Put(int id, [FromBody] User user)
        ////{
        ////    if (id != user.Id)
        ////        return BadRequest();

        ////    var existingUser = UserService.Get(id);
        ////    if (existingUser == null)
        ////        return NotFound();

        ////    user.Id = existingUser.Id;
        ////    UserService.Update(user);
        ////    return NoContent();
        ////}
        //public async Task<IActionResult> Put(string id, User user)
        //{
        //    if (id != user.Id)
        //        return BadRequest();

        //    var existingUser = await ctx.Users.FindAsync(id);
        //    if (existingUser == null)
        //        return NotFound();

        //    existingUser.Name = user.Name;
        //    existingUser.FingerIdx = user.FingerIdx;
        //    existingUser.Password = user.Password;
        //    existingUser.Priviledge = user.Priviledge;
        //    existingUser.Enabled = user.Enabled;

        //    try
        //    {
        //        await ctx.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException) when (!ctx.Users.Any(e => e.sUserID == id))
        //    {
        //        return NotFound();
        //    }
        //    return NoContent();
        //}

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var user = UserService.Get(id);
        //    if (user == null)
        //        return NotFound();

        //    UserService.Delete(id);
        //    return NoContent();
        //}
        public async Task<IActionResult> Delete(string id)
        {
            var user = await ctx.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            ctx.Users.Remove(user);
            await ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}
