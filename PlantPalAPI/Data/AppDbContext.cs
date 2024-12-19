
using Microsoft.EntityFrameworkCore;
using PlantPalAPI.Models;

namespace PlantPalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Plant ve User bağlama
            modelBuilder.Entity<Plant>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId);
        }
    }
}

