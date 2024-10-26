﻿using BGN.Domain.Entities;
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
        public DbSet<GameNightAttendee> Attendees { get; set; } = null!;


        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<FoodOptions> FoodOptions { get; set; } = null!;
        public DbSet<Gender> Genders { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;



        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);

            //Configuring the many-to-many relationship between Game and GameNight

            modelBuilder.Entity<GameNightAttendee>()
                .HasKey(gna => new { gna.GameNightId, gna.AttendeeId });

            modelBuilder.Entity<GameNightAttendee>()
            .HasOne(ga => ga.GameNight) // Each GameNightAttendee has one GameNight
            .WithMany(g => g.Attendees) // A GameNight can have many GameNightAttendees
            .HasForeignKey(ga => ga.GameNightId) // Foreign key in GameNightAttendee
            .OnDelete(DeleteBehavior.NoAction); // Specify delete behavior

            modelBuilder.Entity<GameNightAttendee>()
                .HasOne(ga => ga.Attendee) // Each GameNightAttendee has one Person
                .WithMany(p => p.GameNights) // A Person can have many GameNightAttendees
                .HasForeignKey(ga => ga.AttendeeId) // Foreign key in GameNightAttendee
                .OnDelete(DeleteBehavior.NoAction); // Specify delete behavior


            //Configuring the one-to-one relationship between GameNight and Organiser --> Person
            modelBuilder.Entity<GameNight>()
                .HasOne(gn => gn.Organiser);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var CategoryList = new List<Category>() 
            { 
                new() {Id = 1, Name="Partyspel" },
                new() {Id = 2, Name="Rollenspel" },
                new() {Id = 3, Name="Kaartspel" },
                new() {Id = 4, Name="Bordspel" },
                new() {Id = 5, Name="Kinderspel" } 
            };

            var GenreList = new List<Genre>()
            {
                //new() {Id = 1, Name="Geen" },
                new() {Id = 2, Name="Coöperatief" },
                new() {Id = 3, Name="Strategie" },
                new() {Id = 4, Name="Tactiek" },
                new() {Id = 5, Name="Abstract" },
                new() {Id = 6, Name="Familie" }
            };

            var GenderList = new List<Gender>()
            {
                new() {Id = 1, Name="M" },
                new() {Id = 2, Name="V" },
                new() {Id = 3, Name="X" }
            };

            var FoodOptionsList = new List<FoodOptions>()
            {
                new() {Id = 1, Name="Lactose" },
                new() {Id = 2, Name="Alcohol" },
                new() {Id = 3, Name="Vegetarisch" },
                new() {Id = 4, Name="Veganistisch" },
                new() {Id = 5, Name="Glutenvrij" },
                new() {Id = 6, Name="Noten" }
            };


            modelBuilder.Entity<Category>().HasData(CategoryList);
            modelBuilder.Entity<Genre>().HasData(GenreList);
            modelBuilder.Entity<Gender>().HasData(GenderList);
            modelBuilder.Entity<FoodOptions>().HasData(FoodOptionsList);
        }
    }
}
