using Hospital_appointment_system.Data;
using Hospital_appointment_system.Interfaces;
using Hospital_appointment_system.Models;
using Hospital_appointment_system.Repository;
using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace Hospital_appointment_system.Controllers
{
    public class PatientUserController : Controller
    {
        private readonly IPatientUserRepository _PatientUserRepository;
        private readonly ApplicationDbContext _context;
        private readonly Seeds _seeds;

        public PatientUserController( IPatientUserRepository patientUserRepository, ApplicationDbContext context, Seeds seeds)
        {
            _PatientUserRepository = patientUserRepository;
            _context = context;
            _seeds = seeds;
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
                    // Add an error to the ModelState
                    ModelState.AddModelError(string.Empty, "User already exists with the given email.");
                    return View(patientUser);
                }
                await _seeds.RegisterPatientUserAsync(patientUser);
                //_PatientUserRepository.Add(patientUser);
                return RedirectToAction(nameof(Index));
            }
            // If we reach here, something went wrong, re-show form
            return View(patientUser);
        }
		//GET Edit
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || id == 0)
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
		//POST Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(RegisterViewModel obj)
		{
			//if (obj.UserID > 0 && obj.Username != string.Empty && obj.Email!= string.Empty && obj.Password != string.Empty)
			if (obj.Username != string.Empty && obj.Emi!= string.Empty && obj.Password != string.Empty)
			{
				_context.PatientUsers.Update(obj);
				_context.SaveChanges();
			}

			return RedirectToAction("Index");
		}
		// GET: User/Delete
		[HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
			if (id == null || id == 0)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(PatientUser patientsFromDb)
        {
            var patient = await _context.PatientUsers.FindAsync(patientsFromDb.UserID);
            if (patient == null) 
            {
                // Add an error to the ModelState
                ModelState.AddModelError(string.Empty, "User Doesn`t exists with the given email.");
                return View(patient);
            }
            _PatientUserRepository.Delete(patient);
            return RedirectToAction("Index");
        }
    }

}
