using FluentResults;
using Microsoft.AspNetCore.SignalR;

namespace Buttons.Web;

public class GameHub : Hub
{
    private readonly GameSession _gameSession;

    public GameHub(GameSession gameSession)
    {
        _gameSession = gameSession;
    }

    public async Task<Result> RegisterClick()
    {
        var timestamp = DateTime.Now;
        var registerClick = await _gameSession.RegisterClick(Context.UserIdentifier, timestamp);

        if (registerClick.IsFailed)
        {
            return registerClick;
        }

        await Clients.Groups("admins").SendAsync("clickRegistered", Context.UserIdentifier, timestamp);

        return Result.Ok();
    }

    public async Task<Result> ResetGame()
    {
        _gameSession.ClearGame();
        await Clients.All.SendAsync("gameReset");

        return Result.Ok();
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var role = Context.GetHttpContext().Request.Query["role"];

        if (role == "player")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "players");

            await Clients.Groups("admins").SendAsync("playerJoined", Context.UserIdentifier);
        }
        else
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var role = Context.GetHttpContext().Request.Query["role"];

        if (role == "player")
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "players");

            await Clients.Groups("admins").SendAsync("playerLeft", Context.UserIdentifier);
        }
        else
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admins");
        }
    }
}