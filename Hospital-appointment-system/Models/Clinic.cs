﻿using System.ComponentModel;

namespace Hospital_appointment_system.Models
{
    public class Clinic
    {

		[DisplayName("Clinic")]
		public int ClinicID { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        // Navigation property for related department
        public virtual Departments Department { get; set; }

        // Navigation property for related doctors
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
