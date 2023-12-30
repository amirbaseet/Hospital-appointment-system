using Hospital_appointment_system.Data.Enum;

namespace Hospital_appointment_system.ViewModels
{
    public class WorkingHourViewModel
    {
        public int DoctorID { get; set; }
        public Days DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
