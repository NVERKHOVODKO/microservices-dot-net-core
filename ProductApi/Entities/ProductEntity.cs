using System.ComponentModel.DataAnnotations;
using Entities;

public class ProductEntity : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public string? Description { get; set; }

    [Required] public decimal Price { get; set; }

    [Required] public bool Availability { get; set; }

    [Required] public Guid CreatorId { get; set; }
}