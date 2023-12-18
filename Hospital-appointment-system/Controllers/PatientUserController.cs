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

namespace Hospital_appointment_system.Controllers
{
    public class PatientUserController : Controller
    {
        private readonly IPatientUserRepository _PatientUserRepository;
        private readonly ApplicationDbContext _context;
		private readonly UserManager<PatientUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		//private readonly Seeds _seeds;

		//public PatientUserController( IPatientUserRepository patientUserRepository, ApplicationDbContext context, Seeds seeds)
		public PatientUserController( IPatientUserRepository patientUserRepository, ApplicationDbContext context
            , UserManager<PatientUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _PatientUserRepository = patientUserRepository;
            _context = context;
			_userManager = userManager;
			_roleManager = roleManager;
            //_seeds = seeds;
        }
        public async Task <IActionResult> Index()
        {
            //var patients = _context.PatientUsers.ToList();
            //return View(patients);
            IEnumerable<PatientUser> patients =await _PatientUserRepository.GetAll();
            return View(patients);
        }
        // GET: User/Create
        [HttpGet]
		public IActionResult Create()
		{
			return View();
        }
		//var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<PatientUser>>();
		[HttpPost]

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
				//await _seeds.RegisterPatientUserAsync(patientUser);
				//_PatientUserRepository.Add(patientUser);
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
			// Optionally handle email confirmation and change tokens
			// var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
			// var result = await _userManager.ChangeEmailAsync(user, model.Email, token);

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

			//         if (patientsFromDb == null) 
			//         {
			//             // Add an error to the ModelState
			//             ModelState.AddModelError(string.Empty, "User Doesn`t exists with the given email.");
			//             return View(patientsFromDb);
			//         }

			//// Use UserManager to delete the user
			//var result =  _PatientUserRepository.Delete(patientsFromDb);
			//var result = await _userManager.DeleteAsync(patientsFromDb);
			//if (result.Succeeded)
			//{
			//	// Optionally, you can also remove related data from other tables
			//	// _context.SomeOtherTable.RemoveRange(_context.SomeOtherTable.Where(x => x.UserId == userId));
			//	// await _context.SaveChangesAsync();

			//	return RedirectToAction("Index"); // Redirect to the user list page
			//}
			//else
			//{
			//	// Handle errors
			//	TempData["Error"] = "entered information is not correct";
			//	return View("Error"); // Or return to a suitable error view
			//}
			return RedirectToAction("Index"); // Redirect to the user list page

		}
	}
}