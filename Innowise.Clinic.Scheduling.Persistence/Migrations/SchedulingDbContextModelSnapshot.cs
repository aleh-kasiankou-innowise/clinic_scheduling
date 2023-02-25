﻿// <auto-generated />
using System;
using Innowise.Clinic.Scheduling.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Innowise.Clinic.Scheduling.Persistence.Migrations
{
    [DbContext(typeof(SchedulingDbContext))]
    partial class SchedulingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Innowise.Clinic.Scheduling.Persistence.Models.ReservedTimeSlot", b =>
                {
                    b.Property<Guid>("ReservedTimeSlotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AppointmentFinish")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("AppointmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AppointmentStart")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReservedTimeSlotId");

                    b.ToTable("ReservedTimeSlots");
                });

            modelBuilder.Entity("Innowise.Clinic.Scheduling.Persistence.Models.Schedule", b =>
                {
                    b.Property<Guid>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Day")
                        .HasColumnType("date");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OfficeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ShiftId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SpecializationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ScheduleId");

                    b.HasIndex("ShiftId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Innowise.Clinic.Scheduling.Persistence.Models.Shift", b =>
                {
                    b.Property<Guid>("ShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("LunchStart")
                        .HasColumnType("time");

                    b.Property<string>("ShiftName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<TimeSpan>("ShiftStart")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("WorkingHours")
                        .HasColumnType("time");

                    b.HasKey("ShiftId");

                    b.ToTable("Shifts");

                    b.HasData(
                        new
                        {
                            ShiftId = new Guid("fca4da48-877d-4538-9bbe-927897dc0e92"),
                            LunchStart = new TimeSpan(0, 13, 0, 0, 0),
                            ShiftName = "9 to 6 (8 hours)",
                            ShiftStart = new TimeSpan(0, 9, 0, 0, 0),
                            WorkingHours = new TimeSpan(0, 8, 0, 0, 0)
                        },
                        new
                        {
                            ShiftId = new Guid("df9f9f88-1a70-4161-b9a3-edbc521016a5"),
                            LunchStart = new TimeSpan(0, 16, 0, 0, 0),
                            ShiftName = "12 to 21 (8 hours)",
                            ShiftStart = new TimeSpan(0, 12, 0, 0, 0),
                            WorkingHours = new TimeSpan(0, 8, 0, 0, 0)
                        });
                });

            modelBuilder.Entity("Innowise.Clinic.Scheduling.Persistence.Models.ShiftPreference", b =>
                {
                    b.Property<Guid>("ShiftPreferenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OfficeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ShiftId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SpecializationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ShiftPreferenceId");

                    b.HasIndex("ShiftId");

                    b.ToTable("ShiftPreferences");
                });

            modelBuilder.Entity("Innowise.Clinic.Scheduling.Persistence.Models.Schedule", b =>
                {
                    b.HasOne("Innowise.Clinic.Scheduling.Persistence.Models.Shift", "Shift")
                        .WithMany()
                        .HasForeignKey("ShiftId");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("Innowise.Clinic.Scheduling.Persistence.Models.ShiftPreference", b =>
                {
                    b.HasOne("Innowise.Clinic.Scheduling.Persistence.Models.Shift", "Shift")
                        .WithMany()
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");
                });
#pragma warning restore 612, 618
        }
    }
}
