using ContactManager.Authorization;
using ContactManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -outDir Pages\Contacts --referenceScriptLibraries
namespace ContactManager.Data
{
    public static class SeedData
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        #region snippet_Initialize
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, "Sky@265694", "admin@contoso.com");
                await EnsureRole(serviceProvider, adminID, Constants.ContactAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, "Sky@265694", "manager@contoso.com");
                await EnsureRole(serviceProvider, managerID, Constants.ContactManagersRole);

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        #endregion
        #region snippet1
        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.Contact.Any())
            {
                return;   // DB has been seeded
            }

            context.Contact.AddRange(
            #region snippet_Contact
                new Contact
                {
                    Name = "Debra Garcia",
                    Address = "1234 Main St",
                    City = "Redmond",
                    State = "WA",
                    Zip = "10999",
                    Email = "debra@example.com",
                    Status = ContactStatus.Approved,
                    OwnerID = adminID
                },
            #endregion
            #endregion
                new Contact
                {
                    Name = "Akash Kumar",
                    Address = "166 Hanmer Street East",
                    City = "Barrie",
                    State = "Ontario",
                    Zip = "LM4 6w2",
                    Email = "200573251@Student.georgian.on.ca",
                    Status = ContactStatus.Approved,
                    OwnerID = adminID
                },
                new Contact
                {
                    Name = "Sukhjeet Kaur",
                    Address = "Cundles Street",
                    City = "Barrie",
                    State = "Ontario",
                    Zip = "L4M 6NM",
                    Email = "sukhjeetkaur.sukhjeetkaur@mygeorgian.ca",
                    Status = ContactStatus.Rejected,
                    OwnerID = adminID
                },
                new Contact
                {
                    Name = "Darshan Hemantkumar",
                    Address = "Duckworth street",
                    City = "Barrie",
                    State = "Ontario",
                    Zip = "L4M 6W1",
                    Email = "darshanhemantkumar.tahiliani@mygeorgian.ca",
                    Status = ContactStatus.Submitted,
                    OwnerID = adminID
                },
                new Contact
                {
                    Name = "Rohan Juneja",
                    Address = "Dunlop street",
                    City = "Barrie",
                    State = "Ontario",
                    Zip = "L4M3R3",
                    Email = "rohan.juneja@mygeorgian.ca",
                    Status = ContactStatus.Submitted,
                    OwnerID = adminID
                }
             );
            context.SaveChanges();
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
