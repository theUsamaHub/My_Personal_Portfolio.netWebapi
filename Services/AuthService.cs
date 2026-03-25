//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using My_Personal_Portfolio.Data;
//using My_Personal_Portfolio.DTOs;
//using My_Personal_Portfolio.Models;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace My_Personal_Portfolio.Services
//{
//    public interface IAuthService
//    {
//        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
//        Task InitializeAdminUserAsync();
//        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
//    }

//    public class AuthService : IAuthService
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<AuthService> _logger;

//        public AuthService(
//            ApplicationDbContext context,
//            IConfiguration configuration,
//            ILogger<AuthService> logger)
//        {
//            _context = context;
//            _configuration = configuration;
//            _logger = logger;
//        }

//        public async Task InitializeAdminUserAsync()
//        {
//            try
//            {
//                // Check if any admin exists
//                var adminExists = await _context.Users.AnyAsync(u => u.Role == "admin");

//                if (!adminExists)
//                {
//                    var adminEmail = _configuration["Admin:Email"];
//                    var adminPassword = _configuration["Admin:Password"];
//                    var adminFullName = _configuration["Admin:FullName"];

//                    // Validate admin config exists
//                    if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
//                    {
//                        _logger.LogWarning("Admin credentials not configured in appsettings.json");
//                        return;
//                    }

//                    // Create admin user
//                    var adminUser = new User
//                    {
//                        Id = Guid.NewGuid(),
//                        Email = adminEmail,
//                        FullName = adminFullName ?? "Administrator",
//                        Role = "admin",
//                        IsActive = true,
//                        EmailVerified = true,
//                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
//                        CreatedAt = DateTime.UtcNow,
//                        UpdatedAt = DateTime.UtcNow
//                    };

//                    await _context.Users.AddAsync(adminUser);
//                    await _context.SaveChangesAsync();

//                    _logger.LogInformation($"Admin user created with email: {adminEmail}");
//                    Console.WriteLine($"✅ Admin user created successfully!");
//                    Console.WriteLine($"📧 Email: {adminEmail}");
//                    Console.WriteLine($"🔑 Password: {adminPassword}");
//                    Console.WriteLine($"⚠️  IMPORTANT: Change this password after first login!");
//                }
//                else
//                {
//                    _logger.LogInformation("Admin user already exists");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error initializing admin user");
//                Console.WriteLine($"❌ Error creating admin user: {ex.Message}");
//            }
//        }

//        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
//        {
//            try
//            {
//                // Find user by email
//                var user = await _context.Users
//                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

//                if (user == null)
//                {
//                    _logger.LogWarning($"Login failed: User not found - {loginDto.Email}");
//                    return AuthResponseDto.ErrorResponse("Invalid email or password");
//                }

//                // Verify password
//                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
//                {
//                    _logger.LogWarning($"Login failed: Invalid password for {loginDto.Email}");
//                    return AuthResponseDto.ErrorResponse("Invalid email or password");
//                }

//                // Update login tracking
//                user.LastLogin = DateTime.UtcNow;
//                user.LastLoginIp = loginDto.IpAddress;
//                user.LoginCount++;
//                user.UpdatedAt = DateTime.UtcNow;

//                await _context.SaveChangesAsync();

//                // Generate JWT token
//                var token = GenerateJwtToken(user);

//                _logger.LogInformation($"User logged in: {user.Email}");

//                return AuthResponseDto.SuccessResponse(token, user);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error during login");
//                return AuthResponseDto.ErrorResponse("An error occurred during login");
//            }
//        }

//        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
//        {
//            try
//            {
//                var user = await _context.Users.FindAsync(userId);
//                if (user == null)
//                {
//                    return false;
//                }

//                // Verify current password
//                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
//                {
//                    return false;
//                }

//                // Update password
//                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
//                user.UpdatedAt = DateTime.UtcNow;

//                await _context.SaveChangesAsync();

//                _logger.LogInformation($"Password changed for user: {user.Email}");
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error changing password");
//                return false;
//            }
//        }

//        private string GenerateJwtToken(User user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Email, user.Email),
//                new Claim(ClaimTypes.Role, user.Role),
//                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
//                new Claim("UserId", user.Id.ToString()),
//                new Claim("Email", user.Email)
//            };

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Expires = DateTime.UtcNow.AddHours(24),
//                Issuer = _configuration["Jwt:Issuer"],
//                Audience = _configuration["Jwt:Audience"],
//                SigningCredentials = new SigningCredentials(
//                    new SymmetricSecurityKey(key),
//                    SecurityAlgorithms.HmacSha256Signature)
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using My_Personal_Portfolio.Data;
using My_Personal_Portfolio.DTOs;
using My_Personal_Portfolio.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;

namespace My_Personal_Portfolio.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAdminAsync(RegisterAdminDto registerDto);
        Task InitializeAdminUserAsync();
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InitializeAdminUserAsync()
        {
            try
            {
                // Check if any admin exists
                var adminExists = await _context.Users.AnyAsync(u => u.Role == "admin");

                if (!adminExists)
                {
                    // Try to read from .env first, then appsettings
                    var adminEmail = Env.GetString("ADMIN_EMAIL") ?? _configuration["Admin:Email"];
                    var adminPassword = Env.GetString("ADMIN_PASSWORD") ?? _configuration["Admin:Password"];
                    var adminFullName = Env.GetString("ADMIN_FULL_NAME") ?? _configuration["Admin:FullName"];

                    // Validate admin config exists
                    if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
                    {
                        _logger.LogWarning("Admin credentials not configured in .env or appsettings.json");
                        Console.WriteLine("⚠️  WARNING: No admin credentials found!");
                        Console.WriteLine("Please set ADMIN_EMAIL and ADMIN_PASSWORD in .env file");
                        return;
                    }

                    // Create admin user
                    var adminUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = adminEmail,
                        FullName = adminFullName ?? "Administrator",
                        Role = "admin",
                        IsActive = true,
                        EmailVerified = true,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _context.Users.AddAsync(adminUser);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"✅ Admin user created with email: {adminEmail}");
                    Console.WriteLine("=".PadRight(50, '='));
                    Console.WriteLine("✅ ADMIN USER CREATED SUCCESSFULLY!");
                    Console.WriteLine($"📧 Email: {adminEmail}");
                    Console.WriteLine($"🔑 Password: {adminPassword}");
                    Console.WriteLine($"⚠️  IMPORTANT: Delete or change this password immediately!");
                    Console.WriteLine("=".PadRight(50, '='));
                }
                else
                {
                    _logger.LogInformation("Admin user already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing admin user");
                Console.WriteLine($"❌ Error creating admin user: {ex.Message}");
            }
        }

        public async Task<AuthResponseDto> RegisterAdminAsync(RegisterAdminDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

                if (existingUser != null)
                {
                    return AuthResponseDto.ErrorResponse("User with this email already exists");
                }

                // Check if any admin already exists
                var adminExists = await _context.Users.AnyAsync(u => u.Role == "admin");
                if (adminExists)
                {
                    return AuthResponseDto.ErrorResponse("Admin user already exists. Only one admin allowed.");
                }

                // Create new admin user
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = registerDto.Email,
                    FullName = registerDto.FullName,
                    Role = "admin",
                    IsActive = true,
                    EmailVerified = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                // Generate token
                var token = GenerateJwtToken(newUser);

                _logger.LogInformation($"New admin registered: {newUser.Email}");

                return AuthResponseDto.SuccessResponse(token, newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering admin");
                return AuthResponseDto.ErrorResponse("An error occurred during registration");
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

                if (user == null)
                {
                    _logger.LogWarning($"Login failed: User not found - {loginDto.Email}");
                    return AuthResponseDto.ErrorResponse("Invalid email or password");
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning($"Login failed: Invalid password for {loginDto.Email}");
                    return AuthResponseDto.ErrorResponse("Invalid email or password");
                }

                // Update login tracking
                user.LastLogin = DateTime.UtcNow;
                user.LastLoginIp = loginDto.IpAddress;
                user.LoginCount++;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = GenerateJwtToken(user);

                _logger.LogInformation($"User logged in: {user.Email}");

                return AuthResponseDto.SuccessResponse(token, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return AuthResponseDto.ErrorResponse("An error occurred during login");
            }
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return false;
                }

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    return false;
                }

                // Update password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Password changed for user: {user.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return false;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = Env.GetString("JWT_KEY") ?? _configuration["Jwt:Key"];
            var jwtIssuer = Env.GetString("JWT_ISSUER") ?? _configuration["Jwt:Issuer"];
            var jwtAudience = Env.GetString("JWT_AUDIENCE") ?? _configuration["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}