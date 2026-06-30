using Integgreat.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Integgreat.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
    public DbSet<Request> Requests => Set<Request>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Héritage User → Client / Admin
        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("Type")
            .HasValue<Client>("CLIENT")
            .HasValue<Admin>("ADMIN");

        // Clé composite WorkspaceMember
        modelBuilder.Entity<WorkspaceMember>()
            .HasKey(wm => new { wm.ClientId, wm.WorkspaceId });

        // Clé composite RolePermission
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.Permission });

        // Project → Contracts (1 à plusieurs)
        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Project)
            .WithMany(p => p.Contracts)
            .HasForeignKey(c => c.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Role → Workspace (optionnel)
        modelBuilder.Entity<Role>()
            .HasOne(r => r.Workspace)
            .WithMany(w => w.Roles)
            .HasForeignKey(r => r.WorkspaceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}