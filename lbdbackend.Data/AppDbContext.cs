using Microsoft.EntityFrameworkCore;
using lbdbackend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

// dotnet ef --startup-project ..\lbdbackend.Api migrations add InitialMigration
// dotnet ef --startup-project ..\lbdbackend.Api database update
// dotnet ef --startup-project ..\lbdbackend.Api migration remove 
namespace lbdbackend.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Year> Years { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Profession> Professions { get; set; }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
