    using Hospital_appointment_system.Data.Enum;
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    namespace Hospital_appointment_system.Models
    {
        public class PatientUser : IdentityUser
        {
            public GenderCategory Gender { get; set; } // Consider encryption or hashing
        
            public  string Name{ get; set; } // Consider encryption or hashing
        }
    }
