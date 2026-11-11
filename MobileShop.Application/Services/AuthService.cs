using MobileShop.Application.DTOs.Auth;
using MobileShop.Application.Interfaces;
using MobileShop.Application.Interfaces.Services;
using MobileShop.Domain.Entities;
using MobileShop.Domain.Enums;
using MobileShop.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace MobileShop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICurrentUserService _currentUserService;

        public AuthService(
            IApplicationDbContext context,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _currentUserService = currentUserService;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var emailNormalized = request.Email.Trim().ToLower();
            var existingUser = await _context.Users.AnyAsync(u => u.Email.ToLower() == emailNormalized);
            if (existingUser)
            {
                throw new BadRequestException("Email đã được đăng ký trong hệ thống.");
            }

            var user = new User
            {
                FullName = request.FullName.Trim(),
                Email = emailNormalized,
                PhoneNumber = request.PhoneNumber.Trim(),
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Role = UserRole.Customer,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Tạo giỏ hàng mặc định cho user
            var cart = new Cart
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                ExpiresAt = _jwtTokenGenerator.GetTokenExpiry(),
                User = new UserProfileDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    Address = user.Address
                }
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var emailNormalized = request.Email.Trim().ToLower();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalized);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new BadRequestException("Email hoặc mật khẩu không chính xác.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                ExpiresAt = _jwtTokenGenerator.GetTokenExpiry(),
                User = new UserProfileDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    Address = user.Address
                }
            };
        }

        public async Task<UserProfileDto> GetCurrentUserProfileAsync()
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedException("Vui lòng đăng nhập.");
            var user = await _context.Users.FindAsync(userId) ?? throw new NotFoundException("Người dùng", userId);

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                Address = user.Address
            };
        }
    }
}
