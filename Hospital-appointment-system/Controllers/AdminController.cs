using Hospital_appointment_system.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_appointment_system.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            var Admins= _context.AdminUser.ToList();
            return View(Admins);
        }
    }
}
