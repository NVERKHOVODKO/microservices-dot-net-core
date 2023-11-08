namespace TestApplication.DTO;

public class EditPriceRequest
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public decimal NewPrice { get; set; }
}