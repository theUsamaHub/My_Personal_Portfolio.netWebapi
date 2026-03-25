using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Personal_Portfolio.DTOs;
using My_Personal_Portfolio.Services;
using System.Security.Claims;

namespace My_Personal_Portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _configuration = configuration;
            _logger = logger;
        }

        // SECRET LOGIN ENDPOINT - Only you know the path!
        [HttpPost]
        [AllowAnonymous]
        [Route("{endpoint}")]
        public async Task<IActionResult> Login(string endpoint, [FromBody] LoginDto loginDto)
        {
            // Get the configured secret endpoint
            var secretEndpoint = Env.GetString("SECRET_LOGIN_ENDPOINT") ??
                                 _configuration["SecretLoginEndpoint"] ??
                                 "admin-console-login";

            // Validate the endpoint
            if (string.IsNullOrEmpty(endpoint) || endpoint != secretEndpoint)
            {
                _logger.LogWarning($"Unauthorized access attempt to login endpoint: {endpoint}");
                return NotFound();
            }

            // Get client IP for tracking
            loginDto.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        // SPECIAL REGISTRATION ENDPOINT - Only works if no admin exists
        [HttpPost("register-first-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterFirstAdmin([FromBody] RegisterAdminDto registerDto)
        {
            // Optional: Add a secret key check for extra security
            var masterSecretKey = Env.GetString("MASTER_SECRET_KEY") ??
                                  _configuration["Admin:MasterSecretKey"];

            if (!string.IsNullOrEmpty(masterSecretKey) &&
                registerDto.SecretKey != masterSecretKey)
            {
                _logger.LogWarning("Invalid master secret key used for registration");
                return Unauthorized(new { message = "Invalid secret key" });
            }

            var result = await _authService.RegisterAdminAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // Change password after first login
        [HttpPost("change-password")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);

            if (!result)
            {
                return BadRequest(new { message = "Failed to change password. Check your current password." });
            }

            return Ok(new { message = "Password changed successfully" });
        }

        // Test endpoint to verify authentication
        [HttpGet("verify")]
        [Authorize]
        public IActionResult Verify()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                authenticated = true,
                userId,
                email,
                role,
                message = "Token is valid"
            });
        }

        // Get secret endpoint path (for debugging, remove in production)
        [HttpGet("endpoint-info")]
        [Authorize(Roles = "admin")]
        public IActionResult GetEndpointInfo()
        {
            var secretEndpoint = Env.GetString("SECRET_LOGIN_ENDPOINT") ??
                                 _configuration["SecretLoginEndpoint"] ??
                                 "admin-console-login";

            return Ok(new
            {
                loginEndpoint = $"/api/auth/{secretEndpoint}",
                registerEndpoint = "/api/auth/register-first-admin",
                note = "Keep these endpoints secret! Register endpoint only works if no admin exists.",
                method = "POST"
            });
        }

    }
}