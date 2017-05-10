using ContosoU2016.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Data
{
    public class AdministratorSeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministratorSeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {
            if (await _userManager.FindByEmailAsync("admin@contoso.com") == null)
            {


                // Create admin role
                var adminRole = await _roleManager.FindByNameAsync("admin"); // does role exist? 
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("admin");
                    await _roleManager.CreateAsync(adminRole);
                }
                // Create admin user
                ApplicationUser adminUser = new ApplicationUser
                {
                    UserName = "admin@contoso.com",
                    Email = "admin@contoso.com"
                };

                await _userManager.CreateAsync(adminUser, "Admin@123456");
                await _userManager.SetLockoutEnabledAsync(adminUser, false); //admin cannot be locked out. 

                //assign admin to admin role
                IdentityResult result = await _userManager.AddToRoleAsync(adminUser, "admin");
            }

            // Create student role
            var studentRole = await _roleManager.FindByNameAsync("student");
            if(studentRole == null)
            {
                studentRole = new IdentityRole("student");
                await _roleManager.CreateAsync(studentRole);
            }
            // Create instructor user
            var instructorRole = await _roleManager.FindByNameAsync("instructor");
            if (instructorRole == null)
            {
                instructorRole = new IdentityRole("instructor");
                await _roleManager.CreateAsync(instructorRole);
            }
        }
    }
}
