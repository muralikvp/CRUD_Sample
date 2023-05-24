using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {


        //[Route("HopeTutors/api")]

        //localhost:33/HopeTutors/api/GetUser

        //[Route("[controller]/api")]

        //localhost:33/user/api/GetUser

        //[Route("Dev/[controller]/api")]

        //localhost:33/Dev/user/api/GetUser

        private readonly ILogger<UserController> _logger;
        private readonly HTContext _context;
        public UserController(ILogger<UserController> logger, HTContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IEnumerable<User> GetUser()
        {
            //select * from user 

            return _context.user.ToList();
        }

        [HttpPost]
        [Route("InsertUser")]
        public ActionResult<User> InsertUser(User userOb)
        {
            //insert into[HTInfo_1].[dbo].[User] values('krishnan', 'krishnankvp', 'test', 1)
            _context.user.Add(userOb);
            _context.SaveChanges();
            return CreatedAtAction("GetUsers", new { id = userOb.Id }, userOb);
        }

        [HttpGet]
        [Route("GetLinqUser")]
        public IEnumerable<string> getLinqUser(int id)
        {

            //select * from user 
            List<User> users = _context.user.ToList();

            //select top 1 from users where id=12

            User user1 = (from u in users
                          where u.Id == id
                          select u).First();

            //select username from users

            var user = (from u in users                      
                        select u.Username);

            //select top 1 from users where id=12

            User user2 = users.FirstOrDefault(p => p.Id == id);
            //if (user2 == null)
            //{
            //    return NotFound();
            //}

            return user;
        }

        [HttpGet]
        [Route("GetUser")]
        public ActionResult<User> getUser(int id)
        {
            //select * from user where id = 12;
            var _user = _context.user.Find(id);
            if (_user == null)
            {
                return NotFound();
            }
            return _user;
        }

        [HttpGet]
        [Route("testGroupBy")]
        public dynamic testGroupBy()
        {
            var users = _context.user.ToList();
            var orderGroups = from u in users
                              group u by u.Role into g
                              select (Role: g.Key, users: g);

            return orderGroups;

        }

        [HttpPost]
        [Route("UpdateUser")]
        public ActionResult<User> UpdateUser(User user)
        {

            //update user set name = Hari,userName = Dani id = 12;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return user;

            //var _user = _context.user.Find(user.Id);
            //if (_user == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
               
            //}
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public ActionResult<User> deleteUser(int id)
        { 
            var _user = _context.user.Find(id);
            if (_user == null)
            {
                return NotFound();
            }
            else
            {
                _context.user.Remove(_user);
                _context.SaveChanges();
                return Ok();
            }
        }

    }
    }