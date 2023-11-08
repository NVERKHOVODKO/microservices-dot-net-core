namespace TestApplication.DTO;

public class EditDescriptionRequest
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public string NewDescription { get; set; }
}