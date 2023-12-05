namespace TestApplication.DTO;

public class DeleteUserRequest
{
    public Guid DeleterId { get; set; }
    public Guid DeletedId { get; set; }
}