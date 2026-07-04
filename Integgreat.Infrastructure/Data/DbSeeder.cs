using Integgreat.Domain.Entities;
using Integgreat.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // ═══════════════════════════════
        // ROLE: Owner — toutes les permissions
        // ═══════════════════════════════
        if (!await context.Roles.AnyAsync(r => r.Name == "Owner"))
        {
            var ownerRole = new Role { Name = "Owner", WorkspaceId = null };
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

        // ═══════════════════════════════
        // ROLE: Manager
        // ═══════════════════════════════
        if (!await context.Roles.AnyAsync(r => r.Name == "Manager"))
        {
            var managerRole = new Role { Name = "Manager", WorkspaceId = null };
            context.Roles.Add(managerRole);
            await context.SaveChangesAsync();

            var managerPermissions = new[]
            {
                Permission.ViewHours,
                Permission.ViewContract,
                Permission.DownloadContract,
                Permission.ViewTask,
                Permission.ViewRequest,
                Permission.CreateRequest,
                Permission.ViewMembers,
            };

            foreach (var perm in managerPermissions)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = managerRole.Id,
                    Permission = perm
                });
            }
            await context.SaveChangesAsync();
        }

        // ═══════════════════════════════
        // ROLE: Collaborator
        // ═══════════════════════════════
        if (!await context.Roles.AnyAsync(r => r.Name == "Collaborator"))
        {
            var collabRole = new Role { Name = "Collaborator", WorkspaceId = null };
            context.Roles.Add(collabRole);
            await context.SaveChangesAsync();

            var collabPermissions = new[]
            {
                Permission.ViewHours,
                Permission.ViewTask,
                Permission.ViewRequest
            };

            foreach (var perm in collabPermissions)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = collabRole.Id,
                    Permission = perm
                });
            }
            await context.SaveChangesAsync();
        }

        // ═══════════════════════════════
        // ROLE: Guest — lecture seule
        // ═══════════════════════════════
        if (!await context.Roles.AnyAsync(r => r.Name == "Guest"))
        {
            var guestRole = new Role { Name = "Guest", WorkspaceId = null };
            context.Roles.Add(guestRole);
            await context.SaveChangesAsync();

            var guestPermissions = new[]
            {
                Permission.ViewTask,
            };

            foreach (var perm in guestPermissions)
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = guestRole.Id,
                    Permission = perm
                });
            }
            await context.SaveChangesAsync();
        }
    }
}