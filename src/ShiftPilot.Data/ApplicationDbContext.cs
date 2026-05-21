using Microsoft.EntityFrameworkCore;
using ShiftPilot.Core.Models;

namespace ShiftPilot.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Shift> Shifts { get; set; } = null!;
    public DbSet<SickLeave> SickLeaves { get; set; } = null!;
    public DbSet<ShiftSwapRequest> ShiftSwapRequests { get; set; } = null!;
    public DbSet<Availability> Availabilities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Shift configuration
        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.AssignedUser)
                .WithMany(u => u.AssignedShifts)
                .HasForeignKey(e => e.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // SickLeave configuration
        modelBuilder.Entity<SickLeave>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.SickLeaves)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ShiftSwapRequest configuration
        modelBuilder.Entity<ShiftSwapRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Initiator)
                .WithMany(u => u.InitiatedSwaps)
                .HasForeignKey(e => e.InitiatorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.TargetUser)
                .WithMany()
                .HasForeignKey(e => e.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Availability configuration
        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Availabilities)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
