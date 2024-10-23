﻿// <auto-generated />
using System;
using BGN.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BGN.Infrastructure.Migrations
{
    [DbContext(typeof(RepositoryDbContext))]
    [Migration("20241022140947_Changed_AttendanceStatus_Field_To_Nullable")]
    partial class Changed_AttendanceStatus_Field_To_Nullable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BGN.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Partyspel"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Rollenspel"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Kaartspel"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Bordspel"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Kinderspel"
                        });
                });

            modelBuilder.Entity("BGN.Domain.Entities.FoodOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("FoodOptions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Lactose"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Alcohol"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Vegetarisch"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Veganistisch"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Glutenvrij"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Noten"
                        });
                });

            modelBuilder.Entity("BGN.Domain.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("EstimatedTime")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsAdult")
                        .HasColumnType("bit");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<int>("MinPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("GenreId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("BGN.Domain.Entities.GameNight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("HouseNr")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("OnlyAdultWelcome")
                        .HasColumnType("bit");

                    b.Property<int>("OrganiserId")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("OrganiserId");

                    b.ToTable("GameNights");
                });

            modelBuilder.Entity("BGN.Domain.Entities.GameNightAttendee", b =>
                {
                    b.Property<int>("GameNightId")
                        .HasColumnType("int");

                    b.Property<int>("AttendeeId")
                        .HasColumnType("int");

                    b.Property<bool?>("AttendanceStatus")
                        .HasColumnType("bit");

                    b.HasKey("GameNightId", "AttendeeId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("BGN.Domain.Entities.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Genders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "M"
                        },
                        new
                        {
                            Id = 2,
                            Name = "V"
                        },
                        new
                        {
                            Id = 3,
                            Name = "X"
                        });
                });

            modelBuilder.Entity("BGN.Domain.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Name = "Coöperatief"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Strategie"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Tactiek"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Abstract"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Familie"
                        });
                });

            modelBuilder.Entity("BGN.Domain.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<string>("HouseNr")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("IdentityUserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("FoodOptionsGameNight", b =>
                {
                    b.Property<int>("FoodOptionsId")
                        .HasColumnType("int");

                    b.Property<int>("GameNightsId")
                        .HasColumnType("int");

                    b.HasKey("FoodOptionsId", "GameNightsId");

                    b.HasIndex("GameNightsId");

                    b.ToTable("FoodOptionsGameNight");
                });

            modelBuilder.Entity("FoodOptionsPerson", b =>
                {
                    b.Property<int>("PersonsId")
                        .HasColumnType("int");

                    b.Property<int>("PreferencesId")
                        .HasColumnType("int");

                    b.HasKey("PersonsId", "PreferencesId");

                    b.HasIndex("PreferencesId");

                    b.ToTable("FoodOptionsPerson");
                });

            modelBuilder.Entity("GameGameNight", b =>
                {
                    b.Property<int>("GameNightsId")
                        .HasColumnType("int");

                    b.Property<int>("GamesId")
                        .HasColumnType("int");

                    b.HasKey("GameNightsId", "GamesId");

                    b.HasIndex("GamesId");

                    b.ToTable("GameGameNight");
                });

            modelBuilder.Entity("BGN.Domain.Entities.Game", b =>
                {
                    b.HasOne("BGN.Domain.Entities.Category", "Category")
                        .WithMany("Games")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BGN.Domain.Entities.Genre", "Genre")
                        .WithMany("Games")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("BGN.Domain.Entities.GameNight", b =>
                {
                    b.HasOne("BGN.Domain.Entities.Person", "Organiser")
                        .WithMany()
                        .HasForeignKey("OrganiserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organiser");
                });

            modelBuilder.Entity("BGN.Domain.Entities.GameNightAttendee", b =>
                {
                    b.HasOne("BGN.Domain.Entities.Person", "Attendee")
                        .WithMany("GameNights")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BGN.Domain.Entities.GameNight", "GameNight")
                        .WithMany("Attendees")
                        .HasForeignKey("GameNightId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Attendee");

                    b.Navigation("GameNight");
                });

            modelBuilder.Entity("BGN.Domain.Entities.Person", b =>
                {
                    b.HasOne("BGN.Domain.Entities.Gender", "Gender")
                        .WithMany("Persons")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gender");
                });

            modelBuilder.Entity("FoodOptionsGameNight", b =>
                {
                    b.HasOne("BGN.Domain.Entities.FoodOptions", null)
                        .WithMany()
                        .HasForeignKey("FoodOptionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BGN.Domain.Entities.GameNight", null)
                        .WithMany()
                        .HasForeignKey("GameNightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FoodOptionsPerson", b =>
                {
                    b.HasOne("BGN.Domain.Entities.Person", null)
                        .WithMany()
                        .HasForeignKey("PersonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BGN.Domain.Entities.FoodOptions", null)
                        .WithMany()
                        .HasForeignKey("PreferencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameGameNight", b =>
                {
                    b.HasOne("BGN.Domain.Entities.GameNight", null)
                        .WithMany()
                        .HasForeignKey("GameNightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BGN.Domain.Entities.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BGN.Domain.Entities.Category", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("BGN.Domain.Entities.GameNight", b =>
                {
                    b.Navigation("Attendees");
                });

            modelBuilder.Entity("BGN.Domain.Entities.Gender", b =>
                {
                    b.Navigation("Persons");
                });

            modelBuilder.Entity("BGN.Domain.Entities.Genre", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("BGN.Domain.Entities.Person", b =>
                {
                    b.Navigation("GameNights");
                });
#pragma warning restore 612, 618
        }
    }
}