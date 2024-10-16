using Microsoft.AspNetCore.Identity;
using Models.Dtos;
using Models.Entities;
namespace Window.Extenstion
{
    public static class ApplicationDbIntializer
    {
        public static async System.Threading.Tasks.Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Models.Dtos.Role>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Seed Roles
            string[] roleNames = { "Admin", "Customer", "Empolyee" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Models.Dtos.Role { Name = roleName });
                }
            }

            // Seed Admin User
            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new User
                {
                    UserName = adminEmail.Substring(0, adminEmail.IndexOf("@")),
                    Email = adminEmail,
                    FirstName = "admin",
                    LastName = "admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
