using TestApplication.Models;

namespace TestApplication.DTO;

public class UserGetResponse
{
    public Guid Id { get; set; }

    public string Login { get; set; }
    public List<RoleEntity> Roles { get; set; }
}