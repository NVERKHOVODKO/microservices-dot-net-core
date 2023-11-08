using Entities;

namespace TestApplication.Models;

public class RoleEntity : IEntity
{
    public Guid Id { get; set; }
    public string Role { get; set; }
    public List<UserRoleEntity> UserRoleModels { get; set; }
}