using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_appointment_system.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        [ForeignKey("PatientUser")]
        public int PatientUserID { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; } // Enum can be used for predefined statuses

        // Navigation properties for related user and doctor
        public virtual PatientUser User { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
