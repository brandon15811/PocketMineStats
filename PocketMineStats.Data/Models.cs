
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PocketMineStats.Data;

public class PluginStats
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public int Count { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}

public class PlayerCountHistory
{
    public Guid Id { get; set; }
    public int PlayerCount { get; set; }
    public int PlayerLimit { get; set; }
    public int ServerCount { get; set; }
    public DateTimeOffset Date { get; set; }
}

public class ServerTotals {
    [Key]
    public Guid Id { get; set; }
    public int PlayerCount { get; set; }
    public int PlayerLimit { get; set; }
    public int ServerCount { get; set; }
    public Dictionary<int, int> Cores { get; set; }
    public Dictionary<string, int> GameVersions { get; set; }
    public Dictionary<string, int> Locations { get; set; }
    public Dictionary<string, int> OperatingSystems { get; set; }
    public Dictionary<string, int> PhpVersions { get; set; }
    public Dictionary<string, int> Platforms { get; set; }
    public Dictionary<string, int> Releases { get; set; }
    public Dictionary<string, int> ServerVersions { get; set; }
    public DateTimeOffset Date { get; set; }
}

/////////////////////////////////

public class ServerInfo
{
    [Key]
    public Guid UniqueServerId { get; set; }
    public Guid UniqueMachineId { get; set; }
    public Guid UniqueRequestId { get; set; }
    public DateTimeOffset LastRequest { get; set; }
    //public string IpHash { get; set; }
    public string Country { get; set; }
    public List<Plugin> Plugins { get; set; }
    public Player Players { get; set; }
    public SystemInfo System { get; set; }
    public PocketMineInfo PocketMine { get; set; }
}

[Owned]
public class PocketMineInfo
{
    public int Port { get; set; }
    public string Software { get; set; }
    public string FullVersion { get; set; }
    public string Version { get; set; }
    public int Build { get; set; }
    public string Api { get; set; }
    public string MinecraftVersion { get; set; }
    public int Protocol { get; set; }
    public int TicksPerSecond { get; set; }
    public double TickUsage { get; set; }
    public long Ticks { get; set; }
}

[Owned]
public class Player
{
    public int Count { get; set; }
    public int Limit { get; set; }
    public List<string> CurrentList { get; set; }
    public List<string> HistoryList { get; set; }
}

public class Plugin
{
    public string Name { get; set; }
    public string Version { get; set; }
    public bool Enabled { get; set; }
}

[Owned]
public class SystemInfo
{
    public long AvailableMemory { get; set; }
    public long TotalMemory { get; set; }
    public long MainMemory { get; set; }
    public string PhpVersion { get; set; }
    public int ThreadCount { get; set; }
    public int Cores { get; set; }
    public string Machine { get; set; }
    public string Platform { get; set; }
    public string OperatingSystem { get; set; }
    public string Release { get; set; }
}

