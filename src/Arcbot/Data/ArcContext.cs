using Microsoft.EntityFrameworkCore;

namespace Arcbot.Data;

public class ArcContext : DbContext
{
    public ArcContext(DbContextOptions<ArcContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<MessageModel> Messages { get; set; }
    public DbSet<TriggerModel> Triggers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageModel>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<TriggerModel>()
            .HasKey(t => t.Id);
    }
}