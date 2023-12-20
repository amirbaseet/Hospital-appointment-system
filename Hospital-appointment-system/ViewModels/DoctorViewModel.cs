using Hospital_appointment_system.Models;
using Hospital_appointment_system.ViewModels;

public class DoctorViewModel
{
    public Doctor doctor { get; set; }
    public List<Clinic> clinicList { get; set; } = null;
    public List<WorkingHourViewModel> WorkingHours { get; set; } = new List<WorkingHourViewModel>();
}

