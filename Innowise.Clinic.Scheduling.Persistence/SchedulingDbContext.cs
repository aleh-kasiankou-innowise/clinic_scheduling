using Innowise.Clinic.Scheduling.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Scheduling.Persistence;

public class SchedulingDbContext : DbContext
{
    public SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) : base(options)
    {
    }

    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ShiftPreference> ShiftPreferences { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ReservedTimeSlot> ReservedTimeSlots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shift>()
            .HasKey(x => x.ShiftId);
        
        modelBuilder.Entity<Shift>()
            .Property(x => x.ShiftName)
            .HasColumnType("nvarchar(256)");

        modelBuilder.Entity<Shift>().HasData(new List<Shift>
        {
            new()
            {
                ShiftId = Guid.Parse("fca4da48-877d-4538-9bbe-927897dc0e92"),
                ShiftName = "9 to 6 (8 hours)",
                ShiftStart = new TimeOnly(9, 00),
                LunchStart = new TimeOnly(13, 00),
            },
            new()
            {
                ShiftId = Guid.Parse("df9f9f88-1a70-4161-b9a3-edbc521016a5"),
                ShiftName = "12 to 21 (8 hours)",
                ShiftStart = new TimeOnly(12, 00),
                LunchStart = new TimeOnly(16, 00),
            }
        });

        modelBuilder.Entity<ShiftPreference>()
            .HasKey(x => x.ShiftPreferenceId);
        
        modelBuilder.Entity<ShiftPreference>()
            .HasOne(x => x.Shift)
            .WithMany()
            .HasForeignKey(sp => sp.ShiftId);

        modelBuilder.Entity<Schedule>()
            .HasKey(x => x.ScheduleId);
        
        modelBuilder.Entity<Schedule>()
            .HasOne(x => x.Shift)
            .WithMany()
            .HasForeignKey(s => s.ShiftId);

        modelBuilder.Entity<ReservedTimeSlot>()
            .HasKey(x => x.ReservedTimeSlotId);
    }
}