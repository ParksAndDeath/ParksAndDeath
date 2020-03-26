using System;
using System.Collections.Generic;

namespace ParksAndDeath.Models
{
    public partial class UserPreferences
    {
        public int PreferencesId { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public int Frequency { get; set; }
        public string CurrentUserId { get; set; }

        public virtual AspNetUsers CurrentUser { get; set; }
    }
}
