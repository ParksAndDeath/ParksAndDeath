using System;
using System.Collections.Generic;

namespace ParksAndDeath.Models
{
    public partial class UserParks
    {
        public int UsersParkIds { get; set; }
        public string ParkName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ParkCode { get; set; }
        public decimal? Cost { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool? ParkVisited { get; set; }
        public string Url { get; set; }
        public string CurrentUserId { get; set; }

        public virtual AspNetUsers CurrentUser { get; set; }
    }
}
