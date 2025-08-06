using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Darmon.Infrastructure.Data;
using Darmon.Application.Mappings;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Repositories;
using Darmon.Application.Interfaces;
using Darmon.Application.Services;
using Darmon.Infrastructure.Services.Auth;
using Darmon.Infrastructure.Services.IServices;
using Darmon.Infrastructure.SettingModels;
using Darmon.Application.DTOs.Configurations;
using Darmon.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// 1. CONFIGURATION SETUP
// =============================================
var configuration = builder.Configuration;

// =============================================
// 2. SERVICE REGISTRATION
// =============================================

// 2.1. CORE SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Darmon API", Version = "v1" });
});

// 2.2. DATABASE CONFIGURATION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DarmonBaza")));

// 2.3. AUTOMAPPER CONFIGURATION
builder.Services.AddAutoMapper(typeof(MappingProfil));

// 2.4. REPOSITORY LAYER
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// 2.5. APPLICATION SERVICES
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();

// 2.6. AUTHENTICATION SERVICES
builder.Services.AddSingleton<IPasswordHasherService>(
    _ => new BCryptPasswordHasher(workFactor: 11));
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// 2.7. EXTERNAL SERVICES CONFIGURATION
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
builder.Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
builder.Services.Configure<SmsSettings>(configuration.GetSection("SmsSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<ISmsService, SmsService>();

// =============================================
// 3. APP BUILDING
// =============================================
var app = builder.Build();

// =============================================
// 4. MIDDLEWARE PIPELINE
// =============================================

// 4.1. DEVELOPMENT CONFIGURATION
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Darmon API V1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at root
    });
}

// 4.2. SECURITY MIDDLEWARE
app.UseHttpsRedirection();

// 4.3. ROUTING MIDDLEWARE
app.UseRouting();

// 4.4. AUTHENTICATION & AUTHORIZATION
app.UseAuthentication();
app.UseAuthorization();

// 4.5. ENDPOINTS
app.MapControllers();

// =============================================
// 5. APPLICATION START
// =============================================
app.Run();