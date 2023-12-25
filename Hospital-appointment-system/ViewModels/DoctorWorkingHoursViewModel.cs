using Hospital_appointment_system.Data.Enum;
using Hospital_appointment_system.Models;

namespace Hospital_appointment_system.ViewModels
{
    public class DoctorWorkingHoursViewModel
    {
         public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public List<WorkingHourDetail> WorkingHours { get; set; }

        public class WorkingHourDetail
        {
            public string DayOfWeek { get; set; } // Using enum here
            public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        }
    }
}
