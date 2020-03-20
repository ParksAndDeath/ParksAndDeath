using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParksAndDeath.Models;

namespace ParksAndDeath.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly ParksAndDeathDbContext _context;
        public UserController(ParksAndDeathDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddUserInput()
        {
            return View();
        }

        [HttpPost]
        //public IActionResult AddUserInput(UserInfo userInfo)
        //{
        //    string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    return RedirectToAction()
        //}
        public IActionResult UserInput()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userInfo = _context.UserInfo.Where(x => x.OwnerId == id);
            var testType = userInfo.GetType().ToString();
            return View(userInfo);
        }
    }
}