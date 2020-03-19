using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace ParksAndDeath.Models
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public bool? Smoker { get; set; }
        public bool? Drinker { get; set; }

        public virtual AspNetUsers Owner { get; set; }
    }
}
