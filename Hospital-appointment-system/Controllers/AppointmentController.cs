using Hospital_appointment_system.Data;
using Hospital_appointment_system.Models;
using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_appointment_system.Controllers
{
    public class AppointmentController:Controller
    {
        private readonly UserManager<PatientUser> _userManeger;
        private readonly ApplicationDbContext _context;

        public AppointmentController(UserManager<PatientUser> userManeger,
            ApplicationDbContext context)
        {
            _userManeger = userManeger;
            _context= context;
        }
        public async Task<IActionResult> MakeAnAppointment()
        {
			AvailableAppointmentsViewModel viewModel = new AvailableAppointmentsViewModel
			{
				clinics = await _context.Clinic.ToListAsync()
			};
			return View(viewModel);
        }
		[HttpGet]
		public JsonResult DoctorjsonData(int? ClinicID)
		{
			if (ClinicID.HasValue)
			{
				var DoktorListesi = (from d in _context.Doctors where d.ClinicID == ClinicID select d).ToList();
				return Json(DoktorListesi);
			}
			return Json(null);
		}
		[HttpGet]
		public JsonResult WorkingHourjsonData(int? DoctorID)
		{
			if (DoctorID.HasValue)
			{
				var WorkingHourListesi = (from d in _context.WorkingHours where d.DoctorID == DoctorID select d).ToList();
				return Json(WorkingHourListesi);
			}
			return Json(null);
		}
	}
}
