using MobileShop.Application.DTOs.Auth;
using MobileShop.Domain.Entities;
using MobileShop.Domain.Enums;


namespace MobileShop.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<UserProfileDto> GetCurrentUserProfileAsync();
}

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    DateTime GetTokenExpiry();
}

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
public interface ICurrentUserService
{
    int? UserId { get; }
    string? Email { get; }
    UserRole? Role { get; }
    bool IsAuthenticated { get; }
}
