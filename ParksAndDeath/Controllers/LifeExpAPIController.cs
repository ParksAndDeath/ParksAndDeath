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
                if (found.Age < 5)
                {
                    ageGroup = "AGELT1";
                }
                if (found.Age >= 5 && found.Age <= 9)
                {
                    ageGroup = "AGE5-9";
                }
                if (found.Age >= 10 && found.Age <= 14)
                {
                    ageGroup = "AGE10-14";
                }
                if (found.Age >= 15 && found.Age <= 19)
                {
                    ageGroup = "AGE15-19";
                }
                if (found.Age >= 20 && found.Age <= 24)
                {
                    ageGroup = "AGE20-24";
                }
                if (found.Age >= 25 && found.Age <= 29)
                {
                    ageGroup = "AGE25-29";
                }
                if (found.Age >= 30 && found.Age <= 34)
                {
                    ageGroup = "AGE30-34";
                }
                if (found.Age >= 35 && found.Age <= 39)
                {
                    ageGroup = "AGE35-39";
                }
                if (found.Age >= 40 && found.Age <= 44)
                {
                    ageGroup = "AGE40-44";
                }
                if (found.Age >= 45 && found.Age <= 49)
                {
                    ageGroup = "AGE45-49";
                }
                if (found.Age >= 50 && found.Age <= 54)
                {
                    ageGroup = "AGE50-54";
                }
                if (found.Age >= 55 && found.Age <= 59)
                {
                    ageGroup = "AGE55-59";
                }
                if (found.Age >= 60 && found.Age <= 64)
                {
                    ageGroup = "AGE60-64";
                }
                if (found.Age >= 65 && found.Age <= 69)
                {
                    ageGroup = "AGE65-69";
                }
                if (found.Age >= 70 && found.Age <= 74)
                {
                    ageGroup = "AGE70-74";
                }
                if (found.Age >= 75 && found.Age <= 79)
                {
                    ageGroup = "AGE75-79";
                }

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