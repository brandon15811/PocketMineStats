using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketMineStats.Data;
using PocketMineStats.Data.Enums;
using PocketMineStats.Extensions;
using PocketMineStats.Models;
using PocketMineStats.Services;

namespace PocketMineStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsApiController : ControllerBase
    {
        private readonly StatsContext _context;
        private readonly IMapper _mapper;
        private readonly UpdateStatsService _updateStatsService;

        public StatsApiController(StatsContext context, IMapper mapper, UpdateStatsService updateStatsService)
        {
            _context = context;
            _mapper = mapper;
            _updateStatsService = updateStatsService;
        }
        
        [HttpPost("/api/post")]
        public async Task<ActionResult> Post(FullStatsRequest fullStatsRequest)
        {
            //TODO: Data validation
            var requestBody = await Request.GetRawBodyAsync();

            var server = await _context.ServerInfo.FirstOrDefaultAsync(x => x.UniqueServerId == fullStatsRequest.UniqueServerId);
            var newServer = MapRequest(fullStatsRequest, server);
            if (server == null)
            {
                _context.ServerInfo.Add(newServer);
                server = newServer;
            }

            server.Country = Request.Headers["CF-IPCountry"];

            await _context.SaveChangesAsync();

            return Ok();
        }

#if DEBUG
        [HttpGet("/api/forceParse")]
        public async Task<ActionResult> ForceParse()
        {
            await _updateStatsService.RunStats();
            return Ok();
        }  
#endif

        private ServerInfo MapRequest(FullStatsRequest fullStatsRequest, ServerInfo server)
        {
            switch (fullStatsRequest.Event)
            {
                case EventType.Open:
                    var openRequest = _mapper.Map<OpenRequest>(fullStatsRequest);
                    return server != null ? _mapper.Map(openRequest, server) : _mapper.Map<ServerInfo>(openRequest);
                case EventType.Status:
                    var statusRequest = _mapper.Map<StatusRequest>(fullStatsRequest);
                    return server != null ? _mapper.Map(statusRequest, server) : _mapper.Map<ServerInfo>(statusRequest);
                case EventType.Close:
                    var closeRequest = _mapper.Map<CloseRequest>(fullStatsRequest);
                    return server != null ? _mapper.Map(closeRequest, server) : _mapper.Map<ServerInfo>(closeRequest);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
