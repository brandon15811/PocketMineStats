using AutoMapper;
using PocketMineStats.Data;
using PocketMineStats.Extensions;
using PocketMineStats.Models;

namespace PocketMineStats.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile() => AddMappings();

    private void AddMappings()
    {
        CreateMap<OpenRequest, ServerInfo>()
            .Ignore(x => x.Country)
            .ForMember(x => x.LastRequest, x => x.MapFrom(y => DateTimeOffset.Now))
            .ForMember(x => x.PocketMine, x => x.MapFrom(y => y.Server))
            .ForMember(x => x.Plugins, x => x.MapFrom(y => y.Plugins.Values));

        CreateMap<PlayersOpenRequest, Player>()
            .Ignore(x => x.CurrentList)
            .Ignore(x => x.HistoryList);
        CreateMap<ServerOpenRequest, PocketMineInfo>()
            .Ignore(x => x.Ticks)
            .Ignore(x => x.TickUsage)
            .Ignore(x => x.TicksPerSecond);
        CreateMap<SystemOpenRequest, SystemInfo>()
            .Ignore(x => x.AvailableMemory)
            .Ignore(x => x.TotalMemory)
            .Ignore(x => x.MainMemory)
            .Ignore(x => x.ThreadCount);

        CreateMap<StatusRequest, ServerInfo>()
            .Ignore(x => x.Country)
            .Ignore(x => x.Plugins)
            .Ignore(x => x.PocketMine)
            .ForMember(x => x.LastRequest, x => x.MapFrom(y => DateTimeOffset.Now))
            .ForMember(x => x.PocketMine, x => x.MapFrom(y => y.Server));
        CreateMap<PlayersStatusRequest, Player>();
        CreateMap<ServerStatusRequest, PocketMineInfo>()
            .Ignore(x => x.Port)
            .Ignore(x => x.Software)
            .Ignore(x => x.FullVersion)
            .Ignore(x => x.Version)
            .Ignore(x => x.Build)
            .Ignore(x => x.Api)
            .Ignore(x => x.MinecraftVersion)
            .Ignore(x => x.Protocol);
        CreateMap<SystemStatusRequest, SystemInfo>()
            .Ignore(x => x.OperatingSystem)
            .Ignore(x => x.Cores)
            .Ignore(x => x.PhpVersion)
            .Ignore(x => x.Machine)
            .Ignore(x => x.Release)
            .Ignore(x => x.Platform);

        CreateMap<PluginRequest, Plugin>();

        CreateMap<CloseRequest, ServerInfo>()
            .Ignore(x => x.Country)
            .Ignore(x => x.Plugins)
            .Ignore(x => x.Players)
            .Ignore(x => x.System)
            .Ignore(x => x.PocketMine)
            .ForMember(x => x.LastRequest, x => x.MapFrom(y => DateTimeOffset.MinValue));

        CreateMap<FullStatsRequest, OpenRequest>();
        CreateMap<FullStatsRequest, StatusRequest>();
        CreateMap<FullStatsRequest, CloseRequest>();
        CreateMap<ServerFullRequest, ServerOpenRequest>();
        CreateMap<ServerFullRequest, ServerStatusRequest>();
        CreateMap<SystemFullRequest, SystemOpenRequest>();
        CreateMap<SystemFullRequest, SystemStatusRequest>();
        CreateMap<PlayersFullRequest, PlayersOpenRequest>();
        CreateMap<PlayersFullRequest, PlayersStatusRequest>();
    }
}