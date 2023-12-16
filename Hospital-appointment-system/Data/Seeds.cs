using Hospital_appointment_system.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Hospital_appointment_system.Data
{
    //deffining intial data for the data base  
    public class Seeds
    {
        //public static void SeedData(IApplicationBuilder applicationBuilder)
        //{
        //    using (var serviceScope=applicationBuilder.ApplicationServices.CreateScope())
        //    {
        //        var context = serviceScope.ServiceProvider.GetService <ApplicationDbContext >();
        //        context.Database.EnsureCreated();


        //        // Seed Departments
        //        if (!context.Departments.Any())
        //        {
        //          context.Departments.AddRange(new List<Departments>
        //          {
        //             new Departments { Name = "Cardiology", Description = "Heart related diseases and treatments" },
        //             new Departments { Name = "Neurology", Description = "Brain and nervous system related diseases and treatments" }
        //          });
        //            context.SaveChanges();
        //        }

        //        // Seed Clinics
        //        if (!context.Clinic.Any())
        //        {
        //          context.Clinic.AddRange(new List<Clinic>
        //          {
        //            new Clinic { DepartmentID = 1, Name = "Heart Clinic", Location = "Building A, Floor 3" },
        //            new Clinic { DepartmentID = 2, Name = "Brain Health Clinic", Location = "Building B, Floor 2" }
        //          });
        //             context.SaveChanges();
        //        }
        //        // Seed Doctors Hours
        //        if (!context.Doctors.Any())
        //        {
        //            context.Doctors.AddRange(new List<Doctor>
        //            {
        //              new Doctor {  Name = "Dr. Eren Yılmaz", Specialization = "Cardiologist", ClinicID = 1 },
        //              new Doctor {  Name = "Dr. Ayşe Güneş", Specialization = "Neurologist", ClinicID = 2 }
        //            });
        //            context.SaveChanges();
        //        }  
        //        // Seed Working Hours
        //        //if (!context.WorkingHours.Any())
        //        //{
        //        //    context.WorkingHours.AddRange(new List<WorkingHour>
        //        //    {
        //        //    new WorkingHour { DoctorID = 3, DayOfWeek = "Monday", StartTime = TimeSpan.Parse("08:00"), EndTime = TimeSpan.Parse("16:00") },
        //        //    new WorkingHour { DoctorID = 4, DayOfWeek = "Monday", StartTime = TimeSpan.Parse("09:00"), EndTime = TimeSpan.Parse("17:00") }
        //        //    });
        //        //    context.SaveChanges();
        //        //}
        //        // Seed Users
        //        if (!context.PatientUsers.Any())
        //        {
        //           context.PatientUsers.AddRange(new List<PatientUser>
        //        {
        //             new PatientUser {  UserName = "Ali", Password = "123", Email = "Ali@example.com"},
        //             new PatientUser { UserName = "Ola", Password = "123", Email = "Ola@example.com"}
        //             });
        //            context.SaveChanges();
        //        }
        //        //// Seed Appointments
        //        //if (!context.Appointments.Any())
        //        //{
        //        //    context.Appointments.AddRange(new List<Appointment>
        //        //    {
        //        //    new Appointment { PatientUserID = 1, DoctorID = 1, Date = DateTime.Parse("2023-12-01"), Time = TimeSpan.Parse("10:00"), Status = "Scheduled" },
        //        //    new Appointment { PatientUserID = 123457, DoctorID = 2, Date = DateTime.Parse("2023-12-02"), Time = TimeSpan.Parse("11:00"), Status = "Scheduled" }
        //        //     });
        //        //    context.SaveChanges();
        //        //}

        //        // Seed Admins
        //        if (!context.AdminUser.Any())
        //        {
        //            context.AdminUser.AddRange(new List<AdminUser>
        //            {
        //            new AdminUser {  Password = "sau", Email = "B201210560@sakarya.edu.tr" },
        //             });
        //            context.SaveChanges();
        //        }

        //    }

        //}
        //public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        //{
        //    using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        //    {
        //        //Roles
        //        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        //        if (!await roleManager.RoleExistsAsync(UserRoles.User))
        //            await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        //        //Users
        //        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<PatientUser>>();
        //        string adminUserEmail = "teddysmithdeveloper@gmail.com";

        //        var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
        //        if (adminUser == null)
        //        {
        //            var newAdminUser = new PatientUser()
        //            {
        //                UserName = "teddysmithdev",
        //                Email = adminUserEmail,
        //                EmailConfirmed = true,
        //            };
        //            await userManager.CreateAsync(newAdminUser, "Coding@1234?");
        //            await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
        //        }

        //        string appUserEmail = "user@etickets.com";

        //        var appUser = await userManager.FindByEmailAsync(appUserEmail);
        //        if (appUser == null)
        //        {
        //            var newAppUser = new PatientUser()
        //            {
        //                UserName = "app-user",
        //                Email = appUserEmail,
        //                EmailConfirmed = true,
        //                      };
        //            await userManager.CreateAsync(newAppUser, "Coding@1234?");
        //            await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
        //        }
        //    }
        //}
        //public static void SeedPatientUsers(UserManager<PatientUser> userManager)
        //{
        //    if (!userManager.Users.Any())
        //    {
        //        var users = new List<PatientUser>
        //    {
        //        new PatientUser { UserName = "amer@example.com", Email = "amer@example.com" },
        //        // Add more users as needed
        //    };

        //        foreach (var user in users)
        //        {
        //            userManager.CreateAsync(user, "123").Wait();
        //        }
        //    }
        //}





        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<PatientUser>>();
                string adminUserEmail = "G211210578@sakarya.edu.tr";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new PatientUser()
                    {
                        UserName = "AmroBASEET",
                        Email = adminUserEmail,
                        Gender=Enum.GenderCategory.male ,
                        EmailConfirmed = true,

                    };
                    await userManager.CreateAsync(newAdminUser, "sau123");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                    adminUserEmail = "B201210560@sakarya.edu.tr";
                    newAdminUser = new PatientUser()
                    {
                        UserName = "SuhaibOTHMAN",
                        Email = adminUserEmail,
                        Gender=Enum.GenderCategory.male ,
                        EmailConfirmed = true,
                      
                    };
                    await userManager.CreateAsync(newAdminUser, "sau123");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new PatientUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        Gender=Enum.GenderCategory.male ,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "sau123");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}

