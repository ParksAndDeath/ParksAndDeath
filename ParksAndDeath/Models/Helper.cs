using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParksAndDeath.Models
{
    public class Helper
    {
        public static bool CheckForUserInfo(UserManager<IdentityUser> userManager, ClaimsPrincipal user, ParksAndDeathDbContext context)
        {
            string id = userManager.GetUserId(user);
            try
            {
                var found = context.UserInfo.Where(x => x.OwnerId == id).First();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
