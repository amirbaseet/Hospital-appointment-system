using Hospital_appointment_system.Data;
using Hospital_appointment_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Hospital_appointment_system.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Index()
        {
            var Doctors = _context.Doctors.ToList();
            return View(Doctors);
        }
        [Authorize(Roles = UserRoles.Admin)]
        //GET
        public async Task<IActionResult> Create()
        {

            var Create = new DoctorViewModel();
            Create.clinicList= await _context.Clinic.ToListAsync();
            return View(Create);
        }

        [Authorize(Roles = UserRoles.Admin)]
        //POST
        [HttpPost]
	
        [ValidateAntiForgeryToken]
		public IActionResult Create(DoctorViewModel obj)
		{
            var doctor = obj.doctor;
            if(doctor.ClinicID > 0&& doctor.Specialization != string.Empty&& doctor.Name != string.Empty)
            {
				_context.Doctors.Add(obj.doctor);
				_context.SaveChanges();
			}
            
			return RedirectToAction("Index");
		}

		//GET Edit
		public async Task<IActionResult> Edit(int? id)
		{
			if(id == null || id == 0)

			{
				return NotFound();
			}
			var DoctorFromDb = await _context.Doctors.FindAsync(id);
			if (DoctorFromDb == null)
			{
				return NotFound();
			}
            var edit = new DoctorViewModel();
			edit.doctor = DoctorFromDb;
            edit.clinicList = await _context.Clinic.ToListAsync();
            return View(edit);
		}
		//POST Edit
		[HttpPost]
		[ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(DoctorViewModel obj)
        {
            var doctor = obj.doctor;

	
			if (doctor.ClinicID > 0 && doctor.Specialization != string.Empty && doctor.Name != string.Empty)
			{
				_context.Doctors.Update(obj.doctor);
				_context.SaveChanges();
			}

			return RedirectToAction("Index");
		}

        //GET //GET Delete
        public async Task<IActionResult> Delete(int? id)


        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var DoctorFromDb = await _context.Doctors.FindAsync(id);
            if (DoctorFromDb == null)
            {
                return NotFound();
            }
            var edit = new DoctorViewModel();
            edit.doctor = DoctorFromDb;
            edit.clinicList = await _context.Clinic.ToListAsync();
            return View(edit);
        }
		//POST Delete
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(DoctorViewModel id)
        {
            var obj = await _context.Doctors.FindAsync(id.doctor.DoctorID);
            if (obj == null)
            {
                return NotFound();
            }
            _context.Doctors.Remove(obj);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}
