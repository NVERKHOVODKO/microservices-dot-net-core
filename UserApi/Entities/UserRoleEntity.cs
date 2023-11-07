using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities;

namespace TestApplication.Models;

public class UserRoleEntity: IEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required] public Guid UserId { get; set; }
    [ForeignKey("UserId")] public virtual UserEntity UserEntity { get; set; }
    [Required] public Guid RoleId { get; set; }
    [ForeignKey("RoleId")] public virtual RoleEntity RoleEntity { get; set; }
}