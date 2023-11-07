using TestApplication.DTO;

namespace TestApplication.Services;

public interface IAuthService
{
    public Task<string> GenerateTokenAsync(AuthRequest request);
}