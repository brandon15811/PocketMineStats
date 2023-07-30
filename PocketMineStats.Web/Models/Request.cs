using PocketMineStats.Data;
using PocketMineStats.Data.Enums;

namespace PocketMineStats.Models;

public class FullStatsRequest : BaseStatsRequest
{
    public ServerFullRequest Server { get; set; }
    public SystemFullRequest System { get; set; }
    public PlayersFullRequest Players { get; set; }
    public Dictionary<string, PluginRequest> Plugins { get; set; }
    public bool Crashing { get; set; }
}

public class ServerFullRequest
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

public class SystemFullRequest
{
    public string OperatingSystem { get; set; }
    public int Cores { get; set; }
    public string PhpVersion { get; set; }
    public string Machine { get; set; }
    public string Release { get; set; }
    public string Platform { get; set; }
    public long AvailableMemory { get; set; }
    public long TotalMemory { get; set; }
    public long MainMemory { get; set; }
    public int ThreadCount { get; set; }
}

public class PlayersFullRequest
{
    public int Count { get; set; }
    public int Limit { get; set; }
    public List<string> CurrentList { get; set; }
    public List<string> HistoryList { get; set; }
}

public class BaseStatsRequest
{
    public Guid UniqueServerId { get; set; }
    public Guid UniqueMachineId { get; set; }
    public Guid UniqueRequestId { get; set; }
    public EventType Event { get; set; }
}

public class OpenRequest : BaseStatsRequest
{
    public ServerOpenRequest Server { get; set; }
    public SystemOpenRequest System { get; set; }
    public PlayersOpenRequest Players { get; set; }
    public Dictionary<string, PluginRequest> Plugins { get; set; }
}

public class StatusRequest : BaseStatsRequest
{
    public ServerStatusRequest Server { get; set; }
    public SystemStatusRequest System { get; set; }
    public PlayersStatusRequest Players { get; set; }
}

public class CloseRequest : BaseStatsRequest
{
    public bool Crashing { get; set; }
}

public class PlayersOpenRequest
{
    public int Count { get; set; }
    public int Limit { get; set; }
}

public class PlayersStatusRequest : PlayersOpenRequest
{
    public List<string> CurrentList { get; set; }
    public List<string> HistoryList { get; set; }
}

public class PluginRequest
{
    public string Name { get; set; }
    public string Version { get; set; }
    public bool Enabled { get; set; }
}

public class ServerOpenRequest
{
    public int Port { get; set; }
    public string Software { get; set; }
    public string FullVersion { get; set; }
    public string Version { get; set; }
    public int Build { get; set; }
    public string Api { get; set; }
    public string MinecraftVersion { get; set; }
    public int Protocol { get; set; }
}

public class ServerStatusRequest
{
    public int TicksPerSecond { get; set; }
    public double TickUsage { get; set; }
    public long Ticks { get; set; }
}

public class SystemOpenRequest
{
    public string OperatingSystem { get; set; }
    public int Cores { get; set; }
    public string PhpVersion { get; set; }
    public string Machine { get; set; }
    public string Release { get; set; }
    public string Platform { get; set; }
}

public class SystemStatusRequest
{
    public long AvailableMemory { get; set; }
    public long TotalMemory { get; set; }
    public long MainMemory { get; set; }
    public int ThreadCount { get; set; }
}