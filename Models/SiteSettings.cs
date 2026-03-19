using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Personal_Portfolio.Models
{
    public class SiteSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } = 1; // Singleton pattern

        // Site Info
        [StringLength(200)]
        public string SiteTitle { get; set; } = "My Portfolio";

        public string SiteDescription { get; set; }

        public string SiteKeywords { get; set; }

        // Theme
        [StringLength(20)]
        public string PrimaryColor { get; set; } = "#3b82f6";

        [StringLength(20)]
        public string SecondaryColor { get; set; } = "#10b981";

        public bool DarkModeEnabled { get; set; } = true;

        // Features
        public bool EnableBlog { get; set; } = false;

        public bool EnableComments { get; set; } = false;

        public bool EnableAnalytics { get; set; } = true;

        // Maintenance
        public bool IsMaintenanceMode { get; set; } = false;

        public string MaintenanceMessage { get; set; }

        // SEO
        [StringLength(50)]
        public string? GoogleAnalyticsId { get; set; }

        [StringLength(100)]
        public string? GoogleSiteVerification { get; set; }

        // Timestamp
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}