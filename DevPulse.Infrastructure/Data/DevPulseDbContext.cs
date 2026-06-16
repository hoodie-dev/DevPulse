using DevPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Infrastructure.Data;

public class DevPulseDbContext: DbContext, IDevPulseDbContext
{
    public DevPulseDbContext(DbContextOptions<DevPulseDbContext> options) : base(options)
    {
        // The parent DbContext does all the initialization work using 'options'.
        // Nothing extra needs to be configured manually inside here!
    }

    // DbSets represent tables
    // EFcore uses these properties to map out DB tables
    public DbSet<Project> Projects{get;set;} = null!;
    public DbSet<Sprint> Sprints{get;set;} = null!;
    public DbSet<Issue> Issues{get;set;} = null!;
    public DbSet<Comment> Comments {get;set;} = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // every project must have a unique short code (e.g. no two projects can be "DEVP")
        modelBuilder.Entity<Project>()
        .HasIndex(p=>p.Code)
        .IsUnique();

        // configure strict max lengths for text columns to optimize database performance
        modelBuilder.Entity<Project>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Project>().Property(p => p.Code).HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Sprint>().Property(s => s.Name).HasMaxLength(100).IsRequired();
        
        modelBuilder.Entity<Issue>().Property(i => i.Title).HasMaxLength(200).IsRequired();
        modelBuilder.Entity<Comment>().Property(c=>c.Text).HasMaxLength(2000).IsRequired();
    }
}