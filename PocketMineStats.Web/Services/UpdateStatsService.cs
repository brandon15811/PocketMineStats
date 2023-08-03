using Microsoft.EntityFrameworkCore;
using PocketMineStats.Data;

namespace PocketMineStats.Services;

public class UpdateStatsService
{
    private readonly IServiceProvider _serviceProvider;

    public UpdateStatsService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task RunStats()
    {
        using var scope = _serviceProvider.CreateScope();
        var statsContext = scope.ServiceProvider.GetRequiredService<StatsContext>();
            
        var tenMinutesAgo = DateTimeOffset.Now.AddMinutes(-15);
        await statsContext.ServerInfo.Where(x => x.LastRequest < tenMinutesAgo).ExecuteDeleteAsync();

        if (!await statsContext.ServerInfo.AnyAsync())
        {
            statsContext.PlayerCountHistory.Add(new PlayerCountHistory { Date = DateTimeOffset.Now });
            statsContext.ServerTotals.Add(new ServerTotals { Date = DateTimeOffset.Now });
            await statsContext.SaveChangesAsync();
            return;
        }
            
        var playerStats = await statsContext.ServerInfo
            .GroupBy(x => 1)
            .Select(x => new PlayerCountHistory()
            {
                PlayerCount = x.Sum(y => y.Players.Count),
                PlayerLimit = x.Sum(y => y.Players.Limit),
                ServerCount = x.Count(),
                Date = DateTimeOffset.Now
            })
            .FirstOrDefaultAsync();
        statsContext.PlayerCountHistory.Add(playerStats);

        //Pretend like we're querying the ServerTotals table so we can easily map the data
        var serverTotals = await statsContext.ServerTotals.FromSqlRaw(@"SELECT
                '00000000-0000-0000-0000-000000000000' AS Id,
                CURDATE() AS Date,
                SUM(Players_Count) AS PlayerCount,
                SUM(Players_Limit) AS PlayerLimit,
                COUNT(*) AS ServerCount,
                (SELECT JSON_OBJECTAGG(System_Cores, CoreCount) FROM (SELECT System_Cores, COUNT(*) AS CoreCount FROM ServerInfo GROUP BY System_Cores) AS CoreCounts) AS Cores,
                (SELECT JSON_OBJECTAGG(PocketMine_MinecraftVersion, GameVersionCount) FROM (SELECT PocketMine_MinecraftVersion, COUNT(*) AS GameVersionCount FROM ServerInfo GROUP BY PocketMine_MinecraftVersion) AS GameVersionCounts) AS GameVersions,
                (SELECT JSON_OBJECTAGG(Country, CountryCount) FROM (SELECT Country, COUNT(*) AS CountryCount FROM ServerInfo GROUP BY Country) AS CountryCounts) AS Locations,
                (SELECT JSON_OBJECTAGG(System_OperatingSystem, OSCount) FROM (SELECT System_OperatingSystem, COUNT(*) AS OSCount FROM ServerInfo GROUP BY System_OperatingSystem) AS OSCounts) AS OperatingSystems,
                (SELECT JSON_OBJECTAGG(System_PhpVersion, PhpVersionCount) FROM (SELECT System_PhpVersion, COUNT(*) AS PhpVersionCount FROM ServerInfo GROUP BY System_PhpVersion) AS PhpVersionCounts) AS PhpVersions,
                (SELECT JSON_OBJECTAGG(System_Platform, PlatformCount) FROM (SELECT System_Platform, COUNT(*) AS PlatformCount FROM ServerInfo GROUP BY System_Platform) AS PlatformCounts) AS Platforms,
                (SELECT JSON_OBJECTAGG(System_Release, ReleaseCount) FROM (SELECT System_Release, COUNT(*) AS ReleaseCount FROM ServerInfo GROUP BY System_Release) AS ReleaseCounts) AS Releases,
                (SELECT JSON_OBJECTAGG(PocketMine_FullVersion, ServerVersionCount) FROM (SELECT PocketMine_FullVersion, COUNT(*) AS ServerVersionCount FROM ServerInfo GROUP BY PocketMine_FullVersion) AS ServerVersionCounts) AS ServerVersions
                FROM ServerInfo
            ").AsNoTracking().FirstOrDefaultAsync();
        serverTotals.Date = DateTimeOffset.Now;
        
        statsContext.ServerTotals.Add(serverTotals);

        var serverInfoPlugins = await statsContext.ServerInfo
            .Select(x => x.Plugins)
            .ToListAsync();
        
        var serverPluginStats = serverInfoPlugins
            .Where(x => x != null)
            .SelectMany(x => x)
            .ToList()
            .GroupBy(x => new { x.Name, x.Version })
            .Select(x => new PluginStats
            {
                Name = x.Key.Name,
                Count = x.Count(),
                Version = x.Key.Version,
            });
        
        var currentPlugins = await statsContext.PluginStats.ToListAsync();
        foreach (var plugin in serverPluginStats)
        {
            var existingPlugin = currentPlugins.FirstOrDefault(x => x.Name == plugin.Name && x.Version == plugin.Version);
            if (existingPlugin != null)
            {
                existingPlugin.Count = plugin.Count;
            }
            else
            {
                statsContext.PluginStats.Add(plugin);
            }
        }

        await statsContext.SaveChangesAsync();
    }
}