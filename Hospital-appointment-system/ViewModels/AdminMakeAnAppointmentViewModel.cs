using Hospital_appointment_system.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_appointment_system.ViewModels
{
    public class AdminMakeAnAppointmentViewModel
    {

        [ForeignKey("PatientUser")]
        public string Id { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicID { get; set; }
        [ForeignKey("PatientUser")]
        public int DoctorID { get; set; }

        [ForeignKey("WorkingHour")]
        public int HoursID { get; set; }

        [ForeignKey("AvailableAppointments")]
        public int AvailableAppointmentsID { get; set; }
    }
}
