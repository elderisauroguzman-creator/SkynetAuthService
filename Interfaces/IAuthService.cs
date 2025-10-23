using AuthService.DTOs;

namespace AuthService.Interfaces
{
    public interface IAuthService
    {
        Task<DTOs.LoginResponseDto> LoginAsync(DTOs.LoginRequestDto loginDto);
        Task<DTOs.RegisterResponseDto> RegisterAsync(DTOs.RegisterRequestDto registerDto);
    }
}
