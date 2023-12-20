using Hospital_appointment_system.ViewModels;
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
        [ForeignKey("Clinic")]
        public int ClinicID { get; set; }

        // Navigation property for related department
        public virtual Clinic Clinic { get; set; } = null;

        // Navigation property for working hours
        public virtual ICollection<WorkingHour> WorkingHours { get; set; } = null;

        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = null;
    }
}

