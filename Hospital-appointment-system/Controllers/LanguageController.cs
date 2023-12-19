using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_appointment_system.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Change(string lang, string returnUrl = "/")
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
