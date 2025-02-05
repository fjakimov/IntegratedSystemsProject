using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;
using HoneyShop.Web.Utils;
using Microsoft.AspNetCore.Identity;
using Services.Implementation;

namespace HoneyShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (_userService.Login(email, password))
            {
                var user = _userService.GetUserByEmail(email); // Retrieve user details (e.g., ID)
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Store user ID
            new Claim(ClaimTypes.Name, user.FullName), // Optionally store user's full name
              new Claim(ClaimTypes.Email, user.Email), // User's email
                new Claim(ClaimTypes.Role, user.Role)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Products");
            }

            TempData["ErrorMessage"] = "Invalid email or password.";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign the user out and clear the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the homepage or login page after logging out
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        public IActionResult Register(string email, string password, string fullName, string phone, string confirmPassword) 
        {
            if(_userService.Register(email, fullName, password, phone, confirmPassword))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}
