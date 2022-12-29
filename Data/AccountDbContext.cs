using Microsoft.EntityFrameworkCore;
using Hzg.Models;

namespace Hzg.Data;

/// <summary>
/// 后台账号
/// </summary>
public class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {}

    // 用户
    public virtual DbSet<HzgUser> Users { get; set; }
    // 用户分组
    public virtual DbSet<HzgGroup> Groups { get; set; }
    // 用户角色
    public virtual DbSet<HzgRole> Roles { get; set; }
    // 用户角色关联
    public virtual DbSet<HzgUserRole> UserRoles { get; set; }
    // 用户分组关联
    public virtual DbSet<HzgUserGroup> UserGroups { get; set; }
    // 角色分组关联
    public virtual DbSet<HzgRoleGroup> RoleGroups { get; set; }
    // 职位
    public virtual DbSet<HzgPosition> Positions { get; set; }
    // 菜单权限
    public virtual DbSet<HzgMenuPermission> MenuPermissions { get; set; }
    // 菜单
    public virtual DbSet<HzgMenu> Menus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 用户表
        modelBuilder.Entity<HzgUser>(b =>
        {
            b.ToTable("user");

            // 配置多对多关系
            b.HasMany(u => u.UserRoles).WithOne(u => u.User).HasForeignKey(r => r.UserId).IsRequired();
            b.HasMany(u => u.UserGroups).WithOne(u => u.User).HasForeignKey(r => r.UserId).IsRequired();
        });

        // 分组表
        modelBuilder.Entity<HzgGroup>(b =>
        {
            b.ToTable("group");

            // 配置多对多关系
            b.HasMany(g => g.UserGroups).WithOne(ug => ug.Group).HasForeignKey(ug => ug.GroupId).IsRequired();
            b.HasMany(g => g.GroupRoles).WithOne(rg => rg.Group).HasForeignKey(rg => rg.GroupId).IsRequired();
        });

        // 角色表
        modelBuilder.Entity<HzgRole>(b =>
        {
            b.ToTable("role");

            // 配置多对多关系
            b.HasMany(r => r.RoleGroups).WithOne(rg => rg.Role).HasForeignKey(rg => rg.RoleId).IsRequired();
            b.HasMany(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
        });

        // 用户分组
        modelBuilder.Entity<HzgUserGroup>(b =>
        {
            b.ToTable("user_group");

            b.HasKey(ug => new { ug.UserId, ug.GroupId });
        });

        // 用户角色
        modelBuilder.Entity<HzgUserRole>(b =>
        {
            b.ToTable("user_role");

            b.HasKey(ug => new { ug.UserId, ug.RoleId });
        });

        // 分组角色
        modelBuilder.Entity<HzgRoleGroup>(b =>
        {
            b.ToTable("role_group");

            b.HasKey(ug => new { ug.RoleId, ug.GroupId });
        });

        // 职位
        modelBuilder.Entity<HzgPosition>(b =>
        {
            b.ToTable("position");
        });

        // 菜单权限表
        modelBuilder.Entity<HzgMenuPermission>(mp =>
        {
            mp.ToTable("menu_permission");
        });

        // 菜单表
        modelBuilder.Entity<HzgMenu>(mp =>
        {
            mp.ToTable("menu");
        });
    }
}