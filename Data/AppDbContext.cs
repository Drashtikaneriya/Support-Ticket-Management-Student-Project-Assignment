using Microsoft.EntityFrameworkCore;
using Student_Project_Assignment.Models;
using Student_Project_Assignment.Enums;

namespace Student_Project_Assignment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketStatusLog> TicketStatusLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Role Seeding
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = RoleType.MANAGER },
                new Role { Id = 2, Name = RoleType.SUPPORT },
                new Role { Id = 3, Name = RoleType.USER }
            );

            // Initial Manager Seed (Password: Admin@123)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "System Admin",
                    Email = "admin@tickets.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    RoleId = 1, // MANAGER
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Role configuration
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasConversion<string>();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Ticket relationships
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            // TicketComment relationships
            modelBuilder.Entity<TicketComment>()
                .HasOne(tc => tc.Ticket)
                .WithMany(t => t.Comments)
                .HasForeignKey(tc => tc.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketComment>()
                .HasOne(tc => tc.User)
                .WithMany()
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // TicketStatusLog relationships
            modelBuilder.Entity<TicketStatusLog>()
                .HasOne(tsl => tsl.Ticket)
                .WithMany(t => t.StatusLogs)
                .HasForeignKey(tsl => tsl.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketStatusLog>()
                .HasOne(tsl => tsl.ChangedBy)
                .WithMany()
                .HasForeignKey(tsl => tsl.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}