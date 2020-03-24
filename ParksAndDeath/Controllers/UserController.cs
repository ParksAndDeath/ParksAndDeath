using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        [HttpGet]
        public IActionResult AddUserInput()
        {
            return View(new UserInfo());
        }

        [HttpPost]
        public IActionResult AddUserInput(UserInfo userInfo)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                DateTime dob = (DateTime)userInfo.Dob;
                int age = 0;
                age = DateTime.Now.Year - dob.Year;
                if (DateTime.Now.DayOfYear < dob.DayOfYear)
                    age = age - 1;

                userInfo.Age = age;
                userInfo.OwnerId = id;
                _context.UserInfo.Add(userInfo);
                _context.SaveChanges();
                return View("ViewUserProfileInformation", userInfo);
            }

            return View("AddUserInput");
        }

        [HttpGet]
        public IActionResult UpdateUserInfo()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserInfo found = _context.UserInfo.Where(x => x.OwnerId == id).First();

            if (found != null)
            {
                return View(found);
            }

            return RedirectToAction("UpdateUserInfo");
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(UserInfo userinfo)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserInfo found = _context.UserInfo.Where(x => x.OwnerId == id).First();

            if (found != null)
            {
                found.Name = userinfo.Name;
                found.Smoker = userinfo.Smoker;
                found.Drinker = userinfo.Drinker;
                found.Country = userinfo.Country;
                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                _context.SaveChanges();
                return View("ViewUserProfileInformation", found);
            }

            return View("UpdateUserInfo");
        }

        public IActionResult ViewUserProfileInformation(UserInfo userInfo)
        {
            return View(userInfo);
        }
        [HttpGet]
        public IActionResult UserPreferences()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserPreferences prefFound = _context.UserPreferences.Where(x => x.CurrentUserId == id).First();
            if (prefFound == null)
            {
                return View();
            }

            else
            {
                TempData["preferenceUpdate"] = prefFound;
                return RedirectToAction("UseCurrentPreferences");
            }
        }
        [HttpPost]
        public IActionResult UserPreferences(UserPreferences userPreferences)
        {

            //getting id of currently logged in user
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //Do the names of the inputs in the form match the properties in our model?
            if (ModelState.IsValid)
            {
                //setting the currentUserId to the id of the user logged in
                userPreferences.CurrentUserId = id;

                //Adding the specified UserPreferences from the form to the db
                _context.UserPreferences.Add(userPreferences);


                _context.SaveChanges();
                return View("ViewUserPreferences", userPreferences);
            }
            return View();
        }

        //Gets the form to modify user preferences if Start date is less than 
        [HttpGet]
        public IActionResult UseCurrentPreferences()
        {
            //get the user preference object obtained and stored in tempdata object and cast it as a UserPreferences object
            UserPreferences currentPrefs = (UserPreferences)TempData["preferenceUpdate"];
            return View(currentPrefs);
        }
    }
}