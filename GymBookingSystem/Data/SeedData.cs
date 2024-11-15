using GymBookingSystem.Data;
using GymBookingSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace GymBooking.Data
{
    public class SeedData
    {
        private static ApplicationDbContext context = default!;
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<ApplicationUser> userManager = default!;


        // services parametern skickas från program.cs. Den förser denna classen md dependency injection containern.
        public static async Task InitAsync(ApplicationDbContext _context, IServiceProvider services)
        {
            context = _context;

            // Kollar om det finns några entries i AspNetRoles tabellen har några entries.
            // Om det finns några roller där så förhindras återseedning.
            if (context.Roles.Any()) return;

            // När man väljer "Individual Accounts" när projektet startas så scaffoldas identity features.
            // DI-containern får då en setup med default implementation av RoleManager och UserManager 
            // som konfigureras i AddIdentity eller AddDefaultIdentity i program.cs.
            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var roleNames = new[] { "User", "Admin" };
            var adminEmail = "admin@gymbokning.se";

            await AddRolesAsync(roleNames);

            var admin = await AddAccountAsync(adminEmail, "Admin", "Adminsson", "P@55w.rd");

            await AddUserToRoleAsync(admin, "Admin");

        }

        // Denna metod skapar en ny entry i kopplingstabellen "AspNetUserRoles" tabellen.
        // Den skapar en composite primary key bestående utav: (User.Id Role.Id)
        // Entryn i AspNetUserRoles består av den inskickade användarens id (taget från AspNetUser-tabellen)...
        // samt id:et som den inskickade rollen har blivit tilldelad i (AspNetRoles-tabellen).
        private static async Task AddUserToRoleAsync(ApplicationUser user, string role)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        // Denna metod populerar AspNetRoles tabellen med värdena från arrayen som skickas in i param.
        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        // Denna metod populerar populerar AspNetUsers tabellen med en ny entry om inte den inskickade emailen redan finns där.
        private static async Task<ApplicationUser> AddAccountAsync(string accountEmail, string fName, string lName, string password)
        {
            var found = await userManager.FindByEmailAsync(accountEmail);

            if (found != null) return null!;

            var user = new ApplicationUser
            {
                UserName = accountEmail,
                Email = accountEmail,
                EmailConfirmed = true,
                FirstName = fName,
                LastName = lName,
                TimeOfRegistration = DateTime.Now
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return user;

        }
    }
}