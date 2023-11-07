namespace TestApplication.DTO;

public class DeleteChatRequest
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
}