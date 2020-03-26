using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParksAndDeath.Models;

namespace ParksAndDeath.Controllers
{
    public class HomeController : Controller
    {
        private readonly ParksAndDeathDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, ParksAndDeathDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //checking if user is logged in
            if (_signInManager.IsSignedIn(User))
            {
                //identify the user that is logged in
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //checks if the user has their information filled in and if they do, it sends them to the parks list to fill their bucket list

                var userBl = CheckUserInfo(_context, userId);
                var userInfo = CheckBucketList(_context, userId);

                if (userBl != false && userInfo != false)
                {
                    return View("FullUser"); //buttons to see parks, button to see BucketList, update user info button. parks youve visited button
                }
                if (userInfo != false)
                {
                    return View("FullUser");

                }
                return RedirectToAction("AddUserInput", "User");
            }
            return View();
        }

        public static bool CheckUserInfo(ParksAndDeathDbContext context, string id)
        {
            try
            {
                context.UserInfo.Where(x => x.OwnerId == id).First();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool CheckBucketList(ParksAndDeathDbContext context, string id)
        {
            try
            {
                context.UserParks.Where(x => x.CurrentUserId == id).First();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

