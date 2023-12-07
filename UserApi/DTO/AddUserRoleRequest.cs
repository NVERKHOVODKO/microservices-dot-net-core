namespace TestApplication.DTO;

/// <summary>
///     Request model for adding a user to a role.
/// </summary>
public class AddUserRoleRequest
{
    /// <summary>
    ///     Gets or sets the user's unique identifier.
    /// </summary>
    public Guid UserId { get; set; }

    public String RoleName { get; set; }
}