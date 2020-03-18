using System;
using System.Collections.Generic;

namespace ParksAndDeath.Models
{
    public partial class ParksVisited
    {
        public int VisitedParkId { get; set; }
        public string ParkName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string ParkCode { get; set; }
        public string CurrentUserId { get; set; }

        public virtual AspNetUsers CurrentUser { get; set; }
    }
}
