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

        //creates a list of available datetimes which can be chosen from and assigned to a park in the users bucketlist
        public List<DateTime> CreateDatetimes(DateTime StartYear, DateTime EndYear, int daysApart)
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
            if (numDates >= numBklItems)
            {
                return true;
            }

            else
            {
                return false;
            }
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
                    newSummary.listOfDateTimes = dateTimes;
                    newSummary.preferences = prefFound;
                    newSummary.bucketListCount = count;
                    newSummary.bucketedParks = userParks;
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

        public IActionResult MarkAsScheduled(int id)
        {
            UserParks found = _context.UserParks.Where(x => x.UsersParkIds == id).First();
            found.ParkVisited = null;
            _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(found);
            _context.SaveChanges();

            return RedirectToAction("CheckUserPrefs");
        }

        public IActionResult GetScheduledParks()
        {
            string Userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<UserParks> userParks = _context.UserParks.OrderBy(w => w.ParkName).Where(x => x.CurrentUserId == Userid).Where(y => y.ParkVisited == null).ToList();

            return View("GetScheduledParks", userParks);
        }
    }
}