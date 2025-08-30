using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscordClone.Models;
using DiscordClone.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DiscordClone.DTOs;

namespace DiscordClone.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;



    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Discord()
    {
        return View();
    }

    public IActionResult CreateServer()
    {
        return View();
    }

   
    public async Task<IActionResult> Server(int? serverId = null)
    {
        // If no serverId provided, get the most recent server (for demo purposes)
        // In a real app, you might get this from session, user preferences, etc.
        Server? server;
        
        if (serverId.HasValue)
        {
            server = await _context.Servers
                .Include(s => s.Admin)
                .Include(s => s.Channels)
                .Include(s => s.Members)
                .FirstOrDefaultAsync(s => s.Id == serverId.Value);
        }
        else
        {
            // Get the most recent server for demo
            server = await _context.Servers
                .Include(s => s.Admin)
                .Include(s => s.Channels)
                .Include(s => s.Members)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }

        if (server == null)
        {
            // If no server exists, create a default one or redirect
            return RedirectToAction("Index");
        }

        // Create view model with server data
        var serverViewModel = new ServerViewModel
        {
            Id = server.Id,
            Name = server.Name,
            Description = server.Description,
            Icon = server.Icon,
            AdminName = server.Admin?.UserName ?? "Unknown",
            CreatedAt = server.CreatedAt,
            InviteCode = server.InviteCode,
            MemberCount = server.Members?.Count ?? 0,
            ChannelCount = server.Channels?.Count ?? 0,
            Channels = server.Channels?.ToList() ?? new List<Channel>()
        };

        return View(serverViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // API endpoint to create server
    [HttpPost]
    public async Task<IActionResult> CreateServer([FromBody] CreateServerRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { success = false, message = "Server name is required" });
            }

            // Generate invite code
            var inviteCode = GenerateInviteCode();

            // Create new server
            var server = new Server
            {
                Name = request.Name,
                Description = request.Description,
                Icon = request.Icon,
                AdminId = "1", // For demo, use admin ID 1. In real app, get from current user
                CreatedAt = DateTime.UtcNow,
                InviteCode = inviteCode
            };

            // Add default channels
            server.Channels.Add(new Channel
            {
                Name = "general",
                Type = ChannelType.Text,
                ServerId = server.Id
            });

            server.Channels.Add(new Channel
            {
                Name = "random",
                Type = ChannelType.Text,
                ServerId = server.Id
            });

            // Add admin as member
            server.Members.Add(new ServerMember
            {
                UserId = "1", // For demo, use user ID 1
                ServerId = server.Id,
                Role = ServerRole.Admin,
                JoinedAt = DateTime.UtcNow
            });

            _context.Servers.Add(server);
            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                serverId = server.Id,
                message = $"Server '{server.Name}' created successfully!" 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating server");
            return StatusCode(500, new { success = false, message = "Error creating server" });
        }
    }

    private string GenerateInviteCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

// View Model for Server data
public class ServerViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string AdminName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? InviteCode { get; set; }
    public int MemberCount { get; set; }
    public int ChannelCount { get; set; }
    public List<Channel> Channels { get; set; } = new List<Channel>();
}

// Request model for creating server
public class CreateServerRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Template { get; set; }
    public string? Purpose { get; set; }
}
