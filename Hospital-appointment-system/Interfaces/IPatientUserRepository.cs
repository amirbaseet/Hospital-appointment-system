using Hospital_appointment_system.Models;

namespace Hospital_appointment_system.Interfaces
{
    public interface IPatientUserRepository
    {
        Task<IEnumerable<PatientUser>> GetAll();
        //Task<PatientUser> GetByIdAsync(int id);
        Task<IEnumerable<PatientUser>> GetPatientyByEmail(string Email);
        bool Add(PatientUser patientUser);
        bool Update(PatientUser patientUser);
        bool Delete(PatientUser patientUser);
        bool Save();
        public Task<PatientUser?> GetByEmailAsync(string Email);

        Task<bool> CheckUserbyEmail(string email);
    }
}
