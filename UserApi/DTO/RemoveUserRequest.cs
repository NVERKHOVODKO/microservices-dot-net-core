namespace TestApplication.DTO;

public class RemoveUserRequest
{
    public Guid UserId { get; set; }

    public String RoleName { get; set; }
}