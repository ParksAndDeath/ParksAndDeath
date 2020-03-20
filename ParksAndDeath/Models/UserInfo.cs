using System;
using System.Collections.Generic;

namespace ParksAndDeath.Models
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Smoker { get; set; }
        public string Drinker { get; set; }
        public string OwnerId { get; set; }

        public virtual AspNetUsers Owner { get; set; }
    }
}
