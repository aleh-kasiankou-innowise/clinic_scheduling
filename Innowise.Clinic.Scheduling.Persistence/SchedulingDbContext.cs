using Innowise.Clinic.Scheduling.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Scheduling.Persistence;

public class SchedulingDbContext : DbContext
{
    public SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) : base(options)
    {
    }

    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<ShiftPreference?> ShiftPreferences { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ReservedTimeSlot> ReservedTimeSlots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shift>()
            .HasKey(x => x.ShiftId);
        modelBuilder.Entity<Shift>()
            .Property(x => x.ShiftName)
            .HasColumnType("nvarchar(256)");
        modelBuilder.Entity<Shift>()
            .Property(x => x.ShiftStart)
            .HasColumnType("time");
        modelBuilder.Entity<Shift>()
            .Property(x => x.LunchStart)
            .HasColumnType("time");
        modelBuilder.Entity<Shift>()
            .Property(x => x.WorkingHours)
            .HasColumnType("time");

        modelBuilder.Entity<Shift>().HasData(new List<Shift>
        {
            new()
            {
                ShiftId = Guid.Parse("fca4da48-877d-4538-9bbe-927897dc0e92"),
                ShiftName = "9 to 6 (8 hours)",
                ShiftStart = new TimeSpan(9, 0, 0),
                LunchStart = new TimeSpan(13, 0, 0),
            },
            new()
            {
                ShiftId = Guid.Parse("df9f9f88-1a70-4161-b9a3-edbc521016a5"),
                ShiftName = "12 to 21 (8 hours)",
                ShiftStart = new TimeSpan(12, 0, 0),
                LunchStart = new TimeSpan(16, 0, 0),
            }
        });

        modelBuilder.Entity<Doctor>()
            .HasKey(x => x.DoctorId);
        
        modelBuilder.Entity<ShiftPreference>()
            .HasKey(x => x.ShiftPreferenceId);
        modelBuilder.Entity<ShiftPreference>()
            .HasOne(x => x.Doctor)
            .WithOne()
            .HasForeignKey<ShiftPreference>(x => x.DoctorId);
        modelBuilder.Entity<ShiftPreference>()
            .HasOne(x => x.Shift)
            .WithMany()
            .HasForeignKey(sp => sp.ShiftId);
        
        modelBuilder.Entity<Schedule>()
            .HasKey(x => x.ScheduleId);
        modelBuilder.Entity<Schedule>()
            .Property(x => x.Day)
            .HasColumnType("date");
        modelBuilder.Entity<Schedule>()
            .HasOne(x => x.Doctor)
            .WithMany()
            .HasForeignKey(x => x.DoctorId);
        modelBuilder.Entity<Schedule>()
            .HasOne(x => x.Shift)
            .WithMany()
            .HasForeignKey(s => s.ShiftId);

        modelBuilder.Entity<ReservedTimeSlot>()
            .HasKey(x => x.ReservedTimeSlotId);
        modelBuilder.Entity<ReservedTimeSlot>()
            .HasOne(x => x.Doctor)
            .WithMany()
            .HasForeignKey(x => x.DoctorId);
    }
}