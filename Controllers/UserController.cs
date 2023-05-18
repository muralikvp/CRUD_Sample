using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly HTContext _context;
        public UserController(ILogger<UserController> logger, HTContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> GetUser()
        {
            //select * from user 

            return _context.user.ToList();
        }

        [HttpPost(Name = "PostUser")]
        public ActionResult<User> InsertUser(User userOb)
        {
            //insert into[HTInfo_1].[dbo].[User] values('krishnan', 'krishnankvp', 'test', 1)
            _context.user.Add(userOb);
            _context.SaveChanges();

            return CreatedAtAction("GetUsers", new { id = userOb.Id }, userOb);


        }
    }
}