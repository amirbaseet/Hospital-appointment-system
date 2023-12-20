using Hospital_appointment_system.Data;
using Hospital_appointment_system.Interfaces;
using Hospital_appointment_system.Models;
using Hospital_appointment_system.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Hospital_appointment_system.Data.Enum;

namespace Hospital_appointment_system.Controllers
{
    public class PatientUserController : Controller
    {
        private readonly IPatientUserRepository _PatientUserRepository;
        private readonly ApplicationDbContext _context;
		private readonly UserManager<PatientUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
        public PatientUserController(IPatientUserRepository patientUserRepository, ApplicationDbContext context
            , UserManager<PatientUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _PatientUserRepository = patientUserRepository;
            _context = context;

			_userManager = userManager;
			_roleManager = roleManager;
        }

        //[Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Index(UserType userType = UserType.Patients)
        {
            IEnumerable<PatientUser> users;

            switch (userType)
            {
                case UserType.Patients:
                    users = await _userManager.GetUsersInRoleAsync(UserRoles.User);
                    break;
                case UserType.Admins:
                    users = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);
                    break;
                default:
                    users = await _userManager.Users.ToListAsync();
                    break;
            }

            return View(users);
		}
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ListPatients()
        {
            // Retrieve all users that are not in the Admin role
            var users = await _userManager.Users.ToListAsync();
            var patients = new List<PatientUser>();

            foreach (var user in users)
            {
                // Check if the user is not an admin
                if (!await _userManager.IsInRoleAsync(user, UserRoles.Admin))
                {
                    // User is not an admin, so we consider them as a patient
                    patients.Add(user);
                }
            }

            return View(patients); // Make sure you have a corresponding view to display the list of patients
        }
        [Authorize(Roles = UserRoles.Admin)]

        public async Task<IActionResult> ListAdmin()
        {
            // Retrieve all users that are not in the Admin role
            var users = await _userManager.Users.ToListAsync();
            var patients = new List<PatientUser>();

            foreach (var user in users)
            {
                // Check if the user is not an admin
                if (!await _userManager.IsInRoleAsync(user, UserRoles.User))
                {
                    // User is not an admin, so we consider them as a patient
                    patients.Add(user);
                }
            }

            return View(patients); // Make sure you have a corresponding view to display the list of patients

        }

      
        // GET: User/Create
        [HttpGet]
        //[Authorize(Roles = UserRoles.Admin)]
        public IActionResult Create()
		{
			return View();
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create(RegisterViewModel patientUser)
        {
            if (ModelState.IsValid)
            {
                if (await _PatientUserRepository.CheckUserbyEmail(patientUser.EmailAddress))
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(patientUser);
                }
                var user = new PatientUser
                {
                    UserName = patientUser.Username,
                    Email = patientUser.EmailAddress,
                    Gender = patientUser.Gender,
                    EmailConfirmed = true  // or set based on your application logic
                };

                var result = await _userManager.CreateAsync(user, patientUser.Password);

                // Optionally add user to a role
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);  // Assign a default role or based on model
                }

                return RedirectToAction(nameof(ListPatients));
            }
            TempData["Error"] = "entered information is not correct";
            // If we reach here, something went wrong, re-show form
            return View(patientUser);
        }

        [HttpGet]
        //[Authorize(Roles = UserRoles.Admin)]

        public IActionResult CreateAdmin()
        {
            return View();
        }
        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(RegisterViewModel patientUser)
        {
            if (ModelState.IsValid)
            {

                if (await _PatientUserRepository.CheckUserbyEmail(patientUser.EmailAddress))
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(patientUser);
                }
                var user = new PatientUser
                {
                    UserName = patientUser.Username,
                    Email = patientUser.EmailAddress,
                    Gender = patientUser.Gender,
                    EmailConfirmed = true  // or set based on your application logic
                };

                var result = await _userManager.CreateAsync(user, patientUser.Password);

                // Optionally add user to a role
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);  // Assign a default role or based on model
                }

                return RedirectToAction(nameof(ListAdmin));
            }
            TempData["Error"] = "entered information is not correct";
            // If we reach here, something went wrong, re-show form
            return View(patientUser);
        }
        //[Authorize(Roles = UserRoles.Admin)]
        //GET Edit
        [HttpGet]

        public async Task<IActionResult> Edit(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var patientUser = await _context.PatientUsers.FindAsync(id);
			if (patientUser == null)
			{
				return NotFound();
			}

			// Pass the PatientUser model to the view
			return View(patientUser);
		}
		//POST Edit
        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(PatientUser model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
			{
				// Handle the case where the user isn't found
				return NotFound();
			}
            //getting the user role
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            // Update the user's properties
            user.UserName = model.UserName;
			user.Email = model.Email;
			user.Gender=model.Gender;

            var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{

                //checking the user`s role to redirect to its own list page
                if (isAdmin)
                {
                    return RedirectToAction(nameof(ListAdmin));
                }
                else
                    return RedirectToAction(nameof(ListPatients));
            }
			else
			{
				// Handle errors
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(model);
			}
		}
        //[Authorize(Roles = UserRoles.Admin)]

        // GET: User/Delete
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(string? id)
        {

			if (id == null)
			{
				return NotFound();
			}
			var patientsFromDb = await _context.PatientUsers.FindAsync(id);
			if (patientsFromDb == null)
			{
				return NotFound();
			}

			return View(patientsFromDb);

		}
        //[Authorize(Roles = UserRoles.Admin)]

        [HttpPost]
        public async Task<IActionResult> Delete(PatientUser User)
        {
            var isAdmin = await _userManager.IsInRoleAsync(User, "Admin");
            
            var user1 = await _userManager.FindByIdAsync(User.Id);
            var result = _PatientUserRepository.Delete(user1);
            //checking the user`s role to redirect to its own list page
            if (isAdmin)
            {
                return RedirectToAction(nameof(ListAdmin));
            }
            else
                return RedirectToAction(nameof(ListPatients));
        }
    }
}
