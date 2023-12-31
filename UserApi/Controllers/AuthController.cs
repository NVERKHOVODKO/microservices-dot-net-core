﻿using Microsoft.AspNetCore.Authorization;
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
    
    [HttpPost("get-user-token")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserToken([FromBody] GetTokenRequest request)
    {
        var token = await _authService.GetUserToken(request);
        return Ok(new { token });
    }

    [HttpPost("send-verification-code")]
    [AllowAnonymous]
    public async Task<IActionResult> SendVerificationCode([FromBody] SendVerificationCodeRequest request)
    {
        await _authService.SendVerificationCode(request.Email);
        return Ok();
    }

    [HttpPost("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        await _authService.VerifyEmail(request);
        return Ok("Verified");
    }

    [HttpPost("restore-password")]
    [AllowAnonymous]
    public async Task<IActionResult> SendRestorePasswordRequest([FromBody] RestorePasswordRequest request)
    {
        await _authService.SendRestorePasswordRequest(request);
        return Ok();
    }

    [HttpPost("confirm-restore-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmRestorePassword([FromBody] ConfirmRestorePasswordRequest request)
    {
        await _authService.ConfirmRestorePassword(request);
        return Ok();
    }
}