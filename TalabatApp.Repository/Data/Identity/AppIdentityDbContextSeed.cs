using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Entities.Identity;

namespace TalabatApp.Repository.Data.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Gomaa",
                    Email = "Ahmed321@Route.com",
                    UserName = "ahmed gomaa",
                    PhoneNumber = "01111111111"
                };

                await _userManager.CreateAsync(user, "P@ssW0rd");
            }
        }
    }
}
