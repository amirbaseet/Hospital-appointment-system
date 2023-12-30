using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_appointment_system.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicID { get; set; }
        [ForeignKey("PatientUser")]
        public string PatientUserID { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }

        [ForeignKey("AvailableAppointments")]
        public int AvailableAppointmentsID { get; set; }

    }
}
