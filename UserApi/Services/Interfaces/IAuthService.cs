﻿using TestApplication.DTO;

namespace TestApplication.Services;

public interface IAuthService
{
    public Task<string> GenerateTokenAsync(AuthRequest request);
    public Task SendVerificationCode(string email);
    public Task VerifyEmail(VerifyEmailRequest request);
    public Task SendRestorePasswordRequest(RestorePasswordRequest request);
    public Task ConfirmRestorePassword(ConfirmRestorePasswordRequest request);
    public Task<string> GetUserToken(GetTokenRequest request);
}