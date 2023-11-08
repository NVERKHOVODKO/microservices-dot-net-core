using Microsoft.EntityFrameworkCore;
using ProjectX;
using ProjectX.Exceptions;
using Repository;
using TestApplication.DTO;
using TestApplication.Models;

namespace TestApplication.Services;

public class UserService : IUserService
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
        if (!await IsLoginUniqueAsync(request.Login)) throw new IncorrectDataException("Login isn't unique");
        if (request.Login == null) throw new IncorrectDataException("Login can't be null");
        if (request.Password == null) throw new IncorrectDataException("Password can't be null");
        if (request.Login.Length < 4) throw new IncorrectDataException("Login must be longer than 3 symbols");
        if (request.Password.Length < 4) throw new IncorrectDataException("Password must be longer than 3 symbols");
        if (request.Email == null) throw new IncorrectDataException("Email can't be null");
        var salt = HashHandler.GenerateSalt(30);
        var entity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Login = request.Login,
            Password = HashHandler.HashPassword(request.Password, salt),
            Email = request.Email,
            Salt = salt,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
        return result;
    }

    public List<UserEntity> GetUsers()
    {
        var users = _dbRepository.GetAll<UserEntity>().ToList();
        if (users == null) throw new EntityNotFoundException("There is no users");
        return users;
    }


    public Task<UserEntity> GetUser(Guid id)
    {
        var user = _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) throw new EntityNotFoundException("User not found");
        return user;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) throw new EntityNotFoundException("User not found");
        await _dbRepository.Delete<UserEntity>(id);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task RemoveUserRoleAsync(Guid id)
    {
        var userRole = _dbRepository.Get<UserRoleEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (userRole == null) throw new EntityNotFoundException("Role not found");
        await _dbRepository.Delete<UserRoleEntity>(id);
        await _dbRepository.SaveChangesAsync();
    }

    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        var user = _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == login);
        return user != null;
    }


    public async Task<bool> IsLoginUniqueForUserAsync(Guid userId, string login)
    {
        var user = _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == login && x.Id != userId);
        return user != null;
    }

    public async Task<Guid> AddRoleToUserAsync(AddUserRoleRequest request)
    {
        var userRole = new UserRoleEntity
        {
            UserId = request.UserId,
            RoleId = request.RoleId,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };
        var result = await _dbRepository.Add(userRole);
        await _dbRepository.SaveChangesAsync();
        return result;
    }

    public async Task UpdateLogin(EditLoginRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) throw new EntityNotFoundException("User not found");
        if (request.NewLogin == null || request.UserId == null) throw new IncorrectDataException("Fill in all details");
        if (request.NewLogin.Length > 20) throw new IncorrectDataException("Login has to be shorter that 20 symbols");
        if (request.NewLogin.Length < 4) throw new IncorrectDataException("Login has to be longer than 3 symbols");
        if (!await IsLoginUniqueForUserAsync(request.UserId, request.NewLogin))
            throw new IncorrectDataException("Login isn't unique");
        user.Login = request.NewLogin;
        await _dbRepository.Update(user);
        await _dbRepository.SaveChangesAsync();
    }


    public async Task UpdateEmail(EditEmailRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) throw new EntityNotFoundException("User not found");
        if (request.NewEmail == null || request.UserId == null) throw new IncorrectDataException("Fill in all details");
        /*if (request.NewEmail.Length > 20) throw new IncorrectDataException("Login has to be shorter that 20 symbols");
        if (request.NewEmail.Length < 4) throw new IncorrectDataException("Login has to be longer than 3 symbols");*/
        if (!await IsLoginUniqueForUserAsync(request.UserId, request.NewEmail))
            throw new IncorrectDataException("Login isn't unique");
        user.Email = request.NewEmail;
        await _dbRepository.Update(user);
        await _dbRepository.SaveChangesAsync();
    }

    /*public async Task EditLoginAsync(EditLoginRequest request)
    {
        var userToUpdate = await _userRepository.GetUserModelAsync(request.UserId);
        if (userToUpdate != null)
            await _userRepository.EditLoginAsync(userToUpdate, request.NewLogin);
        else
            _logger.LogInformation($"User with Id {request.UserId} was not found.");
    }

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