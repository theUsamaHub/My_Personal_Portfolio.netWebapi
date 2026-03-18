using System;
using System.ComponentModel.DataAnnotations;

namespace My_Personal_Portfolio.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string Role { get; set; } = "admin";

        public DateTime? LastLogin { get; set; }

        [StringLength(45)]
        public string LastLoginIp { get; set; }

        public int LoginCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // Security fields
        [StringLength(255)]
        public string TwoFactorSecret { get; set; }

        public bool TwoFactorEnabled { get; set; } = false;

        [StringLength(255)]
        public string PasswordResetToken { get; set; }

        public DateTime? PasswordResetExpires { get; set; }

        public bool EmailVerified { get; set; } = false;

        [StringLength(255)]
        public string EmailVerificationToken { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}