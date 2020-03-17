using System;
using System.Collections.Generic;

namespace ParksAndDeath.Models
{
    public partial class Parks
    {
        public int ParksId { get; set; }
        public string FullName { get; set; }
        public string City { get; set; }
        public string States { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Url { get; set; }
        public string ParkCode { get; set; }
        public string Designation { get; set; }
    }
}
