namespace TestApplication.DTO;

public class EditEmailRequest
{
    public Guid UserId { get; set; }
    public string NewEmail { get; set; }
}