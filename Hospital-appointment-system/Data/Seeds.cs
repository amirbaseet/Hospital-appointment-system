using Hospital_appointment_system.Models;
using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Hospital_appointment_system.Data
{
	//deffining intial data for the data base  
	public class Seeds
	{
		private readonly UserManager<PatientUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public Seeds(UserManager<PatientUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;

		}
		public static void SeedData(IApplicationBuilder applicationBuilder)
		{

			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
				context.Database.EnsureCreated();


				// Seed Departments
				if (!context.Departments.Any())
				{
					context.Departments.AddRange(new List<Departments>
					{
					 new Departments { Name = "Cardiology", Description = "Heart related diseases and treatments" },
					 new Departments { Name = "Neurology", Description = "Brain and nervous system related diseases and treatments" }
					});
					context.SaveChanges();
				}

				// Seed Clinics
				if (!context.Clinic.Any())
				{
					context.Clinic.AddRange(new List<Clinic>
				  {
					new Clinic { DepartmentID = 1, Name = "Heart Clinic", Location = "Building A, Floor 3" },
					new Clinic { DepartmentID = 2, Name = "Brain Health Clinic", Location = "Building B, Floor 2" }
				  });
					context.SaveChanges();
				}
				// Seed Doctors Hours
				if (!context.Doctors.Any())
				{
					context.Doctors.AddRange(new List<Doctor>
					{
					  new Doctor {  Name = "Dr. Eren Yılmaz", Specialization = "Cardiologist", ClinicID = 1 },
					  new Doctor {  Name = "Dr. Ayşe Güneş", Specialization = "Neurologist", ClinicID = 2 }
					});
					context.SaveChanges();
				}
			}
		}




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
						UserName = adminUserEmail,
						Name = "AmroBASEET",
						Email = adminUserEmail,
						Gender = Enum.GenderCategory.male,
						EmailConfirmed = true,

					};
					await userManager.CreateAsync(newAdminUser, "sau123");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
					adminUserEmail = "B201210560@sakarya.edu.tr";
					newAdminUser = new PatientUser()
					{
						Name = "SuhaibOTHMAN",
						Email = adminUserEmail,
						UserName = adminUserEmail,
						Gender = Enum.GenderCategory.male,
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
						Name = "app-user",
						UserName = appUserEmail,
						Email = appUserEmail,
						Gender = Enum.GenderCategory.male,
						EmailConfirmed = true,
					};
					await userManager.CreateAsync(newAppUser, "sau123");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
				}
			}
		}
		public async Task<IdentityResult> RegisterPatientUserAsync(RegisterViewModel model)
		{
			var user = new PatientUser
			{
                Name = model.Username,
				UserName = model.EmailAddress,
				Email = model.EmailAddress,
				Gender = model.Gender,
				EmailConfirmed = true  // or set based on your application logic
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			// Optionally add user to a role
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, UserRoles.User);  // Assign a default role or based on model
			}

			return result;
		}
		public async Task<IdentityResult> RegisterPatientAdminAsync(RegisterViewModel model)
		{
			var user = new PatientUser
			{
                Name = model.Username,
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
				Gender = model.Gender,
				EmailConfirmed = true  // or set based on your application logic
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			// Optionally add user to a role
			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, UserRoles.Admin);  // Assign a default role or based on model
			}

			return result;
		}

	}

}