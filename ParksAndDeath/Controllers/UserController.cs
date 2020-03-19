using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ParksAndDeath.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        public IActionResult UserInput()
        {
            return View();
        }
    }
}