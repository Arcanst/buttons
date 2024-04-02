using System.Collections.Concurrent;
using FluentResults;

namespace Buttons.Web;

public class GameSession
{
    private ConcurrentDictionary<string, DateTime> _playersClicks = new();

    public async Task<Result> RegisterClick(string user, DateTime timestamp)
    {
        var addResult = _playersClicks.TryAdd(user, timestamp);

        return addResult
            ? Result.Ok()
            : Result.Fail($"Click for {user} already registered.");
    }

    public void ClearGame()
    {
        _playersClicks.Clear();
    }
}