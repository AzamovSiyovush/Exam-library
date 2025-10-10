using System;
using System.Security.Claims;
using Domain.DTOs.Auth;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.ApiResult;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace Infrastructure.Services; 

public class AuthService(DataContext context, IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
         var user = await context.Users.FirstOrDefaultAsync(user => user.PhoneNumber == dto.PhoneNumber);
        if (user == null)
            return Result<LoginResponseDto>.Fail("Username or password incorrect.", ErrorType.UnAuthorized);

        bool verify = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!verify)
            return Result<LoginResponseDto>.Fail("Username or password incorrect.", ErrorType.UnAuthorized);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.MobilePhone, user.PhoneNumber),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Result<LoginResponseDto>.Ok(new LoginResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            IsBlocked = user.IsBlocked,
            Role = user.Role
        });
    }

    public async Task<Result<string>> RegisterAsync(RegisterRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            return Result<string>.Fail("Fullname is required.", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            return Result<string>.Fail("Phone number is required", ErrorType.Validation);

        if (string.IsNullOrWhiteSpace(dto.Password))
            return Result<string>.Fail("Password is required", ErrorType.Validation);

        if (dto.Password.Length < 4)
            return Result<string>.Fail("Password too short", ErrorType.Validation);

        if (dto.Password != dto.ConfirmPassword)
            return Result<string>.Fail("Password should match with confirm password", ErrorType.Validation);

        bool phoneExists = await context.Users.AnyAsync(user => user.PhoneNumber == dto.PhoneNumber);
        if (phoneExists)
            return Result<string>.Fail("Phone number already exists", ErrorType.Conflict);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var newUser = new User
        {
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            IsBlocked = false,
            Role = "User",
            PasswordHash = passwordHash
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return Result<string>.Ok(null, "User registered successfully");
    }
}
