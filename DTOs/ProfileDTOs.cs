using System;
using System.ComponentModel.DataAnnotations;

namespace My_Personal_Portfolio.DTOs
{
    public class ProfileDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        public string ShortBio { get; set; }

        public string LongBio { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500)]
        public string ProfileImageUrl { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500)]
        public string HeroImageUrl { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500)]
        public string AvatarImageUrl { get; set; }

        public string[] GalleryImages { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500)]
        public string ResumeUrl { get; set; }

        [Range(0, 50, ErrorMessage = "Years experience must be between 0 and 50")]
        public int? YearsExperience { get; set; }

        [StringLength(50)]
        public string AvailabilityStatus { get; set; }

        [StringLength(50)]
        public string PreferredWorkType { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }
    }

    public class ProfileResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string ShortBio { get; set; }
        public string LongBio { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string ProfileImageUrl { get; set; }
        public string HeroImageUrl { get; set; }
        public string AvatarImageUrl { get; set; }
        public string[] GalleryImages { get; set; }
        public string ResumeUrl { get; set; }
        public int? YearsExperience { get; set; }
        public string AvailabilityStatus { get; set; }
        public string PreferredWorkType { get; set; }
        public int TotalProjects { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}