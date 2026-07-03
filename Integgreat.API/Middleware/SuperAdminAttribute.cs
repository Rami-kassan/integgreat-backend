using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Integgreat.API.Middleware;

public class SuperAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity!.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        var isSuperAdmin = user.FindFirst("isSuperAdmin")?.Value;

        if (role != "ADMIN" || isSuperAdmin != "True")
        {
            context.Result = new ForbidResult();
        }
    }
}