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
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Year> Years { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Profession> Professions { get; set; }

        public DbSet<Person> People { get; set; }
        public DbSet<JoinMoviesPeople> JoinMoviesPeople { get; set; }
        public DbSet<JoinMoviesGenres> JoinMoviesGenres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<JoinMoviewsReviews> JoinMoviesReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
