namespace TestApplication.DTO;

public class SendMessageRequest
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
}