using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Personal_Portfolio.Models
{
    public class Profile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } = 1; // Singleton pattern

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        public string ShortBio { get; set; }

        public string LongBio { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        // Images
        [StringLength(500)]
        public string ProfileImageUrl { get; set; }

        [StringLength(500)]
        public string HeroImageUrl { get; set; }

        [StringLength(500)]
        public string AvatarImageUrl { get; set; }

        [Column(TypeName = "jsonb")]
        public string[] GalleryImages { get; set; } // Stored as JSON array

        // Resume
        [StringLength(500)]
        public string ResumeUrl { get; set; }

        // Professional details
        public int? YearsExperience { get; set; }

        [StringLength(50)]
        public string AvailabilityStatus { get; set; }    //"Available", "Busy", "Open to offers"

        [StringLength(50)]
        public string PreferredWorkType { get; set; }    //"Remote", "Hybrid", "On-site"


        public int TotalProjects { get; set; } // This will be calculated

        // SEO
        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}