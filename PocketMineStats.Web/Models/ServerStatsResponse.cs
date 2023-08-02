namespace PocketMineStats.Models;

public class ServerStatsResponse
{
    public IEnumerable<object[]> PhpVersions { get; set; }
    public IEnumerable<object[]> OperatingSystems { get; set; }
    public IEnumerable<object[]> Platforms { get; set; }
    public IEnumerable<object[]> Cores { get; set; }
    public IEnumerable<object[]> Releases { get; set; }
    public IEnumerable<object[]> GameVersions { get; set; }
    public IEnumerable<object[]> Locations { get; set; }
    public IEnumerable<object[]> ServerVersions { get; set; }
}