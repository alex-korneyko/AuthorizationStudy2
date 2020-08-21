using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Database.DbLayer;
using Authorization.Database.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Database.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

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
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var appUser = await _userManager.FindByNameAsync(loginDto.Username);

            if (appUser == null)
            {
                ModelState.AddModelError("Username", "User not found");
                return View(loginDto);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(appUser, loginDto.Password, false, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Username", "User not found");
                return View(loginDto);
            }

            return Redirect(loginDto.ReturnUrl);
        }

        public async Task<IActionResult> Logoff()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/Home/Index");
        }
    }
}