using SuperAdmin.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace SuperAdmin.IdentityDataSeeder
{
    public static class IdentityDataSeeders
    {
        public static async Task SeedAdminUserAndRole(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<SuperAdminUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create the "Admin" role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create the admin user if it doesn't exist
            var adminUser = await userManager.FindByNameAsync("admin@gmail.com");
            if (adminUser == null)
            {
                var user = new SuperAdminUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };


                var password = "Admin123@#$";
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }

}
