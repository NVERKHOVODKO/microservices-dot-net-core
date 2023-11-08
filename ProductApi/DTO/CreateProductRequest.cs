namespace TestApplication.DTO;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool Availability { get; set; }
    public Guid CreatorId { get; set; }
}