using Hospital_appointment_system.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_appointment_system.Models
{
    public class AvailableAppointments
    {
        [Key]
        public int AvailableAppointmentsID { get; set; }
        public int DoctorID { get; set; }
        public string DayOfWeek { get; set; } // Using enum here
        public TimeSpan Time { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }
        // Navigation property for related doctor
        public virtual Doctor Doctor { get; set; } = null;
    }
}
