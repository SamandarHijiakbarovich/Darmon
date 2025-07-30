using Microsoft.EntityFrameworkCore;
using Darmon.Infrastructure.Data;
using Darmon.Application.Mappings;
using Darmon.Domain.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Infrastructure.Repositories;
using Darmon.Infrastructure.SettingModels;
using Darmon.Infrastructure.Services.IServices;
using Darmon.Infrastructure.Services;
using Darmon.Application.Interfaces;
using Darmon.Application.Services;
using Microsoft.AspNetCore.Identity; // AppDbContext joylashgan namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // API Controllerlar uchun

// Program.cs ichida
builder.Services.AddAutoMapper(typeof(MappingProfil)); // MappingProfile - sizning mapping klassingiz

// PostgreSQL uchun DbContext qo'shish
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DarmonBaza")));

builder.Services.AddScoped<IRepository<User>, UserRepository>();

//EmailService and SmsService
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// SMS service
builder.Services.Configure<SmsSettings>(builder.Configuration.GetSection("SmsSettings"));
builder.Services.AddHttpClient<ISmsService, SmsService>();


builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
//builder.Services.AddScoped<ITokenService, JwtTokenService>();
// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();