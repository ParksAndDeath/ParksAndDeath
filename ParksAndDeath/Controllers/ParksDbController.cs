using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParksAndDeath.Models;

namespace ParksAndDeath.Controllers
{
    public class ParksDbController : Controller
    {
        private readonly ParksAndDeathDbContext _context;
        public ParksDbController(ParksAndDeathDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //creating an Iaction to display the users bucket list.
        public IActionResult DisplayBucketList()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(_context.ParksToVisit.Where(x => x.CurrentUserId == id).ToList());
        }
    }
}