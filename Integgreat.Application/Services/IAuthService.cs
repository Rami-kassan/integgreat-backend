using Integgreat.Application.DTOs.Auth;

namespace Integgreat.Application.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> RegisterClientAsync(ClientRegisterDto dto);
    Task<LoginResponseDto> RegisterAdminAsync(AdminRegisterDto dto);
    Task<MeResponseDto> GetMeAsync(int userId);
}