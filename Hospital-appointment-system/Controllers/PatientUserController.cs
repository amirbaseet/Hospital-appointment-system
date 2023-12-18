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

namespace Hospital_appointment_system.Controllers
{
    public class PatientUserController : Controller
    {
        private readonly IPatientUserRepository _PatientUserRepository;
        private readonly ApplicationDbContext _context;
		private readonly UserManager<PatientUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		//private readonly Seeds _seeds;

		public PatientUserController( IPatientUserRepository patientUserRepository, ApplicationDbContext context
            , UserManager<PatientUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _PatientUserRepository = patientUserRepository;
            _context = context;
			_userManager = userManager;
			_roleManager = roleManager;
            //_seeds = seeds;
        }
    [Authorize(Roles = UserRoles.Admin)]

        public async Task <IActionResult> Index()
        {
   
            IEnumerable<PatientUser> patients =await _PatientUserRepository.GetAll();
            return View(patients);
        }
        // GET: User/Create
        [HttpGet]
		public IActionResult Create()
		{
			return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
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
		
				return RedirectToAction(nameof(Index));
            }
			TempData["Error"] = "entered information is not correct";
			// If we reach here, something went wrong, re-show form
			return View(patientUser);
        }

        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }
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

                return RedirectToAction(nameof(Index));
            }
			TempData["Error"] = "entered information is not correct";
			// If we reach here, something went wrong, re-show form
			return View(patientUser);
        }
		//GET Edit
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

			// Update the user's properties
			user.UserName = model.UserName;
			user.Email = model.Email;
			user.Gender=model.Gender;
			

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return RedirectToAction("Index");
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
		[HttpPost]
        public async Task<IActionResult> Delete(PatientUser User)
        {
			var user1 = await _userManager.FindByIdAsync(User.Id);
			var result = _PatientUserRepository.Delete(user1);

			return RedirectToAction("Index"); // Redirect to the user list page
		}
	}
}