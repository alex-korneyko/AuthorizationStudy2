using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Basics.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim("Demo", "Value")
            };
            
            var claimIdentity = new ClaimsIdentity(claims, "Cookie");
            
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            
            HttpContext.SignInAsync("CookieScheme", claimPrincipal);

            return Redirect(loginDto.ReturnUrl);
        }

        public IActionResult Logoff()
        {
            HttpContext.SignOutAsync("CookieScheme");

            return Redirect("/Home/Index");
        }
    }
}