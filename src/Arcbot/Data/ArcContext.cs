using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Arcbot.Data;

public class ArcContext : DbContext
{
    public DbSet<MessageModel> Messages { get; set; }

    public ArcContext(DbContextOptions<ArcContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageModel>()
            .HasKey(t => t.Id);
    }
}