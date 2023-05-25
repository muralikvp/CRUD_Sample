using Crud_Sample.Models;
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

        [HttpGet]
        [Route("InnerJoinEx")]
        public IEnumerable<UserRoleView> InnerJoinEx()
        {
            // select * from[dbo].[User] u inner join[dbo].[UserRoles] ur on(u.Role = ur.Id)
            IEnumerable<UserRoleView> rolesView = (from t1 in _context.user
                      join t2 in _context.userRoles on t1.Role equals t2.Id
                      select new UserRoleView {Name = t1.Name, RoleName = t2.Name }).ToList();

            return rolesView;

        }




        [HttpGet]
        [Route("LeftJoinEx")]
        public IEnumerable<UserRoleView> LeftJoinEx()
        {
            // select * from[dbo].[User] u left join[dbo].[UserRoles] ur on(u.Role = ur.Id)
            IEnumerable<UserRoleView> rolesView = (from t1 in _context.user
                                                   join t2 in _context.userRoles on t1.Role equals t2.Id into ps
                                                   from t2 in ps.DefaultIfEmpty()
                                                   select new UserRoleView { Name = t1.Name, RoleName = t2.Name }).ToList();

            return rolesView;

        }


        [HttpGet]
        [Route("OrderByEx")]
        public IEnumerable<UserView> OrderByEx()
        {
            // select Name,UserName,Role from [dbo].[User] u order by Username,Password desc

            IEnumerable<UserView> rolesView = (from t1 in _context.user
                                                   orderby t1.Username descending 
                                           select new UserView { Name = t1.Name, Role = t1.Role }).ToList();

            return rolesView;

        }


        [HttpGet]
        [Route("CustomSelectEx")]
        public dynamic CustomSelectEx()
        {
            // select Name as'ExtenName',Role as 'ExtenRole',Password from [dbo].[User] u order by Username desc

            var rolesView = (from t1 in _context.user
                                               orderby t1.Username descending
                                               select new 
                                               { 
                                                   ExtenName = t1.Name.ToUpper(), 
                                                   ExtenRole = t1.Role,
                                                   ExPassword = t1.Password.ToLower(),
                                                   Ex= "Hope Tutors" 
                                               }
                                               ).ToList();

            return rolesView;

        }

        [HttpGet]
        [Route("WhereEx")]
        public dynamic WhereEx()
        {
            // select Name as'ExtenName',Role as 'ExtenRole',Password from [dbo].[User] u where Username = "Murali" 

            var rolesView = (from t1 in _context.user
                             where t1.Username == "Akil1" && t1.Name == "Akil"
                             select new
                             {
                                 ExtenName = t1.Name.ToUpper(),
                                 ExtenRole = t1.Role,
                                 ExPassword = t1.Password.ToLower(),
                                 Ex = "Hope Tutors"
                             }
                                               ).ToList();

            return rolesView;

        }


        [HttpGet]
        [Route("DistinctEx")]
        public dynamic DistinctEx()
        {
            // select Name as'ExtenName',Role as 'ExtenRole',Password from [dbo].[User] u where Username = "Murali" 

            var rolesView = (from t1 in _context.user
                             select new
                             {
                                 t1.Name,
                             }).Distinct();

            return rolesView;

        }

        [HttpGet]
        [Route("UnionEx")]
        public dynamic UnionEx()
        {

            var Normalusers = (from t1 in _context.user
                             where t1.Role == 1
                             select new
                             {
                                 ExtenName = t1.Name.ToUpper(),
                                 ExtenRole = t1.Role,
                                 ExPassword = t1.Password.ToLower(),
                                 Ex = "Hope Tutors"
                             }).ToList();

            var adminUsers = (from t1 in _context.user
                               where t1.Role == 2
                               select new
                               {
                                   ExtenName = t1.Name.ToUpper(),
                                   ExtenRole = t1.Role,
                                   ExPassword = t1.Password.ToLower(),
                                   Ex = "Mango Groove"
                               }).ToList();

            var combinedUsers = Normalusers.Union(adminUsers);


            return combinedUsers;

        }
        [HttpGet]
        [Route("TakeEx")]
        public IEnumerable<User> TakeEx()
        {

            // select top 2 Name,Username,Role from [dbo].[User] u order by Username,Password desc

            IEnumerable<User> rolesView = (from t1 in _context.user
                                           orderby t1.Username descending
                                           select new User { Name = t1.Name, Username = t1.Username, Role = t1.Role }).Take(2);

            return rolesView;

        }

        [HttpGet]
        [Route("DeferredExecution")]
        public int DeferredExecution()
        {
            #region deferred-execution
            // Sequence operators form first-class queries that
            // are not executed until you enumerate over them.

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            int i = 0;
            var q = from n in numbers
                    select ++i;



            //  var result = CustomerList; // logic 
            //   CustomerList.where;
            //   CustomerList.join;

            //toList();

            // Note, the local variable 'i' is not incremented
            // until each element is evaluated (as a side-effect):
            foreach (var v in q)
            {
                Console.WriteLine($"v = {v}, i = {i}");
            }
            #endregion
            return 0;
        }

        [HttpGet]
        [Route("EagerExecution")]
        public int EagerExecution()
        {
            #region eager-execution
            // Methods like ToList() cause the query to be
            // executed immediately, caching the results.

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            int i = 0;
            var q = (from n in numbers
                     select ++i)
                     .ToList();

            // The local variable i has already been fully
            // incremented before we iterate the results:
            foreach (var v in q)
            {
                Console.WriteLine($"v = {v}, i = {i}");
            }
            #endregion
            return 0;
        }

        [HttpGet]
        [Route("ReuseQueryDefinition")]
        public int ReuseQueryDefinition()
        {
            #region reuse-query
            // Deferred execution lets us define a query once
            // and then reuse it later after data changes.

            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var lowNumbers = from n in numbers
                             where n <= 3
                             select n;

            Console.WriteLine("First run numbers <= 3:");
            foreach (int n in lowNumbers)
            {
                Console.WriteLine(n);
            }

            for (int i = 0; i < 10; i++)
            {
                numbers[i] = -numbers[i];
            }

            // During this second run, the same query object,
            // lowNumbers, will be iterating over the new state
            // of numbers[], producing different results:
            Console.WriteLine("Second run numbers <= 3:");
            foreach (int n in lowNumbers)
            {
                Console.WriteLine(n);
            }
            #endregion
            return 0;
        }
    }
}