using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParksAndDeath.Models
{
    public partial class UserInfo
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "NAME IS REQUIRED AND MUST BE BETWEEN 3 AND 100 CHARACTERS")]
        public string Name { get; set; }

        [Required(ErrorMessage = "PLEASE SPECIFY YOUR DATE OF BIRTH")]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "PLEASE SPECIFY YOUR GENDER FROM THE DROP DOWN BELOW")]
        public string Gender { get; set; }
        public bool? Smoker { get; set; }
        public bool? Drinker { get; set; }
        public string OwnerId { get; set; }

        [Required(ErrorMessage ="PLEASE SPECIFY THE COUNTRY YOU LIVE IN FROM THE DROP DOWN BELOW")]
        public string Country { get; set; }

        public int? Age { get; set; }

        public virtual AspNetUsers Owner { get; set; }
    }
}
