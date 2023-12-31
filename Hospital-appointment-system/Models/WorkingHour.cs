﻿using Hospital_appointment_system.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace Hospital_appointment_system.Models
{
    public class WorkingHour
    {
        [Key]
        public int HoursID { get; set; }
        public int DoctorID { get; set; }
        public string DayOfWeek { get; set; } // Using enum here
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Navigation property for related doctor
        public virtual Doctor Doctor { get; set; } = null;
    }
}
