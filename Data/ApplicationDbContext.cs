using Microsoft.EntityFrameworkCore;
using My_Personal_Portfolio.Models;

namespace My_Personal_Portfolio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<SiteSettings> SiteSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Role);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Profile configuration (Singleton)
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.GalleryImages)
                    .HasColumnType("jsonb");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Ensure only one profile record
                entity.HasData(new Profile
                {
                    Id = 1,
                    FullName = "Usama Saleem",
                    Title = "Passionate Backend Developer",
                    ShortBio = "I build scalable backend systems",
                    LongBio = "Experienced in building APIs and backend architectures using .NET",
                    Email = "u641332@example.com",
                    Phone = "0000000000",
                    Location = "Pakistan",

                    ProfileImageUrl = "",
                    HeroImageUrl = "",
                    AvatarImageUrl = "",

                    GalleryImages = new string[] { },

                    ResumeUrl = "",

                    YearsExperience = 2,

                    AvailabilityStatus = "Available", // REQUIRED FIX
                    PreferredWorkType = "Remote",

                    TotalProjects = 0,

                    MetaDescription = "Backend developer portfolio",
                    MetaKeywords = "backend, .net, api",

                    CreatedAt = DateTime.UtcNow, // FIXED VALUE (important)
                    UpdatedAt = DateTime.UtcNow  // FIXED VALUE (important)
                });
            });

            // SocialLink configuration
            modelBuilder.Entity<SocialLink>(entity =>
            {
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.DisplayOrder);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // SiteSettings configuration (Singleton)
            modelBuilder.Entity<SiteSettings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Ensure only one settings record
                entity.HasData(new SiteSettings
                {
                    Id = 1,
                    SiteTitle = "Usama Saleem",

                    SiteDescription = "Personal portfolio website",
                    SiteKeywords = "portfolio, backend, dotnet",

                    PrimaryColor = "#3b82f6",
                    SecondaryColor = "#10b981",
                    DarkModeEnabled = true,

                    EnableBlog = false,
                    EnableComments = false,
                    EnableAnalytics = true,

                    IsMaintenanceMode = false,
                    MaintenanceMessage = "Site is under maintenance",

                    GoogleAnalyticsId = null,
                    GoogleSiteVerification = null,

                    UpdatedAt = new DateTime(2026, 1, 1)
                });
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    // Base entity for timestamp management
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}