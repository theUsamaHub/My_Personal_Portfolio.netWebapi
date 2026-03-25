using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi; // FIXED
using My_Personal_Portfolio.Data;
using My_Personal_Portfolio.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger (REPLACE OpenAPI)
builder.Services.AddEndpointsApiExplorer();
// Configure Swagger with JWT support
builder.Services.AddSwaggerGen();

// Database
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Database Configuration (with optional .env override)
var connectionString = Env.GetString("DB_CONNECTION_STRING") ??
    builder.Configuration.GetConnectionString("DefaultConnection");
// Services
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication - Read from .env or appsettings
var jwtKey = Env.GetString("JWT_KEY") ?? builder.Configuration["Jwt:Key"];
var jwtIssuer = Env.GetString("JWT_ISSUER") ?? builder.Configuration["Jwt:Issuer"];
var jwtAudience = Env.GetString("JWT_AUDIENCE") ?? builder.Configuration["Jwt:Audience"];

var key = Encoding.ASCII.GetBytes(jwtKey);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


// Test route (optional but useful)
//app.MapGet("/", () => "API is running");

// Controllers
app.MapControllers();

// Initialize admin user from .env
using (var scope = app.Services.CreateScope())
{
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    await authService.InitializeAdminUserAsync();
}

app.Run();  