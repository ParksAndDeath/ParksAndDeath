using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        //method to calculate remaining number of years to live if user is a smoker
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

        //creates a list of available datetimes which can be chosen from and assigned to a park in the users bucketlist
        public List<DateTime>  CreateDatetimes(DateTime StartYear, DateTime EndYear, int daysApart)
        {
           List<DateTime> dateTimes = new List<DateTime>();
            for (var dt = StartYear; dt < EndYear; dt = dt.AddDays(daysApart))
            {
                dateTimes.Add(dt);
            }

            return dateTimes;
        }

        public bool EnoughDates(int numDates, int numBklItems)
        {
            if(numDates >= numBklItems) 
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        //method to calculate remaining number of years to live if user is a drinker
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

        //method to determine Agegroup of user based on Age specified to prep for API call
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

        public IActionResult CheckUserPrefs()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserPreferences prefFound = _context.UserPreferences.Where(x => x.CurrentUserId == id).First();

            if (prefFound != null && DateTime.Now <= prefFound.StartYear && prefFound.EndYear > prefFound.StartYear)
            {
                //get the number of items in the users bucketlist
                int count = _context.UserParks.Where(x => x.CurrentUserId == id).Where(y => y.ParkVisited == false).Count();
                List<UserParks> userParks = _context.UserParks.Where(x => x.CurrentUserId == id).Where(y => y.ParkVisited == false).ToList();
                
                //Create a list of dateTimes to assign to parks bucket list based on start and end year entered by user
                int daysApart = (prefFound.EndYear - prefFound.StartYear).Days;
                /*List<DateTime>*/
                var dates = CreateDatetimes(prefFound.StartYear, prefFound.EndYear, 14);
                List<DateTime> dateTimes = CreateDatetimes(prefFound.StartYear, prefFound.EndYear, 14);

                if (EnoughDates(dates.Count, count) == true)
                {
                    ParksSummaryWithUserPrefs newSummary = new ParksSummaryWithUserPrefs();
                    
                    var numYearsRemain = (int)TempData["LifeCalc"];
                    newSummary.numYearsRemaining = numYearsRemain;
                    newSummary.listOfDateTimes = dateTimes;
                    newSummary.preferences = prefFound;
                    newSummary.bucketListCount = count;
                    newSummary.bucketedParks = userParks;
                    //TempData["BuckSummary"] = (ParksSummaryWithUserPrefs)newSummary;
                    return View("UserParkVisitSummary", newSummary);
                }
                else
                {
                    ViewBag.deleteSomeParks = $"Please delete {count - dates.Count} from your bucket list to meet this goal, or Click the link to Update Your Park Visiting Preferences:";
                    return RedirectToAction("DisplayBucketList", "ParksDb");
                }
            }

            else
            {
                TempData["usersPreferences"] = prefFound;
                return RedirectToAction("UserPreferences", "User");
            }
        }

        public IActionResult UserParkVisitSummary(ParksSummaryWithUserPrefs summaryInfo)
        {
            //ParksSummaryWithUserPrefs summary = (ParksSummaryWithUserPrefs)TempData["BucksSummary"];
            return View(summaryInfo);
        }

    }
}