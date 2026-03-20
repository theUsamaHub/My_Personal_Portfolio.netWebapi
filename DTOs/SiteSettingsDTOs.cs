using System;
using System.ComponentModel.DataAnnotations;

namespace My_Personal_Portfolio.DTOs
{
    public class SiteSettingsDto
    {
        [StringLength(200)]
        public string SiteTitle { get; set; }

        public string SiteDescription { get; set; }

        public string SiteKeywords { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
            ErrorMessage = "Invalid color format. Use hex color (e.g., #3b82f6)")]
        [StringLength(20)]
        public string PrimaryColor { get; set; }

        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
            ErrorMessage = "Invalid color format. Use hex color (e.g., #10b981)")]
        [StringLength(20)]
        public string SecondaryColor { get; set; }

        public bool? DarkModeEnabled { get; set; }

        public bool? EnableBlog { get; set; }

        public bool? EnableComments { get; set; }

        public bool? EnableAnalytics { get; set; }

        public bool? IsMaintenanceMode { get; set; }

        public string MaintenanceMessage { get; set; }

        [StringLength(50)]
        public string GoogleAnalyticsId { get; set; }

        [StringLength(100)]
        public string GoogleSiteVerification { get; set; }
    }

    public class SiteSettingsResponseDto
    {
        public int Id { get; set; }
        public string SiteTitle { get; set; }
        public string SiteDescription { get; set; }
        public string SiteKeywords { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public bool DarkModeEnabled { get; set; }
        public bool EnableBlog { get; set; }
        public bool EnableComments { get; set; }
        public bool EnableAnalytics { get; set; }
        public bool IsMaintenanceMode { get; set; }
        public string MaintenanceMessage { get; set; }
        public string GoogleAnalyticsId { get; set; }
        public string GoogleSiteVerification { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}