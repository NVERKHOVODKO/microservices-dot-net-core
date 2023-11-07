namespace TestApplication.DTO;

/// <summary>
///     Request model for retrieving a list of users with pagination.
/// </summary>
public class GetUsersRequest
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}