using Hospital_appointment_system.Models;

namespace Hospital_appointment_system.ViewModels
{
    public class AvailableAppointmentsViewModel
    {
		public List<Clinic> clinics { get; set; }
		public Clinic clinic { get; set; }
		public int SelectedClinicId { get; set; }

		public List<Doctor> doctors { get; set; }
		public Doctor doctor { get; set; }
        public WorkingHour workingHour { get; set; }
        public AvailableAppointments availableAppointments { get; set; }

    }
}
