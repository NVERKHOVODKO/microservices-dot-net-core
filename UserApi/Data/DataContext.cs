using Microsoft.EntityFrameworkCore;
using TestApplication.Models;
using UserApi.Entities;

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
    public DbSet<EmailVerificationCodeEntity> Codes { get; set; }
    public DbSet<RestorePasswordRecordEntity> RestorePasswordRecords { get; set; }
}