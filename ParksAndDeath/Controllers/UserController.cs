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
            return View();
        }
        [HttpPost]
        public IActionResult UserPreferences(UserPreferences userPreferences)
        {
            //getting id of currently logged in user
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //Do the names of the inputs in the form match the properties in our model
            if (ModelState.IsValid)
            {
                //setting the currentUserId to the id of the user logged in
                userPreferences.CurrentUserId = id;
                _context.UserPreferences.Add(userPreferences);
                _context.SaveChanges();
                return View("ViewUserPreferences", userPreferences);
            }
            return View();
        }
    }
}