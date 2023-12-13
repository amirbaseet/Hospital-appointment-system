using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_appointment_system.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        [ForeignKey("Departments")]
        public int DepartmentID { get; set; }

        // Navigation property for related department
        public virtual Departments Department { get; set; }

        // Navigation property for working hours
        public virtual ICollection<WorkingHour> WorkingHours { get; set; }

        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}

