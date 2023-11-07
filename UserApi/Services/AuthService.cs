using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectX;
using Repository;
using TestApplication.DTO;
using TestApplication.Models;

namespace TestApplication.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IDbRepository _dbRepository;
    private readonly ILogger<UserService> _logger;


    public AuthService(IDbRepository dbRepository, ILogger<UserService> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
        _dbRepository = dbRepository;
    }

    public async Task<string> GenerateTokenAsync(AuthRequest request)
    {
        if (await IsUserExists(request) == false) throw new KeyNotFoundException();

        //var user = await _userRepository.GetUserModelAsync(request.Login);
        //await _authRepository.RecordLoginAsync(user.Id);

        var roles = await GetUserRoles(request);

        var claims = new List<Claim>();

        if (roles != null && roles.Any())
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> IsUserExists(AuthRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == request.Login);

        if (user.Login == request.Login &&
            HashHandler.HashPassword(request.Password, user.Salt) == user.Password)
            return true;
        return false;
    }

    public async Task<List<string>> GetUserRoles(AuthRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == request.Login);
        var userRoles = _dbRepository.GetAll<UserRoleEntity>().Where(r => r.UserId == user.Id);
        var roleNames = userRoles.Select(u => u.RoleEntity.Role).ToList();

        return roleNames;
    }
}