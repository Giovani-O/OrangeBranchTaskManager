using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace OrangeBranchTaskManager.Application.UseCases.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? GetUsername()
    {
        var user = _contextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
            return user.FindFirstValue(ClaimTypes.Name);

        return string.Empty;
    }

    public string? GetEmail()
    {
        var user = _contextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
            return user.FindFirstValue(ClaimTypes.Email);

        return string.Empty;
    }
}
