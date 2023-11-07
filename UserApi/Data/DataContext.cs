using Microsoft.EntityFrameworkCore;
using TestApplication.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        /*Database.EnsureDeleted();
        Database.EnsureCreated();*/
    }

    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
}