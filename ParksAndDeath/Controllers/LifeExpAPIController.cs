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
        public async Task<IActionResult> LifeExpectancyCalc(string Country, int Age, string Sex, int year, bool smokes, bool drinks)
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
            
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://apps.who.int/gho/athena/api/GHO/");
            var response = await client.GetAsync($"LIFE_0000000035.json?filter=COUNTRY:{Country};Agegroup:{ageGroup};SEX:{Sex};YEAR:{year}");
            var life = await response.Content.ReadAsAsync<LifeRootobject>();
            double timeLeft = life.fact[0].value.numeric;
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int blCount = _context.UserParks.Where(x => x.CurrentUserId == id).ToList().Count;

            if(smokes == true)
            {
                timeLeft = timeLeft - 10;
            }
            if(drinks == true)
            {
                timeLeft = timeLeft - 3;
            }
            double lifeCalc = Math.Ceiling((blCount / timeLeft));

            TempData["lifeCalc"] = lifeCalc;
            return RedirectToAction("UserPreferences", "User");

        }

        [HttpGet]
        //public IActionResult UserPreferences()
        //{

        //}
        //public IActionResult ParkPlanFeasibilitySummary(LifeRootobject lifeExpectancy)
        //{

        //}
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}