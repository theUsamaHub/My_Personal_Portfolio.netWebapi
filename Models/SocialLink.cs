using System;
using System.ComponentModel.DataAnnotations;

namespace My_Personal_Portfolio.Models
{
    public class SocialLink
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string Platform { get; set; }

        [Required]
        [StringLength(500)]
        [Url]
        public string Url { get; set; }

        // Visual Representation
        [StringLength(50)]
        public string IconName { get; set; }

        [StringLength(50)]
        public string IconPack { get; set; } = "fontawesome";

        // Display Options
        [StringLength(50)]
        public string DisplayName { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public bool IsPrimary { get; set; } = false;

        public bool ShowInFooter { get; set; } = true;

        // Tracking
        public int ClickCount { get; set; } = 0;

        public DateTime? LastClicked { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}