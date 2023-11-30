using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO;
using TestApplication.Services;

namespace TestApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var token = await _authService.GenerateTokenAsync(request);
        return Ok(new { token });
    }

    [HttpPost("sendVerificationCode")]
    [AllowAnonymous]
    public async Task<IActionResult> SendVerificationCode(SendVerificationCodeRequest request)
    {
        await _authService.SendVerificationCode(request.Email);
        return Ok("Sended");
    }

    [HttpPost("verifyEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        await _authService.VerifyEmail(request);
        return Ok("Verified");
    }

    [HttpPost("sendRestorePasswordMessage")]
    [AllowAnonymous]
    public async Task<IActionResult> SendRestorePasswordMessage([FromBody] RestorePasswordRequest request)
    {
        await _authService.SendRestorePasswordMessage(request);
        return Ok();
    }
}