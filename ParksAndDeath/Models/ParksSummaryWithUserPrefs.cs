using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAndDeath.Models
{
    public class ParksSummaryWithUserPrefs
    {
        public List<UserParks> bucketedParks = new List<UserParks>();
        public List<DateTime> listOfDateTimes = new List<DateTime>();
        //public IEnumerable<DateTime> forDateDropDown = new IEnumerable<DateTime>();
        
        public int numYearsRemaining { get; set; }
        public UserPreferences preferences { get; set; }
        public int bucketListCount { get; set; }
    }

    
}
