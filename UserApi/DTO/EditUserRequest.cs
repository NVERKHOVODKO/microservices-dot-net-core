namespace TestApplication.DTO;

public class EditUserRequest
{
    public Guid UserId { get; set; }
    public string NewLogin { get; set; }
    public string NewPassword { get; set; }
    public string NewEmail { get; set; }
}