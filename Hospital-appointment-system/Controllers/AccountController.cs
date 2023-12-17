using Hospital_appointment_system.Data;
using Hospital_appointment_system.Interfaces;
using Hospital_appointment_system.Models;
using Hospital_appointment_system.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_appointment_system.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<PatientUser> _userManeger;
        private readonly SignInManager<PatientUser> _signInManeger;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<PatientUser> userManeger,
            SignInManager<PatientUser> signInManeger,
            ApplicationDbContext context)
        {
            _userManeger = userManeger;
            _signInManeger = signInManeger;
            _context= context;
        }

        //private readonly UserManager<AppUser>userManager;
        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) 
        {
            if(!ModelState.IsValid ) 
            {
                return View(loginViewModel);
            }
            var user = await _userManeger.FindByEmailAsync(loginViewModel.Email);
            if (user != null) 
            {
                var passCheck = await _userManeger.CheckPasswordAsync(user, loginViewModel.Password);
                if(passCheck)
                {
                    var result = await _signInManeger.PasswordSignInAsync(user, loginViewModel.Password, false, false);
        
                    if(result.Succeeded) 
                    {
                        return RedirectToAction("Privacy", "Home");
                        // Redirect to the Dashboard action of the DashboardController
                        //return RedirectToAction("Index", "Dashboard");
                    }
                }
                TempData["Error"] = "Wrong credentials.Please  Try again ";
                return View(loginViewModel);
            }
            TempData["Error"] = "Wrong credentials.Please  Try again 2";
                return View(loginViewModel);
        }
        [HttpGet]
        public IActionResult Register() 
        {
            var response =new RegisterViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task <IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);
            var user = await _userManeger.FindByEmailAsync(registerViewModel.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }
            var newUser = new PatientUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.Username,
                Gender = registerViewModel.Gender,
                EmailConfirmed = true
            };
            var  newUserResponse= await _userManeger.CreateAsync(newUser, registerViewModel.Password);
            if (newUserResponse.Succeeded)
            {
                await _userManeger.AddToRoleAsync(newUser, UserRoles.User);
                return RedirectToAction("Privacy", "Home");
            }
            foreach (var error in newUserResponse.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerViewModel);
            //return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManeger.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}