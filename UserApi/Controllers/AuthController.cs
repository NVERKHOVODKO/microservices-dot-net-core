using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO;
using TestApplication.Services;
using System;
using System.Net;
using System.Net.Mail;

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
    public async Task<IActionResult> SendVerificationCode([FromBody] string email)
    {
        await _authService.SendVerificationCode(email);
        return Ok("Sended");
    }

    [HttpPost("verifyEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        await _authService.VerifyEmail(request);
        return Ok("Verified");
    }
    
    /*[HttpPatch("restorePassword")]
    [AllowAnonymous]
    public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordRequest request)
    {
        try
        {
            MailMessage mm = new MailMessage();
            SmtpClient sc = new SmtpClient("smtp.gmail.com");
            mm.From = new MailAddress("mikita.verkhavodka@gmail.com");
            mm.To.Add("mikita.verkhavodka@gmail.com");
            mm.Subject = "subj.Text";
            mm.Body = "content.Text";
            sc.Port = 587;
            sc.Credentials = new System.Net.NetworkCredential("mikita.verkhavodka@gmail.com", "hors mfwv zsve lvye");
            sc.EnableSsl = true;
            sc.Send(mm);
            return Ok("yes");
        }
        catch (Exception ex)//
        {
            Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
            return Ok("no");
        }
    }*/
}