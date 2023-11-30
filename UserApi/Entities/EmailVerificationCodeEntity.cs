using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities;
using TestApplication.Models;

namespace UserApi.Entities;

public class EmailVerificationCodeEntity : BaseEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Code { get; set; }
    [Required] public string Email { get; set; }
}