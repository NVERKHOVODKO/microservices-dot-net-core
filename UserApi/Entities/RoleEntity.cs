using Entities;

namespace TestApplication.Models;

public class RoleEntity : IEntity
{
    public string Role { get; set; }
    public List<UserRoleEntity> UserRoleModels { get; set; }
    public Guid Id { get; set; }
}