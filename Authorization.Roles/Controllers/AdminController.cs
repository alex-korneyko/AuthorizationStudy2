using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Roles.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "admin")]
        public IActionResult Administrator()
        {
            return View();
        }
        
        [Authorize(Policy = "manager")]
        public IActionResult Manager()
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
                new Claim(ClaimTypes.Name, loginDto.Username),
                new Claim(ClaimTypes.Role, "admin")
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