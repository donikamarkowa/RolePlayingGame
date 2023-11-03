using Microsoft.EntityFrameworkCore;
using RolePlayingGame.Models;

namespace RolePlayingGame.Data
{
    public class RolePlayingGameDbContext : DbContext
    {
        public RolePlayingGameDbContext()
        {

        }

        public DbSet<Character> Characters { get; set; }

        public RolePlayingGameDbContext(DbContextOptions<RolePlayingGameDbContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-CTSCET9\\SQLEXPRESS01;Database=RDB;Integrated Security = True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Character>()
                .Property(c => c.CreatedOn)
                .HasDefaultValue(DateTime.UtcNow);

            base.OnModelCreating(builder);
        }
    }
}
