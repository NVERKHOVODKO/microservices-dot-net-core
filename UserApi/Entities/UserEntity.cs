using System.ComponentModel.DataAnnotations;
using Entities;

namespace TestApplication.Models;

public class UserEntity : IEntity
{
    public Guid Id { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Salt { get; set; }
    public List<UserRoleEntity> UserRoleModels { get; set; }
}