using Hospital_appointment_system.Data;
using Hospital_appointment_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Hospital_appointment_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = UserRoles.Admin)]
    public class DoctorApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DoctorApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DoctorApi
        [HttpGet]
        public List<Doctor> Get()
        {
            var Doctors = _context.Doctors.ToList();
            // normalde json formatına cevirip gondermem lazım  [ApiController] bunu otomatik yapıyor
            return Doctors;
        }
     
        // GET: api/DoctorApi
        [HttpGet("{id}")]
        public Doctor Get(int id) 
        {
            var doctor  = _context.Doctors.FirstOrDefault(x => x.DoctorID == id);
            return doctor;
        }

        [HttpPost]
        public void Post([FromBody]Doctor doctor) 
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }
         //PUT: api/DoctorApi/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDoctor(int id, [FromBody] Doctor doctor)
        {
            var doct=_context.Doctors.FirstOrDefault(x=>x.DoctorID==id);

            if (doct is null)
            {
                return NotFound();
            }
            else 
            {
                doct.Name=doctor.Name;
                doct.ClinicID=doctor.ClinicID;
                doct.Specialization=doctor.Specialization;
                //doct.Appointments = null;
                //doct.WorkingHours = null;
                //doct.Clinic = null;
                //doct.Clinic=doctor.Clinic;
                _context.Update(doct);
                _context.SaveChanges();
                return Ok();
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var doct = _context.Doctors.FirstOrDefault(x => x.DoctorID == id);
            if (doct is null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(doct);
                _context.SaveChanges();
                return Ok();
            }

        }
    }
}
