using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Hospital_appointment_system.Models
{
    public class Departments
    {
        [Key]
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property for related clinics
        public virtual ICollection<Clinic> Clinics { get; set; }

        // Navigation property for related doctors
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
