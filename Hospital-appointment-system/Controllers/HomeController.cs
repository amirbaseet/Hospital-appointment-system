using Hospital_appointment_system.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;

namespace Hospital_appointment_system.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IStringLocalizer<HomeController> _localizer;

		public HomeController(ILogger<HomeController> logger
			, IStringLocalizer<HomeController> localizer)
		{
			_logger = logger;
			_localizer = localizer;
		}
		
	
		public IActionResult Index()
		{
			// Use _localizer to get localized strings
			ViewData["Greeting"] = _localizer["WelcomeMessage"];
			ViewData["Home2"] = _localizer["Home2"];
			// Add more localized strings as needed
			return View();
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}




