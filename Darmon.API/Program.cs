using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Swashbuckle.AspNetCore.SwaggerGen;
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
using Darmon.API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicyName = "DarmonCorsPolicy";

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true) // bu muhim
    .AddEnvironmentVariables();

// =============================================
// 1. CONFIGURATION SETUP
// =============================================
var configuration = builder.Configuration;
var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

// JWT sozlamalari to'g'ri berilganini ilova ishga tushishidayoq tekshiramiz.
// Bu sozlanmagan/bo'sh maxfiy kalit bilan xatolarga tushishning oldini oladi.
if (jwtSettings is null
    || string.IsNullOrWhiteSpace(jwtSettings.Secret)
    || string.IsNullOrWhiteSpace(jwtSettings.Issuer)
    || string.IsNullOrWhiteSpace(jwtSettings.Audience))
{
    throw new InvalidOperationException(
        "JwtSettings noto'g'ri sozlangan. 'Secret', 'Issuer' va 'Audience' " +
        "qiymatlari appsettings yoki muhit o'zgaruvchilarida ko'rsatilishi shart.");
}

if (Encoding.UTF8.GetByteCount(jwtSettings.Secret) < 32)
{
    throw new InvalidOperationException(
        "JwtSettings:Secret kamida 32 bayt (256 bit) uzunlikda bo'lishi kerak.");
}

// =============================================
// 2. SERVICE REGISTRATION
// =============================================

// 2.1. CORE SERVICES
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Darmon API",
        Version = "v1",
        Description = "Dori-darmon yetkazib berish xizmati uchun RESTful API."
    });
    c.UseInlineDefinitionsForEnums();
    c.SchemaGeneratorOptions = new SchemaGeneratorOptions
    {
        UseAllOfToExtendReferenceSchemas = false
    };

    // 🔐 Swagger uchun JWT qo‘llab-quvvatlash
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 2.2. DATABASE CONFIGURATION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DarmonBaza")));

// 2.3. AUTOMAPPER CONFIGURATION
builder.Services.AddAutoMapper(typeof(MappingProfil));

// 2.4. REPOSITORY LAYER
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
builder.Services.AddScoped<ISellerWalletRepository, SellerWalletRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// 2.5. APPLICATION SERVICES
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IClickPaymentService, ClickPaymentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

// 2.6. AUTHENTICATION SERVICES
builder.Services.AddSingleton<IPasswordHasherService>(
    _ => new BCryptPasswordHasher(workFactor: 11));
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// 2.7. JWT AUTHENTICATION CONFIGURATION
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Development'da HTTPS metadata talab qilinmaydi (lokal test uchun),
    // ishlab chiqarishda esa majburiy.
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        // Tokenning amal qilish muddatini aniqroq tekshirish uchun soat farqini nolga tushiramiz.
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// 2.8. EXTERNAL SERVICES CONFIGURATION
builder.Services.Configure<ClickSettings>(configuration.GetSection("ClickSettings"));
builder.Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
builder.Services.Configure<SmsSettings>(configuration.GetSection("SmsSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<ISmsService, SmsService>();

// 2.9. CORS
var corsOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        if (corsOrigins is { Length: > 0 })
        {
            policy.WithOrigins(corsOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            // Aniq manzillar sozlanmagan bo'lsa (masalan, lokal ishlab chiqish),
            // barcha manzillarga ruxsat beramiz (credentials'siz).
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

// 2.10. RATE LIMITING (abuse va DoS'ga qarshi bazaviy himoya)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
    });
});

// 2.11. HEALTH CHECKS
builder.Services.AddHealthChecks();

// =============================================
// 3. APP BUILDING
// =============================================
var app = builder.Build();

// =============================================
// 4. MIDDLEWARE PIPELINE
// =============================================

// 4.0. GLOBAL EXCEPTION HANDLING (eng tashqi qatlam)
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

// 4.1. DEVELOPMENT CONFIGURATION
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Darmon API V1");
        c.RoutePrefix = string.Empty;
    });
}

// 4.2. SECURITY MIDDLEWARE
app.UseHttpsRedirection();

// 4.3. ROUTING & CORS
app.UseRouting();
app.UseCors(CorsPolicyName);

// 4.4. RATE LIMITING
app.UseRateLimiter();

// 4.5. AUTHENTICATION & AUTHORIZATION
app.UseAuthentication();
app.UseAuthorization();

// 4.6. ENDPOINTS
app.MapControllers();
app.MapHealthChecks("/health");

// =============================================
// 5. APPLICATION START
// =============================================
app.Run();

// Integratsion testlar uchun Program sinfini ochiq qilamiz.
public partial class Program { }
