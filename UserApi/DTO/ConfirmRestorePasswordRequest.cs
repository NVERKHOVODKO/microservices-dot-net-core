﻿namespace TestApplication.DTO;

public class ConfirmRestorePasswordRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}