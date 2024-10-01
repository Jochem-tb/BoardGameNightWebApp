using BGN.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Infrastructure
{
    public sealed class RepositoryDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<GameNight> GameNights { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;


        //public DbSet<Category> Categories { get; set; } = null!;
        //public DbSet<FoodOptions> FoodOptions { get; set; } = null!;

        //public DbSet<Gender> Genders { get; set; } = null!;
        //public DbSet<Genre> Genres { get; set; } = null!;



        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configuring the many-to-many relationship between Game and GameNight
            modelBuilder.Entity<Person>()
                .HasMany(p => p.GameNights)
                .WithMany(gn => gn.Attendees)
                .UsingEntity<Dictionary<string, object>>(
                    "GameNightAttendees",
                    j => j
                        .HasOne<GameNight>()
                        .WithMany()
                        .HasForeignKey("GameNightId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Person>()
                        .WithMany()
                        .HasForeignKey("AttendeesId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("GameNightId", "AttendeesId");
                    });

            //Configuring the one-to-many relationship between Review and Person --> Reviewer
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(p => p.Reviews).OnDelete(DeleteBehavior.Restrict);

            //Configuring the one-to-one relationship between GameNight and Organiser --> Person
            modelBuilder.Entity<GameNight>()
                .HasOne(gn => gn.Organiser);
        }



    }
}
