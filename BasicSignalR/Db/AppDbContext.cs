using BasicSignalR.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicSignalR.Db
{
    public class AppDbContext : DbContext { 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Conversation entity
            modelBuilder.Entity<Conversation>()
                .HasKey(c => c.Id);

            // Additional configurations, if needed

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Conversation> Conversations { get; set; }
    }
}
