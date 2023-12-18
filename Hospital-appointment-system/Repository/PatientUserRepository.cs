using Hospital_appointment_system.Data;
using Hospital_appointment_system.Interfaces;
using Hospital_appointment_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_appointment_system.Repository
{
    public class PatientUserRepository : IPatientUserRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(PatientUser patientUser)
        {
            _context.Add(patientUser);
            return Save();
        }

        public bool Delete(PatientUser patientUser)
        {
            
            _context.Remove(patientUser);
            return Save();
        }

        public async Task<IEnumerable<PatientUser>> GetAll()
        {
            return await _context.PatientUsers.ToListAsync();
        }

        public async Task<IEnumerable<PatientUser>> GetPatientyByEmail(string Email)
        {
            return await _context.PatientUsers.Where(Patient => Patient.Email.Equals(Email)).ToArrayAsync();
        }
        public async Task <bool>CheckUserbyEmail(string patientEmail)
        {
            // Check if the user already exists based on username or email
            return await _context.PatientUsers.AnyAsync(u => u.Email.Equals(patientEmail));
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }
        public async Task<PatientUser?> GetByEmailAsync(string Email)
        {
            return await _context.PatientUsers.FirstOrDefaultAsync(Patient=>Patient.Email==Email);
        }
        public bool Update(PatientUser patientUser)
        {
            throw new NotImplementedException();
        }

    }
}
