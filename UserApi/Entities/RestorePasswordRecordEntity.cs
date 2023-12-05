using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities;
using TestApplication.Models;

namespace UserApi.Entities;

public class RestorePasswordRecordEntity : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Code { get; set; }
    [Required] public string NewPassword { get; set; }
    [Required] public Guid UserId { get; set; }
    [ForeignKey("UserId")] public virtual UserEntity UserEntity { get; set; }
}