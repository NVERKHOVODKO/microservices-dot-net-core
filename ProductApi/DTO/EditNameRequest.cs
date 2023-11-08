namespace TestApplication.DTO;

public class EditNameRequest
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public string NewName { get; set; }
}