using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.JwtBearer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "Alex"),
                new Claim(JwtRegisteredClaimNames.Email, "Alex@ya.ru")
            };

            var secretBytes = Encoding.UTF8.GetBytes("saksdjhjkghsdkghlsdkjgioasdjflsfjka");
            SecurityKey key = new SymmetricSecurityKey(secretBytes);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "Issuer", 
                "Aud", 
                claims,
                notBefore: DateTime.Now, 
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

            ViewBag.Token = stringToken;
            
            return View();
        }
    }
}