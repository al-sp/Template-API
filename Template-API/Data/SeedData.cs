using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template_API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedUsers(userManager);
            await SeedRoles(roleManager);
        }

        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@localhost.com") == null)
            {
                var user = new IdentityUser
                {
                    Email = "admin@localhost.com",
                    UserName = "admin"
                };

                var result = await userManager.CreateAsync(user, "P@assword1!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }

            if (await userManager.FindByEmailAsync("customer@localhost.com") == null)
            {
                var user = new IdentityUser
                {
                    Email = "customer@localhost.com",
                    UserName = "customer"
                };

                var result = await userManager.CreateAsync(user, "P@assword1!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };

                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };

                await roleManager.CreateAsync(role);
            }

        }
    }
}
