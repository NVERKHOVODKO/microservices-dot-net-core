using TestApplication.DTO;

namespace TestApplication.Services;

public interface IUserService
{
    public Task<Guid> CreateUserAsync(CreateUserRequest request);

    public List<UserWithRolesDTO> GetUsers();

    public Task<UserWithRolesDTO> GetUser(Guid id);

    public Task DeleteUserAsync(Guid id);

    public Task<bool> IsLoginUniqueAsync(string login);

    public Task<bool> IsLoginUniqueForUserAsync(Guid userId, string login);
    public Task<Guid> AddRoleToUserAsync(AddUserRoleRequest request);
    public Task RemoveUserRoleAsync(Guid id);

    public Task UpdateLogin(EditLoginRequest request);

    public Task UpdateEmail(EditEmailRequest request);
    //public Task Update(UserEntity user);
    /*public Task AddRoleToUserAsync(AddUserRoleRequest roleRequest);

    public Task<UserGetResponse> GetUser(Guid userId);

    //public Task<List<UserGetResponse>> GetUsers();
    public Task<List<UserGetResponse>> GetUsers(int pageNumber, int pageSize);

    public Task DeleteUserAsync(Guid userId);
    public Task EditLoginAsync(EditLoginRequest request);

    public Task<bool> IsLoginUniqueAsync(string login);
    public Task<bool> IsLoginUniqueForUserAsync(Guid userId, string login);*/
}