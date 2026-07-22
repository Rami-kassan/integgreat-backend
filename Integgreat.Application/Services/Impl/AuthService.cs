using AutoMapper;
using Integgreat.Application.DTOs.Auth;
using Integgreat.Application.Exceptions;
using Integgreat.Domain.Entities;
using Integgreat.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Integgreat.Application.Services.Impl;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IWorkspaceMemberRepository workspaceMemberRepository,
        IMapper mapper,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _workspaceMemberRepository = workspaceMemberRepository;
        _mapper = mapper;
        _configuration = configuration;
    }
    // ═══════════════════════════════
    // GET ME
    // ═══════════════════════════════

    public async Task<MeResponseDto> GetMeAsync(int userId, int? workspaceId = null)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found");

        var permissions = new Dictionary<int, List<string>>();
        if (user is Client)
        {
            permissions = await _userRepository.GetClientPermissionsByWorkspaceAsync(userId);
        }

        return new MeResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user is Client ? "CLIENT" : "ADMIN",
            IsSuperAdmin = user is Admin admin && admin.IsSuperAdmin,
            Permissions = permissions
        };
    }


    // ═══════════════════════════════
    // LOGIN
    // ═══════════════════════════════
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) throw new UnauthorizedAppException("Invalid credentials");

        var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isValid) throw new UnauthorizedAppException("Invalid credentials");

        var token = GenerateToken(user);

        return new LoginResponseDto
        {
            Id = user.Id,
            Token = token,
            Name = user.Name,
            Email = user.Email,
            Role = user is Client ? "CLIENT" : "ADMIN",
            IsSuperAdmin = user is Admin admin && admin.IsSuperAdmin
        };
    }

    // ═══════════════════════════════
    // REGISTER CLIENT
    // ═══════════════════════════════
    public async Task<LoginResponseDto> RegisterClientAsync(ClientRegisterDto dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null) throw new ConflictException("Email already exists");

        var client = new Client
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Company = dto.Company,
            Phone = dto.Phone,
            BillingAddress = dto.BillingAddress
        };

        await _userRepository.AddAsync(client);

        var token = GenerateToken(client);

        return new LoginResponseDto
        {
            Token = token,
            Name = client.Name,
            Email = client.Email,
            Role = "CLIENT"
        };
    }

    // ═══════════════════════════════
    // REGISTER ADMIN
    // ═══════════════════════════════
    public async Task<LoginResponseDto> RegisterAdminAsync(AdminRegisterDto dto)
    {
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null) throw new ConflictException("Email already exists");

        var admin = new Admin
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsSuperAdmin = dto.IsSuperAdmin,
            CanManageUsers = dto.CanManageUsers
        };

        await _userRepository.AddAsync(admin);

        var token = GenerateToken(admin);

        return new LoginResponseDto
        {
            Token = token,
            Name = admin.Name,
            Email = admin.Email,
            Role = "ADMIN"
        };
    }

    // ═══════════════════════════════
    // GENERATE JWT TOKEN
    // ═══════════════════════════════
    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user is Client ? "CLIENT" : "ADMIN"),
            new Claim("isSuperAdmin", user is Admin admin && admin.IsSuperAdmin ? "True" : "False")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddDays(7),
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ═══════════════════════════════
    // CHANGE PASSWORD
    // ═══════════════════════════════
    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found.");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new Exception("Current password is incorrect.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
    }
}