using System;
using Authorization.Database.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Database.DbLayer
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {

        public AppDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("AthApp");
        }
    }
}