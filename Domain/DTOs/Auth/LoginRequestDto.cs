using System;

namespace Domain.DTOs.Auth;

public class LoginRequestDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
