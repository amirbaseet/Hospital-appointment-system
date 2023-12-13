using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_appointment_system.Models
{
    public class PatientUser
    {
        [Key]
        public int UserID { get; set; }
        
        //public int TCID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; } // Consider encryption or hashing
        public string Email { get; set; }
        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
