using Hospital_appointment_system.Data;
using Hospital_appointment_system.Models;
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
        public IActionResult Index() {

            var Doctors = _context.Doctors.ToList();
            return View(Doctors);
        }

        //GET
        public IActionResult Create() {
            var Create = new DoctorViewModel();
            Create.clinicList=_context.Clinic.ToList();
            return View(Create);
        }
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
		//GET
		public IActionResult Edit(int? id)
		{
			if(id == null || id == 0)
			{
				return NotFound();
			}
			var DoctorFromDb = _context.Doctors.Find(id);
			if (DoctorFromDb == null)
			{
				return NotFound();
			}
            var edit = new DoctorViewModel();
			edit.doctor = DoctorFromDb;
            edit.clinicList = _context.Clinic.ToList();
            return View(edit);
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(DoctorViewModel obj)
		{
			var doctor = obj.doctor;
			if (doctor.ClinicID > 0 && doctor.Specialization != string.Empty && doctor.Name != string.Empty)
			{
				_context.Doctors.Update(obj.doctor);
				_context.SaveChanges();
			}

			return RedirectToAction("Index");
		}

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var DoctorFromDb = _context.Doctors.Find(id);
            if (DoctorFromDb == null)
            {
                return NotFound();
            }
            var edit = new DoctorViewModel();
            edit.doctor = DoctorFromDb;
            edit.clinicList = _context.Clinic.ToList();
            return View(edit);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(DoctorViewModel id)
        {
            var obj = _context.Doctors.Find(id.doctor.DoctorID);
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
