// Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

public class ChatHub : Hub
{
    public async Task JoinServer(string serverId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Server-{serverId}");
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Room-{roomId}");
    }

    public async Task SendMessage(string roomId, string message)
    {
        // Validate user permissions through your services
        await Clients.Group($"Room-{roomId}").SendAsync("ReceiveMessage", Context.UserIdentifier, message);
    }

    public async Task StartVoiceCall(string roomId)
    {
        await Clients.Group($"Room-{roomId}").SendAsync("VoiceCallStarted", Context.UserIdentifier);
    }

    public async Task WebRTCSignal(string targetUserId, object signal)
    {
        await Clients.User(targetUserId).SendAsync("WebRTCSignal", Context.UserIdentifier, signal);
    }
}