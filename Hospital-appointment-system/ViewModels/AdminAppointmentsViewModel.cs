using Hospital_appointment_system.Models;

namespace Hospital_appointment_system.ViewModels
{
    public class AdminAppointmentsViewModel
    {

        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public int AppointmentID { get; set; }
        public string ClinicName { get; set; }
        public string DoctorName { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan Time { get; set; }
        public int AvailableAppointmentsID { get; set; }
    }
}
