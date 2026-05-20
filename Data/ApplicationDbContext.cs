using SoftwareVersionManager.Models;
using Microsoft.EntityFrameworkCore;

namespace SoftwareVersionManager.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=localhost;Port=3306;Database=software_version_manager_dev;User=root;Password=;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Software>()
            .Property(s => s.Id)
            .HasDefaultValueSql("UUID()");

        modelBuilder.Entity<SoftwareVersion>()
            .Property(sv => sv.Id)
            .HasDefaultValueSql("UUID()");

        modelBuilder.Entity<SoftwareVersion>()
            .HasOne(sv => sv.Software)
            .WithMany(s => s.Versions)
            .HasForeignKey(sv => sv.SoftwareId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Software>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<SoftwareVersion>()
            .HasIndex(sv => new { sv.SoftwareId, sv.VersionNumber })
            .IsUnique();
    }
}

