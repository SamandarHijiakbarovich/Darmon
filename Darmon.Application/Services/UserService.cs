using AutoMapper;
using Darmon.Application.DTOs;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Services.IServices;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger,
        IPasswordHasherService passwordHasher,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task<UserResponseDto> RegisterAsync(UserRequestDto userDto)
    {
        try
        {
            // Email unikalligini tekshirish
            if (await _userRepository.GetByEmailAsync(userDto.Email) != null)
                throw new ApplicationException("Bu email allaqachon ro'yxatdan o'tgan");

            // Parolni hash qilish
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
            user.CreatedAt = DateTime.UtcNow;

            // Foydalanuvchini yaratish
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Xush kelibsiz emailini jo'natish
            await _emailService.SendEmailAsync(
                user.Email,
                "D'Armon - Xush kelibsiz",
                $"<h1>Hurmatli {user.FirstName},</h1><p>Ro'yxatdan o'tganingiz uchun tashakkur!</p>");

            return _mapper.Map<UserResponseDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Foydalanuvchini ro'yxatga olishda xatolik");
            throw;
        }
    }

    public async Task<string> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new ApplicationException("Email yoki parol noto'g'ri");

        return _tokenService.GenerateToken(user);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task UpdateUserAsync(int userId, UserRequestDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        _mapper.Map(updateDto, user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        _userRepository.DeleteByIdAsync(userId);
        return await _userRepository.SaveChangesAsync() > 0;
    }

    public async Task RequestPasswordResetAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return;

        user.ResetToken = Guid.NewGuid().ToString();
        user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
        await _userRepository.SaveChangesAsync();

        var resetLink = $"https://darmon.uz/reset-password?token={user.ResetToken}";

        await _emailService.SendEmailAsync(
            email,
            "Parolni tiklash",
            $"Parolni tiklash uchun <a href='{resetLink}'>havolaga</a> o'ting");
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetDto)
    {
        var user = await _userRepository.GetByResetTokenAsync(resetDto.Token);

        if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
            return false;

        user.PasswordHash = _passwordHasher.HashPassword(resetDto.NewPassword);
        user.ResetToken = null;
        user.ResetTokenExpires = null;

        return await _userRepository.SaveChangesAsync() > 0;
    }
}
