using System;
using System.Collections.Generic;

namespace ParksAndDeath.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            ParksToVisit = new HashSet<ParksToVisit>();
            ParksVisited = new HashSet<ParksVisited>();
        }

        public int UserId { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public bool? Smoker { get; set; }
        public bool? Drinker { get; set; }

        public virtual AspNetUsers Owner { get; set; }
        public virtual ICollection<ParksToVisit> ParksToVisit { get; set; }
        public virtual ICollection<ParksVisited> ParksVisited { get; set; }
    }
}
