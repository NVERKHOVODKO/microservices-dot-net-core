using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectX;
using ProjectX.Exceptions;
using Repository;
using TestApplication.DTO;
using TestApplication.Models;
using UserApi.Entities;

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
        if (await IsUserExists(request) == false) throw new EntityNotFoundException("There is no such user");
        var user = await _dbRepository.Get<UserEntity>()
            .Include(u => u.UserRoleModels)
            .ThenInclude(ur => ur.RoleEntity)
            .FirstOrDefaultAsync(u => u.Login == request.Login);
        var roles = await GetUserRoles(request);
        var claims = new List<Claim>();
        if (roles != null && roles.Any())
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
        claims.Add(new Claim("id", user.Id.ToString()));
        claims.Add(new Claim("name", user.Login));
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

    public async Task VerifyEmail(VerifyEmailRequest request)
    {
        var code = await _dbRepository.Get<EmailVerificationCodeEntity>()
            .FirstOrDefaultAsync(x => x.Email == request.Email);
        if (code == null) throw new EntityNotFoundException("An error occurred. The code was not sent");
        if (code.Code != request.Code) throw new AuthenticationException();
    }

    public async Task SendVerificationCode(string email)
    {
        if (email == null) throw new IncorrectDataException("Email can't be null");
        var existedCode = await _dbRepository.Get<EmailVerificationCodeEntity>().FirstOrDefaultAsync(x => x.Email == email);
        var random = new Random();
        if (existedCode != null)
        {
            var newCode = random.Next(1000, 9999).ToString();
            existedCode.Code = newCode;
            await _dbRepository.SaveChangesAsync();
            SendCode(email, newCode);
            return;
        }
        if (!IsEmailValid(email)) throw new IncorrectDataException("Email isn't valid");
        var code = random.Next(1000, 9999).ToString();
        var entity = new EmailVerificationCodeEntity
        {
            Id = Guid.NewGuid(),
            Email = email,
            Code = code,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
        SendCode(email, code);
    }

    public async Task<bool> IsUserExists(AuthRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == request.Login);
        if (user != null)
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

    private void SendCode(string email, string code)
    {
        var mm = new MailMessage();
        var sc = new SmtpClient("smtp.gmail.com");
        mm.From = new MailAddress("mikita.verkhavodka@gmail.com");
        mm.To.Add(email);
        mm.Subject = "Email confirmation";
        mm.Body = code;
        sc.Port = 587;
        sc.Credentials = new NetworkCredential("mikita.verkhavodka@gmail.com", "hors mfwv zsve lvye");
        sc.EnableSsl = true;
        sc.Send(mm);
    }

    public bool IsEmailValid(string email)
    {
        if (email.Length > 100) throw new IncorrectDataException("Email isn't valid");
        var regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
        return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
    }
    
    /*public async Task<List<string>> RestorePassword(RestorePasswordRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == request.Login);
        var userRoles = _dbRepository.GetAll<UserRoleEntity>().Where(r => r.UserId == user.Id);
        var roleNames = userRoles.Select(u => u.RoleEntity.Role).ToList();

        return roleNames;
    }*/
}