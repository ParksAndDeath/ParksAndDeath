using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParksAndDeath.Models;

namespace ParksAndDeath.Controllers
{
    [Authorize]
    public class LifeExpAPIController : Controller
    {
        private readonly ParksAndDeathDbContext _context;
        public LifeExpAPIController(ParksAndDeathDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> LifeExpectancyCalc(UserInfo userInfo)
        {
            int year = 2016;
            int Userid = userInfo.UserId;

            UserInfo found = _context.UserInfo.Where(x => x.UserId == Userid).First();

            if (found != null)
            {
                string ageGroup = "";
              
             

                //calling the API
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://apps.who.int/gho/athena/api/GHO/");
                var response = await client.GetAsync($"LIFE_0000000035.json?filter=COUNTRY:{found.Country};Agegroup:{ageGroup};SEX:{found.Gender};YEAR:{year}");//specify the endpoint we want to use
                var life = await response.Content.ReadAsAsync<LifeRootobject>(); //this is where the data is parsed to the appropriate class
                double timeLeft = life.fact[0].value.numeric;

                if (found.Smoker == true)
                {
                    timeLeft = timeLeft - 10;
                }
                if (found.Drinker == true)
                {
                    timeLeft = timeLeft - 3;
                }

                string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                UserPreferences prefFound = _context.UserPreferences.Where(x => x.CurrentUserId == id).First();

                if (prefFound != null && DateTime.Now > prefFound.StartYear)
                {
                    TempData["lifeCalc"] = timeLeft;
                    TempData["CurrentPrefs"] = prefFound;
                    return View("UseCurrentPreferences", "User");
                }

                else
                {
                    TempData["usersPreferences"] = prefFound;
                    return RedirectToAction("UserPreferences", "User");
                }
            }

            else
            {
                ViewBag["message"] = "Oooops.... we don't have your Profile info.  Fill it out below:";
                return RedirectToAction("AddUserInput", "User");
            }

        }

    }
}