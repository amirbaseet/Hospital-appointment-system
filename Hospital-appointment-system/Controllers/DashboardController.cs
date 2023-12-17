using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_appointment_system.Controllers
{
    //[Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge(); // Redirect to the login page if the user is not authenticated
            }

            // Pass any necessary data to the dashboard view
            var model = new DashboardViewModel
            {
                // Populate your view model properties here
            };

            return View(model);
        }
    }
}
