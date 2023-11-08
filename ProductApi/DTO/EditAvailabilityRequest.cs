namespace TestApplication.DTO;

public class EditAvailabilityRequest
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public bool NewAvailability { get; set; }
}