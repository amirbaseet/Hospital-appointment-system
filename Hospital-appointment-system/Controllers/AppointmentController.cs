using Hospital_appointment_system.Data;
using Hospital_appointment_system.Data.Enum;
using Hospital_appointment_system.Models;
using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Hospital_appointment_system.ViewModels.MyAppointmentsViewModel;

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
        public async Task<IActionResult> MyAppointments()
        {
            var myAppointmentsViewModelList = new List<MyAppointmentsViewModel>();
            

            var user = await _userManeger.GetUserAsync(User);
            var AppointmentListesi = (from d in _context.Appointments where d.PatientUserID == user.Id select d).ToList();
            foreach (var appointment in AppointmentListesi)
            {
                var myAppointmentsViewModel = new MyAppointmentsViewModel();
                myAppointmentsViewModel.AppointmentID = appointment.AppointmentID;
                var clinic = await _context.Clinic.FindAsync(appointment.ClinicID);
                if (clinic != null)
                {
                    myAppointmentsViewModel.ClinicName = clinic.Name;
                }
                var doctor = await _context.Doctors.FindAsync(appointment.DoctorID);
                if (clinic != null)
                {
                    myAppointmentsViewModel.DoctorName = doctor.Name;
                }
                var availableAppointments = await _context.AppointmentStatus.FindAsync(appointment.AvailableAppointmentsID);
                if (availableAppointments != null)
                {
                    myAppointmentsViewModel.DayOfWeek = availableAppointments.DayOfWeek;
                    myAppointmentsViewModel.Time = availableAppointments.Time;
                }
                myAppointmentsViewModelList.Add(myAppointmentsViewModel);
            }
            return View(myAppointmentsViewModelList);  
        }
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminAppointments()
        {
            var adminAppointmentsViewModelList = new List<AdminAppointmentsViewModel>();
            var AppointmentListesi = await _context.Appointments.ToListAsync();
            foreach (var appointment in AppointmentListesi)
            {
                var AllAppointmentsViewModel = new AdminAppointmentsViewModel();
                AllAppointmentsViewModel.AppointmentID = appointment.AppointmentID;
                var patient = await _context.PatientUsers.FindAsync(appointment.PatientUserID);
                if (patient != null)
                {
                    AllAppointmentsViewModel.PatientName = patient.Name;
                    AllAppointmentsViewModel.PatientEmail = patient.Email;
                }
                var clinic = await _context.Clinic.FindAsync(appointment.ClinicID);
                if (clinic != null)
                {
                    AllAppointmentsViewModel.ClinicName = clinic.Name;
                }
                var doctor = await _context.Doctors.FindAsync(appointment.DoctorID);
                if (clinic != null)
                {
                    AllAppointmentsViewModel.DoctorName = doctor.Name;
                }
                var availableAppointments = await _context.AppointmentStatus.FindAsync(appointment.AvailableAppointmentsID);
                if (availableAppointments != null)
                {
                    AllAppointmentsViewModel.DayOfWeek = availableAppointments.DayOfWeek;
                    AllAppointmentsViewModel.Time = availableAppointments.Time;
                }
                adminAppointmentsViewModelList.Add(AllAppointmentsViewModel);
            }
            return View(adminAppointmentsViewModelList);
        }

        public async Task<IActionResult> MakeAnAppointment()
        {
			AvailableAppointmentsViewModel viewModel = new AvailableAppointmentsViewModel
			{
				clinics = await _context.Clinic.ToListAsync()
			};
			return View(viewModel);
        }
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminMakeAnAppointment()
        {
            var users = await _userManeger.Users.ToListAsync();
            var patients = new List<PatientUser>();

            foreach (var user in users)
            {
                // Check if the user is not an admin
                if (!await _userManeger.IsInRoleAsync(user, UserRoles.Admin))
                {
                    // User is not an admin, so we consider them as a patient
                    patients.Add(user);
                }
            }
            AdminAvailableAppointmentsViewModel viewModel = new AdminAvailableAppointmentsViewModel
            {

                patientUsers = patients,
                clinics = await _context.Clinic.ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(MakeAnAppointmentViewModel makeAnAppointmentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (makeAnAppointmentViewModel.AvailableAppointmentsID != null)
                {
                    Appointment appointment = new Appointment
                    {
                        DoctorID = makeAnAppointmentViewModel.DoctorID,
                        ClinicID = makeAnAppointmentViewModel.ClinicID,
                        AvailableAppointmentsID = makeAnAppointmentViewModel.AvailableAppointmentsID,
                    };
                    var user = await _userManeger.GetUserAsync(User);
                    appointment.PatientUserID = await _userManeger.GetUserIdAsync(user);

                    _context.Appointments.Add(appointment);
                    var apt = await _context.AppointmentStatus.FindAsync(appointment.AvailableAppointmentsID);
                    if(apt != null) {
                        apt.AppointmentStatus = AppointmentStatus.notActive;
                    }
                    _context.Update(apt);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();  
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminCreateAppointment(AdminMakeAnAppointmentViewModel adminMakeAnAppointmentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (adminMakeAnAppointmentViewModel.AvailableAppointmentsID != null)
                {
                    Appointment appointment = new Appointment
                    {
                        PatientUserID = adminMakeAnAppointmentViewModel.Id,
                        DoctorID = adminMakeAnAppointmentViewModel.DoctorID,
                        ClinicID = adminMakeAnAppointmentViewModel.ClinicID,
                        AvailableAppointmentsID = adminMakeAnAppointmentViewModel.AvailableAppointmentsID,
                    };
                    _context.Appointments.Add(appointment);
                    var apt = await _context.AppointmentStatus.FindAsync(appointment.AvailableAppointmentsID);
                    if (apt != null)
                    {
                        apt.AppointmentStatus = AppointmentStatus.notActive;
                    }
                    _context.Update(apt);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AdminAppointments", "Appointment");
                }
            }
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                var availableAppointments = await _context.AppointmentStatus.FindAsync(appointment.AvailableAppointmentsID);
                if (availableAppointments != null)
                {
                    availableAppointments.AppointmentStatus = AppointmentStatus.active;
                    _context.Update(availableAppointments);
                }
                _context.Remove(appointment);
                _context.SaveChanges();

            }
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("AdminAppointments");
            }
            else
                return RedirectToAction("MyAppointments");
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
        public async Task<JsonResult> AvailableAppointmentsjsonData(int? HoursID)
        {
            if (HoursID.HasValue)
            {
				var workingHour = await _context.WorkingHours.FindAsync(HoursID);
                var AvailableAppointmentsListesi = (from d in _context.AppointmentStatus where d.DayOfWeek == workingHour.DayOfWeek select d).ToList();
                return Json(AvailableAppointmentsListesi);
            }
            return Json(null);
        }
    }
}
