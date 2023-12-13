using System.ComponentModel.DataAnnotations;

namespace Hospital_appointment_system.Models
{
    public class AdminUser
    {
        [Key]
        public int AdminID { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
