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
    public virtual DbSet<User> Users { get; set; }
    // 用户分组
    public virtual DbSet<Group> Groups { get; set; }
    // 用户角色
    public virtual DbSet<Role> Roles { get; set; }
    // 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 用户表
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("User");

            // 配置多对多关系
            b.HasMany(u => u.UserRoles).WithOne(u => u.User).HasForeignKey(r => r.UserId).IsRequired();
            b.HasMany(u => u.UserGroups).WithOne(u => u.User).HasForeignKey(r => r.UserId).IsRequired();
        });

        // 分组表
        modelBuilder.Entity<Group>(b =>
        {
            b.ToTable("Group");

            // 配置多对多关系
            b.HasMany(g => g.UserGroups).WithOne(ug => ug.Group).HasForeignKey(ug => ug.GroupId).IsRequired();
            b.HasMany(g => g.GroupRoles).WithOne(rg => rg.Group).HasForeignKey(rg => rg.GroupId).IsRequired();
        });

        // 角色表
        modelBuilder.Entity<Role>(b =>
        {
            b.ToTable("Role");

            // 配置多对多关系
            b.HasMany(r => r.RoleGroups).WithOne(rg => rg.Role).HasForeignKey(rg => rg.RoleId).IsRequired();
            b.HasMany(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
        });

        // 用户分组
        modelBuilder.Entity<UserGroup>(b =>
        {
            b.ToTable("UserGroup");

            b.HasKey(ug => new { ug.UserId, ug.GroupId });
        });

        // 用户角色
        modelBuilder.Entity<UserRole>(b =>
        {
            b.ToTable("UserRole");

            b.HasKey(ug => new { ug.UserId, ug.RoleId });
        });

        // 分组角色
        modelBuilder.Entity<RoleGroup>(b =>
        {
            b.ToTable("RoleGroup");

            b.HasKey(ug => new { ug.RoleId, ug.GroupId });
        });
    }
}