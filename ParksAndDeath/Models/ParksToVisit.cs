using System;
using System.Collections.Generic;

namespace ParksAndDeath.Models
{
    public partial class ParksToVisit
    {
        public int DesiredParkId { get; set; }
        public string ParkName { get; set; }
        public string AAddress { get; set; }
        public string SState { get; set; }
        public string ParkCode { get; set; }
        public decimal? Cost { get; set; }
        public int CurrentUserId { get; set; }

        public virtual UserInfo CurrentUser { get; set; }
    }
}
