using Microsoft.EntityFrameworkCore;
using My_Personal_Portfolio.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using My_Personal_Portfolio.Data;
using My_Personal_Portfolio.Services;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// ===== DATABASE CONFIGURATION =====
// Just uncomment the one you need!

// OPTION 1: SQL Server (for local development) - USE THIS NOW
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// OPTION 2: PostgreSQL (for production) - COMMENT THIS OUT FOR NOW
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Register Services
//builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication -1
var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.ASCII.GetBytes(jwtKey);

// JWT Authentication -2
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


// CORS - Allow only your frontend -3
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//Jwt Authentication -4
// IMPORTANT: Initialize admin user on startup
using (var scope = app.Services.CreateScope())
{
    //var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    //await authService.InitializeAdminUserAsync();
}


app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
