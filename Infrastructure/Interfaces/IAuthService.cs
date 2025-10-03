using Domain.DTOs.Auth;
using System;
using Microsoft.EntityFrameworkCore.Internal;
using Infrastructure.Response;
using Infrastructure.ApiResult;

namespace Infrastructure.Interfaces;

public interface IAuthService
{
    Task<Result<string>> RegisterAsync(RegisterRequestDto dto);
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
}
