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
        public IActionResult UserInput()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userInfo = _context.UserInfo.Where(x => x.OwnerId == id);
            var testType = userInfo.GetType().ToString();
            return View(testType);
        }
    }
}