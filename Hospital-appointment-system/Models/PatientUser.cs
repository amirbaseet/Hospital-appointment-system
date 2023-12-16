using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hospital_appointment_system.Models
{
    public class PatientUser:IdentityUser
    {
        //[Key]
        //public int UserID { get; set; }

        //public int TCID { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; } // Consider encryption or hashing
        //public string Email { get; set; }
        //public string Role { get; set; }
        // Navigation property for appointments
        //public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
