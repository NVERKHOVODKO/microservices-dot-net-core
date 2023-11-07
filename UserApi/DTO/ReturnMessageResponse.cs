namespace TestApplication.DTO;

public class ReturnMessageResponse
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime SentTime { get; set; }
    public Guid SenderId { get; set; }
}