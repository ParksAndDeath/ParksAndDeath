using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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

        public async Task<IActionResult> Index()
        {
            //checking if user is logged in
            if (_signInManager.IsSignedIn(User))
            {
                //identify the user that is logged in
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
           
                UserInfo checkValue = _context.UserInfo.Where(x => x.OwnerId == userId).First();
               //checks if the user has their information filled in and if they do, it sends them to the parks list to fill their bucket list
                
             
                var userInfo = CheckUserInfo(_context, userId);
                var userBl = CheckBucketList(_context, userId);

                if (userBl != false && userInfo != false)
                {
                    return View("FullUser", checkValue); //buttons to see parks, button to see BucketList, update user info button. parks youve visited button
                }
                if (userInfo != false)
                {
                    if (checkValue.LifeExpectancy == 0)
                    {
                        await LifeExpectancyCalc();
                    }
                    return RedirectToAction("FullUser", checkValue);
                    //return RedirectToAction("Index", "ParksDb");
                }
                return RedirectToAction("AddUserInput", "User");
            }
            return View();
        }

<<<<<<< HEAD
        //public async Task<IActionResult> LifeExpectancyCalc()
        //{
        //    int year = 2016;
=======
        public async Task LifeExpectancyCalc()
        {
            int year = 2016;
>>>>>>> reggie

        //    string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    UserInfo found = _context.UserInfo.Where(x => x.OwnerId == id).First();

        //    if (found != null)
        //    {
        //        //get the ageGroup that corresponds to the options available in the API using the calculated age in the database
        //        string ageGroup = GetAgeGroup((int)found.Age);

        //        //we create an new HttpClient
        //        //calling the API
        //        var client = new HttpClient();

        //        //specify the base address
        //        client.BaseAddress = new Uri("http://apps.who.int/gho/athena/api/GHO/");

        //        //specify the endpoint we want to use in our API call
        //        var response = await client.GetAsync($"LIFE_0000000035.json?filter=COUNTRY:{found.Country};Agegroup:{ageGroup};SEX:{found.Gender};YEAR:{year}");

        //        //parse the json into the appropriate class in our models
        //        var life = await response.Content.ReadAsAsync<LifeRootobject>();

        //        int timeLeft = (int)Math.Round((double)life.fact[0].value.numeric);

        //        timeLeft = Smoker((bool)found.Smoker, timeLeft);

        //        timeLeft = Drinker((bool)found.Drinker, timeLeft);

<<<<<<< HEAD
        //        TempData["lifeCalc"] = timeLeft;
        //        return RedirectToAction("CheckUserPrefs");
        //    }
        //    ViewBag.message = "Oooops.... we don't have your Profile info.  Fill it out below:";
        //    return RedirectToAction("AddUserInput", "User");
        //}
=======
                found.LifeExpectancy = timeLeft;
                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                await _context.SaveChangesAsync();
            }
        }
>>>>>>> reggie

        public string GetAgeGroup(int Age)
        {
            string ageGroup = "";
            if (Age < 5)
            {
                ageGroup = "AGELT1";
            }
            if (Age >= 5 && Age <= 9)
            {
                ageGroup = "AGE5-9";
            }
            if (Age >= 10 && Age <= 14)
            {
                ageGroup = "AGE10-14";
            }
            if (Age >= 15 && Age <= 19)
            {
                ageGroup = "AGE15-19";
            }
            if (Age >= 20 && Age <= 24)
            {
                ageGroup = "AGE20-24";
            }
            if (Age >= 25 && Age <= 29)
            {
                ageGroup = "AGE25-29";
            }
            if (Age >= 30 && Age <= 34)
            {
                ageGroup = "AGE30-34";
            }
            if (Age >= 35 && Age <= 39)
            {
                ageGroup = "AGE35-39";
            }
            if (Age >= 40 && Age <= 44)
            {
                ageGroup = "AGE40-44";
            }
            if (Age >= 45 && Age <= 49)
            {
                ageGroup = "AGE45-49";
            }
            if (Age >= 50 && Age <= 54)
            {
                ageGroup = "AGE50-54";
            }
            if (Age >= 55 && Age <= 59)
            {
                ageGroup = "AGE55-59";
            }
            if (Age >= 60 && Age <= 64)
            {
                ageGroup = "AGE60-64";
            }
            if (Age >= 65 && Age <= 69)
            {
                ageGroup = "AGE65-69";
            }
            if (Age >= 70 && Age <= 74)
            {
                ageGroup = "AGE70-74";
            }
            if (Age >= 75 && Age <= 79)
            {
                ageGroup = "AGE75-79";
            }
            return (ageGroup);
        }

        public int Drinker(Boolean drinker, int timeLeft)
        {
            if (drinker == true)
            {
                timeLeft = timeLeft - 3;
                return timeLeft;
            }
            else
            {
                return timeLeft;
            }
        }

        public int Smoker(Boolean smoker, int timeLeft)
        {
            if (smoker == true)
            {
                timeLeft = timeLeft - 10;
                return timeLeft;
            }
            else
            {
                return timeLeft;
            }
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

