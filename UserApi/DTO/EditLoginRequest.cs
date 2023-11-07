namespace TestApplication.DTO;

public class EditLoginRequest
{
    public Guid UserId { get; set; }
    public string NewLogin { get; set; }
}