using Integgreat.Domain.Interfaces;
using System.Security.Claims;

namespace Integgreat.API.Helpers;

public class PermissionHelper
{
    private readonly IUserRepository _userRepository;

    public PermissionHelper(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission, int workspaceId)
    {
        var clientIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (clientIdStr == null) return false;

        var clientId = int.Parse(clientIdStr);
        var permissions = await _userRepository.GetClientPermissionsByWorkspaceAsync(clientId);

        if (!permissions.ContainsKey(workspaceId)) return false;
        return permissions[workspaceId].Contains(permission);
    }
}