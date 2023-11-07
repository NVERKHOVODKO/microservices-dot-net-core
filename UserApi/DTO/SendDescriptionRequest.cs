namespace TestApplication.DTO;

public class SendDescriptionRequest
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public string Content { get; set; }
}