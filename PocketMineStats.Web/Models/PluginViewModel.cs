namespace PocketMineStats.Models;

public class PluginViewModel
{
    public PluginViewModel(List<PluginDataViewModel> plugins)
    {
        Plugins = plugins;
    }
    public List<PluginDataViewModel> Plugins { get; set; }
}

public class PluginDataViewModel
{
    public string Name { get; set; }
    public int TotalCount { get; set; }
    public Dictionary<string, PluginVersionViewModel> Versions { get; set; }
}

public class PluginVersionViewModel
{
    public int Count { get; set; }
    public string Version { get; set; }
}