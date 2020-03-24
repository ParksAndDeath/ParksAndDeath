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

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, ParksAndDeathDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //checking if user is logged in
            if (_signInManager.IsSignedIn(User))
            {
                //identify the user that is logged in
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //checks if the user has their information filled in and if they do, it sends them to the parks list to fill their bucket list
                try
                {
                    var userBl = _context.UserParks.Where(b => b.CurrentUserId == userId).First();
                    var userInfo = _context.UserInfo.Where(x => x.OwnerId == userId).First();
                    if (userBl != null && userInfo != null)
                    {
                        return View("FullUser"); //buttons to see parks, button to see BucketList, update user info button. parks youve visited button
                    }
                    if (userInfo != null)
                    {
                        return RedirectToAction("Index", "ParksDb");

                    }
                }
                //if they do not have any info in then they get sent to the page to populate their information
                catch
                {
                    return RedirectToAction("AddUserInput", "User");
                }

            }
            return View();
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
