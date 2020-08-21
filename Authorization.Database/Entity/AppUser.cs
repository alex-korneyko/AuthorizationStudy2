using System;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Database.Entity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}