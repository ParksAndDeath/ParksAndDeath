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

                var userInfo = CheckUserInfo(_context, userId);
                var userBl = CheckBucketList(_context, userId);

                if (userBl != false && userInfo != false)
                {


                    return View("FullUser"); //buttons to see parks, button to see BucketList, update user info button. parks youve visited button
                }
                if (userInfo != false)
                {
                    return View("FullUser", "ParksDb");

                }
                return RedirectToAction("AddUserInput", "User");
            }
            return View();
        }

        public async Task<IActionResult> LifeExpectancyCalc()
        {
            int year = 2016;

            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            UserInfo found = _context.UserInfo.Where(x => x.OwnerId == id).First();

            if (found != null)
            {
                //get the ageGroup that corresponds to the options available in the API using the calculated age in the database
                string ageGroup = GetAgeGroup((int)found.Age);

                //we create an new HttpClient
                //calling the API
                var client = new HttpClient();

                //specify the base address
                client.BaseAddress = new Uri("http://apps.who.int/gho/athena/api/GHO/");

                //specify the endpoint we want to use in our API call
                var response = await client.GetAsync($"LIFE_0000000035.json?filter=COUNTRY:{found.Country};Agegroup:{ageGroup};SEX:{found.Gender};YEAR:{year}");

                //parse the json into the appropriate class in our models
                var life = await response.Content.ReadAsAsync<LifeRootobject>();

                int timeLeft = (int)Math.Round((double)life.fact[0].value.numeric);

                timeLeft = Smoker((bool)found.Smoker, timeLeft);

                timeLeft = Drinker((bool)found.Drinker, timeLeft);

                TempData["lifeCalc"] = timeLeft;
                return RedirectToAction("CheckUserPrefs");
            }
            ViewBag.message = "Oooops.... we don't have your Profile info.  Fill it out below:";
            return RedirectToAction("AddUserInput", "User");
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
                context.UserParks.Where(x => x.CurrentUserId == id).Where(y => y.ParkVisited == false).First();
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

