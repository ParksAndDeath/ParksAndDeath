using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParksAndDeath.Models
{
    public partial class UserPreferences
    {
        public int PreferencesId { get; set; }

        [Required]
        public DateTime StartYear { get; set; }

        [Required]
        public DateTime EndYear { get; set; }

        [Required]
        public int Frequency { get; set; }
        public string CurrentUserId { get; set; }

        public virtual AspNetUsers CurrentUser { get; set; }
    }
}
