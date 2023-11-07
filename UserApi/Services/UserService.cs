using Microsoft.EntityFrameworkCore;
using ProjectX;
using Repository;
using TestApplication.DTO;
using TestApplication.Models;

namespace TestApplication.Services;

public class UserService: IUserService
{
    private readonly IDbRepository _dbRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IDbRepository dbRepository, ILogger<UserService> logger)
    {
        _logger = logger;
        _dbRepository = dbRepository;
    }

    public async Task<Guid> CreateUserAsync(CreateUserRequest request)
    {
        var salt = HashHandler.GenerateSalt(30);
        var entity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            Password = HashHandler.HashPassword(request.Password, salt),
            Email = request.Email,
            Salt = salt
        };
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
        return result;
    }
    
    public List<UserEntity> GetUsers()
    {
        var users = _dbRepository.GetAll<UserEntity>().ToList();
        //if (user == null) throw new EntityNotFoundException("User not found");
        return users;
    }


    public Task<UserEntity> GetUser(Guid id)
    {
        var user = _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == id);
        //if (userList == null) throw new EntityNotFoundException("Users not found");
        return user;
    }


    /*public async Task EditLoginAsync(EditLoginRequest request)
    {
        var userToUpdate = await _userRepository.GetUserModelAsync(request.UserId);
        if (userToUpdate != null)
            await _userRepository.EditLoginAsync(userToUpdate, request.NewLogin);
        else
            _logger.LogInformation($"User with Id {request.UserId} was not found.");
    }

    public async Task AddRoleToUserAsync(AddUserRoleRequest roleRequest)
    {
        await _userRepository.AddRoleToUserAsync(roleRequest);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetUserModelAsync(userId);
        if (user == null) throw new EntityNotFoundException("User not found");
        await _userRepository.DeleteAsync(user);
    }


    


    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        var IsLoginUnique = await _userRepository.IsLoginUniqueAsync(login);
        return IsLoginUnique;
    }


    public async Task<bool> IsLoginUniqueForUserAsync(Guid userId, string login)
    {
        var IsLoginUnique = await _userRepository.IsLoginUniqueForUserAsync(userId, login);
        return IsLoginUnique;
    }*/

    /*public async Task<List<UserGetResponse>> GetFilteredAndSortedUsers(FilterSortUserRequest request)
    {
        IQueryable<UserModel> query = _context.Users
            .Include(u => u.UserRoleModels)
            .ThenInclude(ur => ur.RoleModel);

        foreach (var param in request.Filters)
        {
            if (!string.IsNullOrWhiteSpace(param.Param) && param.Min >= 0 && param.Max >= param.Min)
            {
                switch (param.Param.ToLower())
                {
                    case "age":
                        query = query.Where(u => u.Age >= param.Min && u.Age <= param.Max);
                        break;
                    case "name":
                        query = query.Where(u => u.Login.Length >= param.Min && u.Login.Length <= param.Max);
                        break;
                    case "email":
                        query = query.Where(u => u.Email.Length >= param.Min && u.Email.Length <= param.Max);
                        break;
                }
            }
        }

        switch (request.SortField.ToLower())
        {
            case "age":
                query = request.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(u => u.Age)
                    : query.OrderByDescending(u => u.Age);
                break;
            case "name":
                query = request.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(u => u.Login)
                    : query.OrderByDescending(u => u.Login);
                break;
            case "email":
                query = request.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(u => u.Email)
                    : query.OrderByDescending(u => u.Email);
                break;
        }
        
        var skipCount = (request.PageNumber - 1) * request.PageSize;
        var users = await query.Skip(skipCount).Take(request.PageSize).ToListAsync();
        var userGetResponses = users.Select(user => new UserGetResponse
        {
            Id = user.Id,
            Name = user.Login,
            Email = user.Email,
            Age = user.Age,
            Roles = user.UserRoleModels
                .Select(ur => new RoleModel
                {
                    Id = ur.RoleModel.Id,
                    Role = ur.RoleModel.Role
                })
                .ToList()
        }).ToList();

        return userGetResponses;
    }*/


    /*public async Task<List<RoleModel>> GetFilteredAndSortedRoles(FilterSortRolesRequest request)
    {
        IQueryable<RoleModel> query = _context.Roles;
        if (request.SelectedRoles != null && request.SelectedRoles.Any())
        {
            query = query.Where(role => request.SelectedRoles.Contains(role.Role));
        }

        if (!string.IsNullOrWhiteSpace(request.SortField))
        {
            switch (request.SortField.ToLower())
            {
                case "role":
                    if (request.SortDirection == SortDirection.Ascending)
                    {
                        query = query.OrderBy(role => role.Role);
                    }
                    else
                    {
                        query = query.OrderByDescending(role => role.Role);
                    }
                    break;
            }
        }
        var skipCount = (request.PageNumber - 1) * request.PageSize;
        var roles = await query.Skip(skipCount).Take(request.PageSize).ToListAsync();

        return roles;
    }*/
}