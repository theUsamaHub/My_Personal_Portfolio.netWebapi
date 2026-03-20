using System;
using System.ComponentModel.DataAnnotations;

namespace My_Personal_Portfolio.DTOs
{
    public class SocialLinkCreateDto
    {
        [Required(ErrorMessage = "Platform is required")]
        [StringLength(50, ErrorMessage = "Platform cannot exceed 50 characters")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        public string Url { get; set; }

        [StringLength(50)]
        public string IconName { get; set; }

        [StringLength(50)]
        public string IconPack { get; set; } = "fontawesome";

        [StringLength(50)]
        public string DisplayName { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public bool IsPrimary { get; set; } = false;

        public bool ShowInFooter { get; set; } = true;
    }

    public class SocialLinkUpdateDto
    {
        [StringLength(50)]
        public string Platform { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500)]
        public string Url { get; set; }

        [StringLength(50)]
        public string IconName { get; set; }

        [StringLength(50)]
        public string IconPack { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsPrimary { get; set; }

        public bool? ShowInFooter { get; set; }
    }

    public class SocialLinkResponseDto
    {
        public Guid Id { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public string IconName { get; set; }
        public string IconPack { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrimary { get; set; }
        public bool ShowInFooter { get; set; }
        public int ClickCount { get; set; }
        public DateTime? LastClicked { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SocialLinkClickDto
    {
        public Guid Id { get; set; }
    }
}