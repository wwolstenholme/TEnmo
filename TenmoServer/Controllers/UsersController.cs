using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;
using Microsoft.AspNetCore.Authorization;

namespace TenmoServer.Controllers
{
    [Route("User")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserDao userDao;

        public UsersController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpPost()]
        public ActionResult<User> Add(string username, string password)
        {
            User user1 = userDao.AddUser(username, password);
            return Created($"/User/{user1.UserId}", user1);
        }

       [HttpGet()]

       public ActionResult<List<User>> GetUsers()
        {
            List<User> usersList = userDao.GetUsers();
            return Ok(usersList);
        }

        //fix this method
        [HttpGet("{UserId}")]

        public ActionResult<User> GetAccountFromId(string userName)
        {
            User user = userDao.GetUser(userName);
            return Ok(userName);
        }






    }
}
