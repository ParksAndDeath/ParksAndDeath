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
            ViewBag.userInfoMessage = "PLEASE TAKE A MOMENT AND ENTER THE INFORMATION BELOW:";
            return View();
        }

        [HttpPost]
        public IActionResult AddUserInput(UserInfo userInfo)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //checks if the user already has info

            if(ModelState.IsValid)
            {
                DateTime dob = (DateTime)userInfo.Dob;
                var today = DateTime.Today;
                var age = today.Year - dob.Year;
                if (dob.Date > today.AddYears(-age))
                {
                    age--;
                }

                userInfo.Age = age;
                userInfo.OwnerId = id;
                _context.UserInfo.Add(userInfo);
                _context.SaveChanges();
                ViewBag.userName = (string)userInfo.Name;
                return RedirectToAction("Index", "Home");
            }

            else 
            {
                ViewBag.userInfoMessage = "SOMETHING WENT WRONG.... PLEASE MAKE SURE ALL INFORMATION IS ENTERED BELOW....";
                return View("AddUserInput");
            }
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

            ViewBag.updateUserError = "USER NOT FOUND... PLEASE CLICK LINK TO ADD USER INFO BEFORE TRYING TO UPDATE";
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
                ViewBag.userName = (string)found.Name;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.updateUserError = "USER NOT FOUND... PLEASE CLICK LINK TO ADD USER INFO BEFORE TRYING TO UPDATE";
            return View("UpdateUserInfo");
        }

        [HttpGet]
        public IActionResult UserPreferences()
        {

            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                UserPreferences prefFound = _context.UserPreferences.Where(x => x.CurrentUserId == id).First();
                return RedirectToAction("UseCurrentPreferences");
            }
            catch
            {
                ViewBag.messageforPrefs = "PLEASE ENTER YOUR VISITING PREFERENCES BELOW:";
                return View("UserPreferences");
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

                return RedirectToAction("LifeExpectancyCalc", "LifeExpAPI");
            }
            return View();
        }

        //Gets the form to modify user preferences if Start date is less than 
        [HttpGet]
        public IActionResult UseCurrentPreferences()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserPreferences prefFound = _context.UserPreferences.Where(x => x.CurrentUserId == id).First();

            //get the user preference object obtained and stored in tempdata object and cast it as a UserPreferences object
            //UserPreferences currentPrefs = (UserPreferences)TempData["preferenceUpdate"];
            ViewBag.messageforPrefs = "PLEASE UPDATE YOUR VISITING PREFERENCES BELOW:";
            return View("UseCurrentPreferences",prefFound);
            //return View(currentPrefs);
        }

        [HttpPost]
        public IActionResult UpdateUserPreferences(UserPreferences updatedPrefs)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserPreferences found = _context.UserPreferences.Where(x => x.CurrentUserId == id).First();

            if (found != null)
            {
                found.StartYear = updatedPrefs.StartYear;
                found.EndYear = updatedPrefs.EndYear;
                found.Frequency = updatedPrefs.Frequency;
                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                _context.SaveChanges();
                return RedirectToAction("LifeExpectancyCalc", "LifeExpAPI");
            }
            else
            {
                ViewBag.messageforPrefs = "NO PREFERENCES FOUND.... PLEASE ADD YOUR PARK VISIT PREFERENCES BELOW";
                return View("UserPreferences");
            }
        }
    }
}