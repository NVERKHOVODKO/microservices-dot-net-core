using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Web;
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
        claims.Add(new Claim("email", user.Email));
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
        if (code == null) throw new EntityNotFoundException("An error occurred. The code wasn't sent");
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
            SendCodeAsync(email, newCode);// correct
            return;
        }
        if (!IsEmailValid(email)) throw new IncorrectDataException("Email isn't valid");
        var code = random.Next(1000, 9999).ToString();
        var entity = new EmailVerificationCodeEntity
        {
            Id = Guid.NewGuid(),
            Email = email,
            Code = code,
            DateCreated = DateTime.UtcNow
        };
        var result = await _dbRepository.Add(entity);
        await _dbRepository.SaveChangesAsync();
        SendCodeAsync(email, code);// correct
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
    

    private async Task SendCodeAsync(string email, string code)
    {
        Console.WriteLine(email, code);
        var mm = new MailMessage();
        var sc = new SmtpClient("smtp.gmail.com");

        mm.From = new MailAddress("mikita.verkhavodka@gmail.com");
        mm.To.Add(email);
        mm.Subject = "Email confirmation";
        mm.Body = $"Hello!<br>" +
                  $"Thank you for registering on our website.<br><br>" +
                  $"To complete the confirmation process, please enter the following code: <strong style='font-size: 25px;'>{code}</strong><br><br><br>" +
                  $"Please do not reply to this message.";
        mm.IsBodyHtml = true;
        sc.Port = 587;
        sc.Credentials = new NetworkCredential("mikita.verkhavodka@gmail.com", "hors mfwv zsve lvye");
        sc.EnableSsl = true;
        await sc.SendMailAsync(mm);
    }

    public bool IsEmailValid(string email)
    {
        if (email.Length > 100) throw new IncorrectDataException("Email isn't valid");
        var regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
        return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
    }
    
    private async Task<string> GenerateCode()
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("x2"));

            return sb.ToString().Substring(0, 8);
        }
    }
    
    public async Task<bool> IsRecordExists(Guid userId)
    {
        var recore = await _dbRepository.Get<RestorePasswordRecordEntity>(x => x.UserId == userId).FirstOrDefaultAsync();
        return recore != null;
    }
    
    public async Task SendRestorePasswordMessage(RestorePasswordRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>(x => x.Id == request.UserId).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        if (!await IsRecordExists(request.UserId))
        {
            var code = await GenerateCode();
            var entity = new RestorePasswordRecordEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Code = code,
                DateCreated = DateTime.UtcNow
            };
            await _dbRepository.Add(entity);
            await _dbRepository.SaveChangesAsync();
            SendLinkAsync(request, code);
        }
        else
        {
            var record = await _dbRepository.Get<RestorePasswordRecordEntity>(x => x.UserId == request.UserId).FirstOrDefaultAsync();
            string code = await GenerateCode();
            record.Code = code;
            await _dbRepository.SaveChangesAsync();
            SendLinkAsync(request, code);
        }
    }

    public async Task SendLinkAsync(RestorePasswordRequest request, string code)
    {
        var user = await _dbRepository.Get<UserEntity>(x => x.Id == request.UserId).FirstOrDefaultAsync();

        MailMessage mm = new MailMessage();
        SmtpClient sc = new SmtpClient("smtp.gmail.com");
        mm.From = new MailAddress("mikita.verkhavodka@gmail.com");
        mm.To.Add(user.Email);
        mm.Subject = "subj.Text";
        mm.Body = $"users/editPassword/{code}";
        sc.Port = 587;
        sc.Credentials = new System.Net.NetworkCredential("mikita.verkhavodka@gmail.com", "hors mfwv zsve lvye");
        sc.EnableSsl = true;
        sc.Send(mm);
        Console.WriteLine($"code: {code}\nuser.Email: {user.Email}");
    }
    
    
    
    /*public async Task SendRestorePasswordLink(RestorePasswordRequest request)
    {
        var user = await _dbRepository.Get<UserEntity>().FirstOrDefaultAsync(x => x.Login == request.Login);
        var userRoles = _dbRepository.GetAll<UserRoleEntity>().Where(r => r.UserId == user.Id);
        var roleNames = userRoles.Select(u => u.RoleEntity.Role).ToList();

        return roleNames;
    }*/
}