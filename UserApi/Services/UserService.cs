using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ProjectX;
using ProjectX.Exceptions;
using Repository;
using TestApplication.DTO;
using TestApplication.Models;
using UserApi.Entities;

namespace TestApplication.Services;

public class UserService : IUserService
{
    private readonly IDbRepository _dbRepository;

    public UserService(IDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<Guid> CreateUserAsync(CreateUserRequest request)
    {
        if (!await IsLoginUniqueAsync(request.Login)) throw new IncorrectDataException("Login isn't unique");
        if (request.Login == null) throw new IncorrectDataException("Login can't be null");
        if (request.Password == null) throw new IncorrectDataException("Password can't be null");
        if (request.Login.Length < 4) throw new IncorrectDataException("Login must be longer than 4 symbols");
        if (request.Login.Length > 30) throw new IncorrectDataException("Login must be less than 30 symbols");
        if (request.Password.Length < 4) throw new IncorrectDataException("Password must be longer than 4 symbols");
        if (request.Password.Length > 30) throw new IncorrectDataException("Password must be less than 30 symbols");
        if (request.Email == null) throw new IncorrectDataException("Email can't be null");
        if (!await IsEmailUniqueAsync(request.Email)) throw new IncorrectDataException("Email isn't unique");
        if (!IsEmailValid(request.Email)) throw new IncorrectDataException("Email isn't valid");

        var salt = HashHandler.GenerateSalt(30);
        var id = Guid.NewGuid();
        var entity = new UserEntity
        {
            Id = id,
            Login = request.Login,
            Password = HashHandler.HashPassword(request.Password, salt),
            Email = request.Email,
            Salt = salt,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };
        var result = await _dbRepository.Add(entity);
        await AddRoleToUserAsync(new AddUserRoleRequest
        {
            UserId = id,
            RoleName = "User"
        });
        await _dbRepository.SaveChangesAsync();
        return id;
    }

    public List<UserWithRolesDTO> GetUsers()
    {
        var users = _dbRepository.GetAll<UserEntity>()
            .Include(u => u.UserRoleModels)
            .ThenInclude(ur => ur.RoleEntity)
            .ToList();

        if (users == null || users.Count == 0) throw new EntityNotFoundException("There are no users");

        var usersDTO = users.Select(user => new UserWithRolesDTO
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            RoleNames = user.UserRoleModels?.Select(ur => ur.RoleEntity.Role).ToList() ?? new List<string>(),
            DateCreated = user.DateCreated,
            DateUpdated = user.DateUpdated ?? null
        }).ToList();

        return usersDTO;
    }


    public async Task<UserWithRolesDTO> GetUser(Guid id)
    {
        var user = await _dbRepository.Get<UserEntity>()
            .Include(u => u.UserRoleModels)
            .ThenInclude(ur => ur.RoleEntity)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) throw new EntityNotFoundException("User not found");

        var userDTO = new UserWithRolesDTO
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.Password,
            Email = user.Email,
            Salt = user.Salt,
            RoleNames = user.UserRoleModels?.Select(ur => ur.RoleEntity.Role).ToList() ?? new List<string>()
        };

        return userDTO;
    }


    public async Task DeleteUserAsync(DeleteUserRequest request)
    {
        if (request.DeletedId == request.DeleterId) throw new IncorrectDataException("You can't delete yourself");
        var deleted = await _dbRepository.Get<UserEntity>()
            .Include(u => u.UserRoleModels)
            .ThenInclude(r => r.RoleEntity)
            .FirstOrDefaultAsync(x => x.Id == request.DeletedId);
        var deleter = await _dbRepository.Get<UserEntity>()
            .Include(u => u.UserRoleModels)
            .ThenInclude(r => r.RoleEntity)
            .FirstOrDefaultAsync(x => x.Id == request.DeleterId);
        if (deleted == null) throw new EntityNotFoundException("User not found");
        if (deleter == null) throw new EntityNotFoundException("User not found");

        if (IsDeletingAllowed(deleter, deleted))
        {
            await _dbRepository.Delete<UserEntity>(request.DeletedId);
            await _dbRepository.SaveChangesAsync();
        }
    }

    public async Task RemoveUserRoleAsync(RemoveUserRequest request)
    {
        if (!await IsUserRoleExistsAsync(request.UserId, request.RoleName))
            throw new UserRoleAlreadyExistsException("User hasn't this role.");

        var role = await _dbRepository.Get<RoleEntity>().FirstOrDefaultAsync(x => x.Role == request.RoleName);
        if (role == null) throw new EntityNotFoundException("Role not found");
        await _dbRepository.Delete<UserRoleEntity>(role.Id);
        await _dbRepository.SaveChangesAsync();
    }


    public async Task<bool> IsLoginUniqueAsync(string login)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == login);
        return user == null;
    }

    public async Task<bool> IsLoginUniqueForUserAsync(Guid userId, string login)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == login && x.Id != userId);
        return user != null;
    }


    public async Task AddRoleToUserAsync(AddUserRoleRequest request)
    {
        if (await IsUserRoleExistsAsync(request.UserId, request.RoleName))
            throw new UserRoleAlreadyExistsException("User already has this role.");

        var role = await _dbRepository.Get<RoleEntity>().FirstOrDefaultAsync(x => x.Role == request.RoleName);
        if (role == null) throw new EntityNotFoundException("Role not found");
        var userRole = new UserRoleEntity
        {
            UserId = request.UserId,
            RoleId = role.Id,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };
        var result = await _dbRepository.Add(userRole);
        await _dbRepository.SaveChangesAsync();
    }


    public async Task UpdateLogin(EditLoginRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) throw new EntityNotFoundException("User not found");
        if (request.NewLogin == null || request.UserId == null) throw new IncorrectDataException("Fill in all details");
        if (request.NewLogin.Length > 20) throw new IncorrectDataException("Login has to be shorter that 20 symbols");
        if (request.NewLogin.Length < 4) throw new IncorrectDataException("Login has to be longer than 4 symbols");
        /*if (!await IsLoginUniqueForUserAsync(request.UserId, request.NewLogin))
            throw new IncorrectDataException("Login isn't unique");*/
        user.Login = request.NewLogin;
        await _dbRepository.Update(user);
        await _dbRepository.SaveChangesAsync();
    }


    public async Task UpdateEmail(EditEmailRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) throw new EntityNotFoundException("User not found");
        if (request.NewEmail == null || request.UserId == null) throw new IncorrectDataException("Fill in all details");
        if (!IsEmailValid(request.NewEmail)) throw new IncorrectDataException("Email isn't valid");
        /*if (!await IsLoginUniqueForUserAsync(request.UserId, request.NewEmail))
            throw new IncorrectDataException("Email isn't unique");*/
        user.Email = request.NewEmail;
        await _dbRepository.Update(user);
        await _dbRepository.SaveChangesAsync();
    }


    public async Task UpdatePassword(EditPasswordRequest request)
    {
        if (request.NewPassword == null || request.Code == null)
            throw new IncorrectDataException("Fill in all details");
        if (request.NewPassword.Length > 20)
            throw new IncorrectDataException("Password has to be shorter that 20 symbols");
        if (request.NewPassword.Length < 4)
            throw new IncorrectDataException("Password has to be longer than 4 symbols");
        var code = await _dbRepository.Get<RestorePasswordRecordEntity>()
            .FirstOrDefaultAsync(x => x.Code == request.Code);
        if (code == null) throw new EntityNotFoundException("Code not found");
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Id == code.Id);
        if (user == null) throw new EntityNotFoundException("User not found");
        user.Password = HashHandler.HashPassword(request.NewPassword, user.Salt);
        await _dbRepository.Update(user);
        await _dbRepository.SaveChangesAsync();
    }


    public bool IsDeletingAllowed(UserEntity deleter, UserEntity deleted)
    {
        var deleterRoles = deleter.UserRoleModels.Select(ur => ur.RoleEntity.Role).ToArray();
        var deletedRoles = deleted.UserRoleModels.Select(ur => ur.RoleEntity.Role).ToArray();
        var deletedLeadRole = GetLeadingRole(deletedRoles);
        var deleterLeadRole = GetLeadingRole(deleterRoles);
        if (deleterLeadRole == deletedLeadRole) return false;
        if (deleterLeadRole == "SuperAdmin") return true;
        if (deleterLeadRole == "Admin" && deletedLeadRole != "SuperAdmin") return true;
        return false;
    }


    public string GetLeadingRole(string[] roles)
    {
        string[] rolePriority = { "SuperAdmin", "Admin", "Support", "User" };
        foreach (var role in rolePriority)
            if (roles.Contains(role))
                return role;
        return "User";
    }


    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Email == email);
        return user == null;
    }


    private async Task<bool> IsUserRoleExistsAsync(Guid userId, String roleName)
    {
        var isRoleExists = await _dbRepository.Get<UserRoleEntity>()
            .AnyAsync(ur => ur.UserId == userId &&
                            ur.RoleEntity.Role.ToLower() == roleName.ToLower());

        return isRoleExists;
    }

    public bool IsEmailValid(string email)
    {
        if (email.Length > 100) throw new IncorrectDataException("Email isn't valid");
        var regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
        return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
    }
}