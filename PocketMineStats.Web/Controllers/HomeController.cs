using System.Diagnostics;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PocketMineStats.Data;
using PocketMineStats.Models;

namespace PocketMineStats.Controllers;

public class HomeController : Controller
{
    private readonly StatsContext _context;

    public HomeController(StatsContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Servers()
    {
        return View();
    }
    
    public async Task<IActionResult> Plugins()
    {
        var groupedQuery = await _context.PluginStats.AsQueryable().GroupBy(x => x.Name).ToListAsync();
        
        var data = groupedQuery
            .Select(x => new PluginDataViewModel
            {
                Name = x.Key,
                TotalCount = x.Count(),
                Versions = x.GroupBy(y => y.Version)
                    .ToDictionary(y => y.Key, y => new PluginVersionViewModel()
                    {
                        Count = y.Count(),
                        Version = y.Key
                    })
            })
            .OrderByDescending(x => x.TotalCount)
            .ToList();

        return View(new PluginViewModel(data));
    }
    
    public async Task<IActionResult> PluginDetail(string id)
    {
        var plugins = await _context.PluginStats
            .Where(x => x.Name == id)
            .ToListAsync();

        if (!plugins.Any())
        {
            return View("Error", new ErrorViewModel
            {
                Message = "Plugin not found."
            });
        }
        
        var data = new PluginDetailViewModel
        {
            Name = plugins.First().Name,
            EscapedName = HtmlEncoder.Default.Encode(plugins.First().Name),
            TotalCount = plugins.Sum(x => x.Count),
            VersionCount = plugins.Count,
            Versions = JsonConvert.SerializeObject(plugins.Select(version => new object[]{ HtmlEncoder.Default.Encode(version.Version), version.Count }))
        };
        return View(data);
    }
    
    public async Task<ActionResult> GlobalStats()
    {
        var data = await _context.PlayerCountHistory.ToListAsync();
        return Ok(data.ToDictionary(x => x.Date.ToUnixTimeMilliseconds(), x => new { x.ServerCount, x.PlayerCount, x.PlayerLimit }));
    }
    
    public async Task<ActionResult> ServerStats()
    {
        var data = await _context.ServerTotals
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .FirstOrDefaultAsync();

        data.PhpVersions = data.PhpVersions?.ToDictionary(x => Escape(x.Key), x => x.Value);
        data.OperatingSystems = data.OperatingSystems?.ToDictionary(x => Escape(x.Key), x => x.Value);
        data.Platforms = data.Platforms?.ToDictionary(x => Escape(x.Key), x => x.Value);
        data.Releases = data.Releases?.ToDictionary(x => Escape(x.Key), x => x.Value);
        data.GameVersions = data.GameVersions?.ToDictionary(x => Escape(x.Key), x => x.Value);
        data.Locations = data.Locations?.ToDictionary(x => Escape(x.Key), x => x.Value);
        data.ServerVersions = data.ServerVersions?.ToDictionary(x => Escape(x.Key), x => x.Value);

        var data2 = new
        {
            phpVersions = data.PhpVersions?.Select(x => new object[] { x.Key, x.Value }),
            operatingSystems = data.OperatingSystems?.Select(x => new object[] { x.Key, x.Value }),
            platforms = data.Platforms?.Select(x => new object[] { x.Key, x.Value }),
            cores = data.Cores?.Select(x => new object[] { x.Key, x.Value }),
            releases = data.Releases?.Select(x => new object[] { x.Key, x.Value }),
            gameVersions = data.GameVersions?.Select(x => new object[] { x.Key, x.Value }),
            locations = data.Locations?.Select(x => new object[] { x.Key, x.Value }),
            serverVersions = data.ServerVersions?.Select(x => new object[] { x.Key, x.Value }),
        };

        return Ok(data2);
    }

    private string Escape(string data) => HtmlEncoder.Default.Encode(data);

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}