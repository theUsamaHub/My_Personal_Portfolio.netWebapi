//using System;
//using System.ComponentModel.DataAnnotations;

//namespace My_Personal_Portfolio.DTOs
//{
//    // For login requests
//    public class LoginDto
//    {
//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid email format")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Password is required")]
//        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
//        public string Password { get; set; }
//    }

//    // For creating/updating users (Admin only)
//    public class UserCreateDto
//    {
//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid email format")]
//        [StringLength(255)]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Password is required")]
//        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
//        public string Password { get; set; }

//        [StringLength(100)]
//        public string FullName { get; set; }

//        [StringLength(50)]
//        public string Role { get; set; } = "admin";
//    }

//    // For updating user
//    public class UserUpdateDto
//    {
//        [StringLength(100)]
//        public string FullName { get; set; }

//        [StringLength(50)]
//        public string Role { get; set; }

//        public bool? IsActive { get; set; }
//    }

//    // For user response (don't send sensitive data)
//    public class UserResponseDto
//    {
//        public Guid Id { get; set; }
//        public string Email { get; set; }
//        public string FullName { get; set; }
//        public string Role { get; set; }
//        public DateTime? LastLogin { get; set; }
//        public bool IsActive { get; set; }
//        public bool TwoFactorEnabled { get; set; }
//        public bool EmailVerified { get; set; }
//        public DateTime CreatedAt { get; set; }
//    }

//    // For password change
//    public class ChangePasswordDto
//    {
//        [Required(ErrorMessage = "Current password is required")]
//        public string CurrentPassword { get; set; }

//        [Required(ErrorMessage = "New password is required")]
//        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
//        public string NewPassword { get; set; }

//        [Required(ErrorMessage = "Confirm password is required")]
//        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
//        public string ConfirmPassword { get; set; }
//    }

//    // For password reset request
//    public class ForgotPasswordDto
//    {
//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid email format")]
//        public string Email { get; set; }
//    }

//    // For password reset
//    public class ResetPasswordDto
//    {
//        [Required(ErrorMessage = "Token is required")]
//        public string Token { get; set; }

//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid email format")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "New password is required")]
//        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
//        public string NewPassword { get; set; }

//        [Required(ErrorMessage = "Confirm password is required")]
//        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
//        public string ConfirmPassword { get; set; }
//    }
//}


using My_Personal_Portfolio.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace My_Personal_Portfolio.DTOs
{
    // Add this to your existing UserDTOs.cs file
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        // For tracking (optional)
        public string IpAddress { get; set; }
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserResponseDto User { get; set; }

        public static AuthResponseDto SuccessResponse(string token, User user)
        {
            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                User = new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role,
                    LastLogin = user.LastLogin,
                    IsActive = user.IsActive,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    EmailVerified = user.EmailVerified,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        public static AuthResponseDto ErrorResponse(string message)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = message,
                Token = null,
                User = null
            };
        }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "New password must be at least 6 characters")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }

    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool EmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RegisterAdminDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Secret key is required")]
        [StringLength(50, ErrorMessage = "Invalid secret key")]
        public string SecretKey { get; set; } // Additional security to prevent unauthorized registration
    }
}