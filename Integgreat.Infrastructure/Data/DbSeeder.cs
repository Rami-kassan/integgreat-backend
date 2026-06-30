using Integgreat.Domain.Entities;
using Integgreat.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Roles.AnyAsync(r => r.Name == "Owner"))
        {
            var ownerRole = new Role
            {
                Name = "Owner",
                WorkspaceId = null
            };
            context.Roles.Add(ownerRole);
            await context.SaveChangesAsync();

            foreach (Permission perm in Enum.GetValues<Permission>())
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = ownerRole.Id,
                    Permission = perm
                });
            }
            await context.SaveChangesAsync();
        }
    }
}