//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WebApiZkteco.Models;
//using WebApiZkteco.Services;
//using WebApiZkteco.Data;
//using Microsoft.EntityFrameworkCore;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace WebApiZkteco.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly ILogger<UserController> _logger;
//        private readonly UserContext _context;

//        public UserController(ILogger<UserController> logger, UserContext context)
//        {
//            _logger = logger;
//            _context = context;
//        }

//        // GET: api/<UserController>
//        [HttpGet]
//        //public ActionResult<List<User>> Get()
//        //{
//        //    return UserService.GetAll();
//        //}
//        public async Task<ActionResult<IEnumerable<User>>> Get()
//        {
//            return await _context.User.ToListAsync();
//        }

//        // GET api/<UserController>/5
//        [HttpGet("{id}")]
//        //public ActionResult<User> Get(int id)
//        //{
//        //    var user = UserService.Get(id);
//        //    if (user == null)
//        //        return NotFound();
//        //    return user;
//        //}
//        public async Task<ActionResult<User>> Get(int id)
//        {
//            var user = await _context.User.FindAsync(id);
//            if (user == null)
//                return NotFound();
//            return user;
//        }

//        // POST api/<UserController>
//        [HttpPost]
//        //public IActionResult Post([FromBody] User user)
//        //{
//        //    UserService.Add(user);
//        //    return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
//        //}
//        public async Task<ActionResult<User>> Post(User user)
//        {
//            var newUser = new User
//            {
//                Name = user.Name,
//                FingerIdx = user.FingerIdx,
//                Password = user.Password,
//                Priviledge = user.Priviledge,
//                Enabled = user.Enabled,
//            };

//            _context.User.Add(newUser);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
//        }

//        // PUT api/<UserController>/5
//        [HttpPut("{id}")]
//        //public IActionResult Put(int id, [FromBody] User user)
//        //{
//        //    if (id != user.Id)
//        //        return BadRequest();

//        //    var existingUser = UserService.Get(id);
//        //    if (existingUser == null)
//        //        return NotFound();

//        //    user.Id = existingUser.Id;
//        //    UserService.Update(user);
//        //    return NoContent();
//        //}
//        public async Task<IActionResult> Put(int id, User user)
//        {
//            if (id != user.Id)
//                return BadRequest();

//            var existingUser = await _context.User.FindAsync(id);
//            if (existingUser == null)
//                return NotFound();

//            existingUser.Name = user.Name;
//            existingUser.FingerIdx = user.FingerIdx;
//            existingUser.Password = user.Password;
//            existingUser.Priviledge = user.Priviledge;
//            existingUser.Enabled = user.Enabled;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException) when (!UserExists(id))
//            {
//                return NotFound();
//            }
//            return NoContent();
//        }

//        // DELETE api/<UserController>/5
//        [HttpDelete("{id}")]
//        //public IActionResult Delete(int id)
//        //{
//        //    var user = UserService.Get(id);
//        //    if (user == null)
//        //        return NotFound();

//        //    UserService.Delete(id);
//        //    return NoContent();
//        //}
//        public async Task<IActionResult> Delete(int id)
//        {
//            var user = await _context.User.FindAsync(id);
//            if (user == null)
//                return NotFound();

//            _context.User.Remove(user);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool UserExists(long id) =>
//             _context.User.Any(e => e.Id == id);
//    }
//}
