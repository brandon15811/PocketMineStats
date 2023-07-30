using Microsoft.EntityFrameworkCore;

namespace PocketMineStats.Data;

public class StatsContext : DbContext
{
    public StatsContext(DbContextOptions<StatsContext> options) : base(options) { }

    public DbSet<ServerInfo> ServerInfo { get; set; }
    public DbSet<PluginStats> PluginStats { get; set; }
    public DbSet<PlayerCountHistory> PlayerCountHistory { get; set; }
    public DbSet<ServerTotals> ServerTotals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServerInfo>()
            .Property(e => e.Plugins)
            .HasColumnType("json");
        modelBuilder.Entity<ServerInfo>().OwnsOne(
            p => p.Players, entity =>
            {
                entity.Property(e => e.CurrentList)
                    .HasColumnType("json");
                entity.Property(e => e.HistoryList)
                    .HasColumnType("json");
            });

        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.Cores)
            .HasColumnType("json");
        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.GameVersions)
            .HasColumnType("json");
        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.Locations)
            .HasColumnType("json");
        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.OperatingSystems)
            .HasColumnType("json");
        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.PhpVersions)
            .HasColumnType("json");
        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.Platforms)
            .HasColumnType("json");
        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.Releases)
            .HasColumnType("json");
        modelBuilder.Entity<ServerTotals>()
            .Property(e => e.ServerVersions)
            .HasColumnType("json");
    }
}