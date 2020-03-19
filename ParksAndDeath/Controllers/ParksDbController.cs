﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParksAndDeath.Models;

namespace ParksAndDeath.Controllers
{
    [Authorize]
    public class ParksDbController : Controller
    {
        private readonly ParksAndDeathDbContext _context;
        public ParksDbController(ParksAndDeathDbContext context)
        {
            _context = context;
        }

        public IActionResult ParksVisited()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<UserParks> userParks = _context.UserParks.Where(x => x.CurrentUserId == id).ToList();
            List<UserParks> visitedParks = new List<UserParks>();
            foreach(UserParks park in userParks)
            {
                if(park.ParkVisited == true)
                {
                    visitedParks.Add(park);
                }
            }
            return View(visitedParks);
        }

        //diplays a list of all the parks
        public IActionResult Index()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Parks> fullList = _context.Parks.OrderBy(x => x.ParkCode).ToList();
            List<string> parcodes = _context.UserParks.Select(x => x.ParkCode).ToList();
          
            List<Parks> parksAvailable = new List<Parks>();
            ////if the park isn't included in the bucketlist it will display the whole list from the database
            if (parcodes.Count == 0)
            {
                parksAvailable = fullList;
                return View(parksAvailable);

            }
            else //this will loop through the full list and check if each item is in the bucketlist
            {
                for (int i = 0; i < fullList.Count; i++)
                {
                    if (!(parcodes.Contains(fullList[i].ParkCode)))
                    {
                        parksAvailable.Add(fullList[i]);
                    }
                }
                return View(parksAvailable);
            }
        }
        //creating an Iaction to display the users bucket list.
        public IActionResult DisplayBucketList()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(_context.UserParks.Where(x => x.CurrentUserId == id).ToList());
        }

        public IActionResult AddParkToBuckList(string name, string city, string state, string latitude, string longitude, string url, string parkcode)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserParks park = new UserParks();
            park.ParkName = name;
            park.City = city;
            park.State = state;
            park.Latitude = latitude;
            park.Longitude = longitude;
            park.Url = url;
            park.ParkCode = parkcode;
            park.CurrentUserId = id;

            if (ModelState.IsValid)
            {
                _context.UserParks.Add(park);
                _context.SaveChanges();


                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}