using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;

namespace Buttons.Web;

public class PlayerIdProvider : IUserIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PlayerIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId(HubConnectionContext connection)
    {
        var role = _httpContextAccessor.HttpContext?.Request.Query["role"];

        if (role.HasValue && !StringValues.IsNullOrEmpty(role.Value))
        {
            if (role.Value == "player")
            {
                var username = _httpContextAccessor.HttpContext?.Request.Query["name"];

                if (username.HasValue && !StringValues.IsNullOrEmpty(username.Value))
                {
                    return username.Value;
                }

                return Guid.NewGuid().ToString();
            }

            return Guid.NewGuid().ToString();
        }

        throw new Exception("Missing role in query string");
    }
}