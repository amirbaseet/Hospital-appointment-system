using Hospital_appointment_system.Data;
using Hospital_appointment_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Hospital_appointment_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
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
            return _context.Doctors.ToList();
        }
        //public async Task<IActionResult> GetDoctors()
        //{
        //    //var doctors = await _context.Doctors.ToListAsync();
        //    //return Ok(doctors);
        //}

        //// POST: api/DoctorApi/Create
        //[HttpPost("Create")]
        //public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (doctor.ClinicID > 0 && !string.IsNullOrEmpty(doctor.Specialization) && !string.IsNullOrEmpty(doctor.Name))
        //    {
        //        await _context.Doctors.AddAsync(doctor);
        //        await _context.SaveChangesAsync();
        //        return CreatedAtAction("GetDoctors", new { id = doctor.DoctorID }, doctor);
        //    }

        //    return BadRequest("Invalid doctor data");
        //}

        //// PUT: api/DoctorApi/Edit/5
        //[HttpPut("Edit/{id}")]
        //public async Task<IActionResult> EditDoctor(int id, [FromBody] Doctor doctor)
        //{
        //    if (id != doctor.DoctorID)
        //    {
        //        return BadRequest();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Entry(doctor).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DoctorExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// DELETE: api/DoctorApi/Delete/5
        //[HttpDelete("Delete/{id}")]
        //public async Task<IActionResult> DeleteDoctor(int id)
        //{
        //    var doctor = await _context.Doctors.FindAsync(id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Doctors.Remove(doctor);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool DoctorExists(int id)
        //{
        //    return _context.Doctors.Any(e => e.DoctorID == id);
        //}
    }
}
