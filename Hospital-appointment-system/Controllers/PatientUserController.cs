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
using Microsoft.Extensions.Localization;

namespace Hospital_appointment_system.Controllers
{
    public class PatientUserController : Controller
    {
        //private readonly IPatientUserRepository _PatientUserRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PatientUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<PatientUserController> _localizer;

        public PatientUserController(/*IPatientUserRepository patientUserRepository,*/ ApplicationDbContext context, UserManager<PatientUser> userManager, RoleManager<IdentityRole> roleManager, IStringLocalizer<PatientUserController> localizer)
        {
            //_PatientUserRepository = patientUserRepository;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer; // Assign the passed localizer to the _localizer field
        }

        [Authorize(Roles = UserRoles.Admin)]
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

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ListPatients()
        {
            ViewData["PatientsList"] = _localizer["PatientsList"];
            ViewData["AddaPatient"] = _localizer["AddaPatient"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Delete"] = _localizer["Delete"];
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
            ViewData["AdminsList"] = _localizer["AdminsList"];
            ViewData["AddanAdmin"] = _localizer["AddanAdmin"];
            ViewData["Edit"] = _localizer["Edit"];
            ViewData["Delete"] = _localizer["Delete"];
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
        [Authorize(Roles = UserRoles.Admin)]
        public IActionResult Create()
        {
            @ViewData["CreatePatient"] = _localizer["CreatePatient"];
            @ViewData["Create"] = _localizer["Create"];
            @ViewData["BacktoList"] = _localizer["BacktoList"];
            return View();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create(RegisterViewModel patientUser)
        {
            if (ModelState.IsValid)
            {
                if (await _context.PatientUsers.AnyAsync(u => u.Email.Equals(patientUser.EmailAddress)))
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(patientUser);
                }
                var user = new PatientUser
                {
                    Name = patientUser.Username,
                    UserName = patientUser.EmailAddress,
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
       
        //GET Edit
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Edit(string? id)
        {
            @ViewData["EditPatient"] = _localizer["EditPatient"];
            @ViewData["Update"] = _localizer["Update"];
            @ViewData["BacktoList"] = _localizer["BacktoList"];
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
        [Authorize(Roles = UserRoles.Admin)]
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
            user.Name = model.Name; // Corrected: Assigning model.Name to user.Name
            user.UserName = model.Email; // Assuming you want to keep UserName and Email same
            user.Email = model.Email;
            user.Gender = model.Gender;

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

        // GET: User/Delete
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
			@ViewData["Delete"] = _localizer["Delete"];
			@ViewData["BacktoList"] = _localizer["BacktoList"];
            @ViewData["DeletePatient"] = _localizer["DeletePatient"];

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

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(PatientUser User)
        {

			var isAdmin = await _userManager.IsInRoleAsync(User, "Admin");

            var user1 = await _userManager.FindByIdAsync(User.Id);
            _context.Remove(user1);
            var result = _context.SaveChanges();
            //checking the user`s role to redirect to its own list page
            if (isAdmin)
            {
                return RedirectToAction(nameof(ListAdmin));
            }
            else
                return RedirectToAction(nameof(ListPatients));
        }
        ///////////////////////////////////////////////////////////////
        ///Admin part
        //GET Edit

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> EditAdmin(string? id)
        {
            @ViewData["EditPatient"] = _localizer["EditPatient"];
            @ViewData["Update"] = _localizer["Update"];
            @ViewData["BacktoList"] = _localizer["BacktoList"];
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
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> EditAdmin(PatientUser model)
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

            // Update the user's properties
            user.Name = model.Name; // Corrected: Assigning model.Name to user.Name
            user.UserName = model.Email; // Assuming you want to keep UserName and Email same
            user.Email = model.Email;
            user.Gender = model.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Checking the user's role to redirect to the appropriate list page
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin)
                {
                    return RedirectToAction(nameof(ListAdmin));
                }
                else
                {
                    return RedirectToAction(nameof(ListPatients));
                }
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
        [Authorize(Roles = UserRoles.Admin)]

        // GET: User/Delete
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteAdmin(string? id)
        {
            @ViewData["BacktoList"] = _localizer["BacktoList"];
            @ViewData["DeletePatient"] = _localizer["DeletePatient"];
            @ViewData["Delete"] = _localizer["Delete"];

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
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteAdmin(PatientUser User)
        {

            var isAdmin = await _userManager.IsInRoleAsync(User, "Admin");

            var user1 = await _userManager.FindByIdAsync(User.Id);
            _context.Remove(user1);
            var result = _context.SaveChanges();
            //checking the user`s role to redirect to its own list page
            if (isAdmin)
            {
                return RedirectToAction(nameof(ListAdmin));
            }
            else
                return RedirectToAction(nameof(ListPatients));
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]

        public IActionResult CreateAdmin()
        {
            @ViewData["Create"] = _localizer["Create"];
            @ViewData["BacktoList"] = _localizer["BacktoList"];
            return View();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(RegisterViewModel patientUser)
        {

            if (ModelState.IsValid)
            {

                if (await _context.PatientUsers.AnyAsync(u => u.Email.Equals(patientUser.EmailAddress)))
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(patientUser);
                }
                var user = new PatientUser
                {
                    Name = patientUser.Username,
                    Email = patientUser.EmailAddress,
                    UserName = patientUser.EmailAddress,
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
    }
}

