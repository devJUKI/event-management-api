using EventManagementAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Infrastructure.Persistence;

public class EventManagementDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
    public DbSet<Category> Categories { get; set; }

    public EventManagementDbContext(DbContextOptions<EventManagementDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Roles
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );

        // Event Categories
        modelBuilder.Entity<EventCategory>()
            .HasKey(ec => new { ec.EventId, ec.CategoryId });

        modelBuilder.Entity<EventCategory>()
            .HasOne(ec => ec.Event)
            .WithMany(e => e.EventCategories)
            .HasForeignKey(ec => ec.EventId);

        modelBuilder.Entity<EventCategory>()
            .HasOne(ec => ec.Category)
            .WithMany(c => c.EventCategories)
            .HasForeignKey(ec => ec.CategoryId);

        modelBuilder.Entity<Category>().HasData(
            new Role { Id = 1, Name = "Tech" },
            new Role { Id = 2, Name = "Conference" },
            new Role { Id = 3, Name = "Workshop" },
            new Role { Id = 4, Name = "Seminar" },
            new Role { Id = 5, Name = "Networking" }
        );
    }
}
