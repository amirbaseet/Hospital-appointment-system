using Hospital_appointment_system.Data;
using Hospital_appointment_system.Models;
using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hospital_appointment_system.Controllers
{
	public class DoctorController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly IStringLocalizer<DoctorController> _localizer;

		public DoctorController(ApplicationDbContext context, IStringLocalizer<DoctorController> localizer)
		{
			_context = context;
			_localizer = localizer;

		}

		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> Index()
		{
            ViewData["Doctor`sList"]= _localizer["Doctor`sList"];
            ViewData["ListDoctorsWithWorkingHours"]=_localizer["ListDoctorsWithWorkingHours"];
			ViewData["CreateNewDoctor"] =_localizer["CreateNewDoctor"];
			ViewData["Edit"]			 =_localizer["Edit"];
			ViewData["Delete"]			 =_localizer["Delete"];
			ViewData["AddWorkingHours"]	 = _localizer["AddWorkingHours"];
            List<Doctor> doctors = new List<Doctor>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7188/api/DoctorApi");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            doctors = JsonConvert.DeserializeObject<List<Doctor>>(jsonResponse);

            return View(doctors);
        }
		[Authorize(Roles = UserRoles.Admin)]
		//GET
		public async Task<IActionResult> Create()
		{
			ViewData["AddDoctor"] = _localizer["AddDoctor"];
			@ViewData["Create"] =    _localizer["Create"];
			@ViewData["BacktoList"] = _localizer["BacktoList"];
			var Create = new DoctorViewModel();
			Create.clinicList = await _context.Clinic.ToListAsync();
			return View(Create);
		}

		[Authorize(Roles = UserRoles.Admin)]
		//POST
		[HttpPost]

		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAsync(DoctorViewModel obj)
		{
            var doctor = obj.doctor;
            if (doctor.ClinicID > 0 && doctor.Specialization != string.Empty && doctor.Name != string.Empty)
            {
                HttpClient client = new HttpClient();
                var json = JsonConvert.SerializeObject(doctor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7188/api/DoctorApi", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
		}
		[Authorize(Roles = UserRoles.Admin)]

		[HttpGet]
        public async Task<IActionResult> CreateWorkingHours(int doctorId) // Make sure you pass the doctorId
        {
			@ViewData["Addinghours"] = _localizer["Addinghours"];
			@ViewData["StartTime"]= _localizer["StartTime"];
			@ViewData["EndTime"]=_localizer["EndTime"];
			@ViewData["Add"] = _localizer["Add"];
			var viewModel = new WorkingHourViewModel
            {
                DoctorID = doctorId // Set the doctorId in the ViewModel
            };
            return View(viewModel);
        }
		[Authorize(Roles = UserRoles.Admin)]

		[HttpPost]
		public async Task<IActionResult> CreateWorkingHoursAsync(WorkingHourViewModel obj)
        {
			if (ModelState.IsValid)
			{
				WorkingHour workingHour = new WorkingHour
				{
					DoctorID = obj.DoctorID,
					StartTime = obj.StartTime,
					EndTime = obj.EndTime,
					DayOfWeek = obj.DayOfWeek.ToString()
            };
				if (obj.DayOfWeek != null)
				{
					_context.WorkingHours.Add(workingHour);
					 _context.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(obj);
        }
		[Authorize(Roles = UserRoles.Admin)]

		public async Task<IActionResult> DoctorsWithWorkingHours()
        {
            @ViewData["ListDoctors"]= _localizer["ListDoctors"];
            @ViewData["Doctor`sWorkingHours"] = _localizer["Doctor`sWorkingHours"];

            var doctorsWithHours = _context.Doctors.Select(d => new DoctorWorkingHoursViewModel
            {
                DoctorID = d.DoctorID,
                Name = d.Name,
                Specialization = d.Specialization,
                WorkingHours = _context.WorkingHours
                    .Where(w => w.DoctorID == d.DoctorID)
                    .Select(w => new DoctorWorkingHoursViewModel.WorkingHourDetail
                    {
                        DayOfWeek = w.DayOfWeek,
                        StartTime = w.StartTime,
                        EndTime = w.EndTime
                    })
                    .ToList()
            }).ToList();

            return View(doctorsWithHours);
        }
		[Authorize(Roles = UserRoles.Admin)]

		//GET Edit
		public async Task<IActionResult> Edit(int? id)
		{
			ViewData["EditDoctor"] = _localizer["EditDoctor"];
			ViewData["Update"] = _localizer["Update"];
			ViewData["BacktoList"] = _localizer["BacktoList"];


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
		[Authorize(Roles = UserRoles.Admin)]
		//POST Edit
		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Edit(DoctorViewModel obj)
		{
			if (ModelState.IsValid)
			{
				var doctor = obj.doctor;
				if (doctor.ClinicID > 0 && doctor.Specialization != string.Empty && doctor.Name != string.Empty)
				{
					HttpClient client = new HttpClient();
					var jason = JsonConvert.SerializeObject(doctor);
					var content = new StringContent(jason, Encoding.UTF8, "application/json");
					var response = await client.PutAsync($"https://localhost:7188/Api/DoctorApi/{doctor.DoctorID}", content);
					if (response.IsSuccessStatusCode)
					{
						return RedirectToAction("Index");
					}
				}
			}
            // Handle failure case
            // Possibly reload the form with error messages or log the error
            return View(obj);
        }
		[Authorize(Roles = UserRoles.Admin)]

		//GET //GET Delete
		public async Task<IActionResult> Delete(int? id)


		{
			@ViewData["DeleteDoctor"]= _localizer["DeleteDoctor"];
			@ViewData["Delete"]= _localizer["Delete"];
			@ViewData["BacktoList"] = _localizer["BacktoList"];

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
		[Authorize(Roles = UserRoles.Admin)]
		//POST Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeletePOST(DoctorViewModel id)
		{
			//	var obj = await _context.Doctors.FindAsync(id.doctor.DoctorID);
			//	if (obj == null)
			//	{
			//		return NotFound();
			//	}
			//	_context.Doctors.Remove(obj);
			//	_context.SaveChanges();
			var doctorId = id.doctor.DoctorID;
			  HttpClient client = new HttpClient();
			var response = await client.DeleteAsync($"https://localhost:7188/api/DoctorApi/{doctorId}");
			if (response.IsSuccessStatusCode)
			{
                return RedirectToAction("Index");
            }


            // Handle failure case
            // You might want to reload the form with error messages or log the error
            return View(id);
        }
	}
}