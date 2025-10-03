using Domain.DTOs.Auth;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
           [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            return Ok(await authService.LoginAsync(dto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            return Ok(await authService.RegisterAsync(dto));
        }
    }
}
