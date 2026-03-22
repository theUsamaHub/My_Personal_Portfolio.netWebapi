# My Personal Portfolio API

A modern, scalable ASP.NET Core Web API backend for a personal portfolio website. This project provides a robust foundation for managing portfolio content, user authentication, and site administration.

## 🚀 Features
 
- **User Management**: Secure authentication with JWT tokens, role-based access control
- **Profile Management**: Dynamic personal profile with bio, contact info, and media
- **Social Links**: Manage social media links with click tracking
- **Site Settings**: Customizable site configuration, themes, and SEO settings
- **Database Support**: SQL Server for development, PostgreSQL for production
- **Modern Architecture**: Clean architecture with Entity Framework Core
- **Security**: Password hashing with bcrypt, JWT authentication, two-factor support

## 🛠️ Tech Stack

- **.NET 10.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Development database
- **PostgreSQL** - Production database support
- **JWT Authentication** - Token-based authentication
- **bcrypt.Net-Next** - Password hashing
- **OpenAPI/Swagger** - API documentation

## 📁 Project Structure

```
My_Personal_Portfolio/
├── Controllers/                 # API Controllers
│   └── WeatherForecastController.cs
├── Data/                       # Database context and configurations
│   └── ApplicationDbContext.cs
├── DTOs/                       # Data Transfer Objects
│   ├── ProfileDTOs.cs
│   ├── ResponseDTOs.cs
│   ├── SiteSettingsDTOs.cs
│   ├── SocialLinkDTOs.cs
│   └── UserDTOs.cs
├── Models/                     # Entity models
│   ├── User.cs
│   ├── Profile.cs
│   ├── SiteSettings.cs
│   └── SocialLink.cs
├── Migrations/                 # Database migrations
├── Services/                   # Business logic services
├── wwwroot/                    # Static files
├── Program.cs                  # Application entry point
├── appsettings.json            # Configuration settings
└── My_Personal_Portfolio.csproj # Project file
```

## 🗄️ Database Schema

### Core Entities

#### User
- Authentication and authorization
- Profile management with security features
- Login tracking and activity monitoring
- Two-factor authentication support

#### Profile
- Personal information (name, title, bio)
- Contact details and location
- Media assets (profile images, gallery)
- Professional information (experience, availability)
- SEO metadata

#### SocialLink
- Social media platform links
- Display ordering and visibility controls
- Click tracking and analytics
- Icon customization

#### SiteSettings
- Site-wide configuration
- Theme customization
- Feature toggles
- SEO and analytics settings

## 🔧 Installation & Setup

### Prerequisites
- .NET 10.0 SDK
- SQL Server (for development) or PostgreSQL (for production)
- Visual Studio 2022 or VS Code

### 1. Clone the Repository
```bash
git clone <repository-url>
cd My_Personal_Portfolio
```

### 2. Configure Database Connection

#### For Development (SQL Server)
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MyPersonalPortfolioDb;Trusted_Connection=True;TrustServerCertificate=true"
  }
}
```

#### For Production (PostgreSQL)
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-production-server;Database=MyPersonalPortfolioDb;Username=youruser;Password=yourpassword;SSL Mode=Require"
  }
}
```

### 3. Configure Database Provider

#### SQL Server (Development)
Ensure `Program.cs` has:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

#### PostgreSQL (Production)
Update `Program.cs` to:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 4. Apply Database Migrations
```bash
dotnet ef database update
```

### 5. Run the Application
```bash
dotnet run
```

The API will be available at `https://localhost:7123` or `http://localhost:5123`

## 🔐 Authentication

### JWT Configuration
The application uses JWT tokens for authentication. Configure in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "your-super-secret-key-at-least-32-characters-long",
    "Issuer": "MyPersonalPortfolio",
    "Audience": "MyPersonalPortfolioClient"
  }
}
```

### Default Admin User
The application seeds with a default admin profile:
- **Email**: u641332@example.com
- **Name**: Usama Saleem
- **Title**: Passionate Backend Developer

## 📚 API Endpoints

### Weather Forecast
```
GET /weatherforecast/
```
Returns sample weather forecast data (demo endpoint).

### User Management
```
POST /api/auth/login          # User login
POST /api/auth/register       # User registration
POST /api/auth/refresh        # Refresh JWT token
GET  /api/users/profile       # Get user profile
PUT  /api/users/profile       # Update user profile
```

### Profile Management
```
GET    /api/profile           # Get portfolio profile
PUT    /api/profile           # Update portfolio profile
POST   /api/profile/image     # Upload profile image
DELETE /api/profile/image     # Delete profile image
```

### Social Links
```
GET    /api/social-links      # Get all social links
POST   /api/social-links      # Create new social link
PUT    /api/social-links/{id} # Update social link
DELETE /api/social-links/{id} # Delete social link
PATCH  /api/social-links/{id}/toggle # Toggle visibility
```

### Site Settings
```
GET /api/settings             # Get site settings
PUT /api/settings             # Update site settings
GET /api/settings/theme       # Get theme configuration
PUT /api/settings/theme       # Update theme configuration
```

## 🏗️ Architecture Patterns

### Repository Pattern
The application follows the repository pattern for data access through Entity Framework Core.

### DTO Pattern
Data Transfer Objects are used to separate API models from database entities, providing better security and flexibility.

### Singleton Pattern
Profile and SiteSettings use the singleton pattern to ensure only one record exists in the database.

### Base Entity
All entities inherit from `BaseEntity` for automatic timestamp management.

## 🔒 Security Features

- **Password Hashing**: Uses bcrypt for secure password storage
- **JWT Authentication**: Stateless authentication with configurable expiration
- **Input Validation**: Comprehensive data annotations on all models
- **CORS Configuration**: Configurable cross-origin resource sharing
- **HTTPS Enforcement**: Automatic HTTPS redirection in production

## 🚀 Deployment

### Environment Variables
Set the following environment variables for production:
- `ASPNETCORE_ENVIRONMENT=Production`
- `ConnectionStrings__DefaultConnection`
- `Jwt__Key`
- `Jwt__Issuer`
- `Jwt__Audience`

### Docker Support
Create a `Dockerfile` for containerized deployment:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["My_Personal_Portfolio.csproj", "./"]
RUN dotnet restore "./My_Personal_Portfolio.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "My_Personal_Portfolio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "My_Personal_Portfolio.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "My_Personal_Portfolio.dll"]
```

## 🧪 Testing

### Unit Tests
```bash
dotnet test
```

### Integration Tests
```bash
dotnet test --filter Category=Integration
```

## 📝 API Documentation

Once running, visit:
- **Swagger UI**: `https://localhost:7123/swagger`
- **OpenAPI JSON**: `https://localhost:7123/swagger/v1/swagger.json`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

- **Developer**: Usama Saleem
- **Email**: u641332@example.com
- **Location**: Pakistan

## 🔄 Version History

- **v1.0.0** - Initial release with core portfolio management features
  - User authentication with JWT
  - Profile management
  - Social links management
  - Site settings configuration
  - Database migrations

## 🛠️ Future Enhancements

- [ ] Blog management system
- [ ] Project portfolio section
- [ ] Contact form with email integration
- [ ] File upload service for media assets
- [ ] Analytics dashboard
- [ ] Multi-language support
- [ ] Role-based permissions system
- [ ] API rate limiting
- [ ] Caching layer with Redis
- [ ] Background job processing

---

**Built with ❤️ using .NET 10.0**
