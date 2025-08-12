using AutoMapper;
using Darmon.Application.DTOs;
using Darmon.Application.DTOs.AuthResponse;
using Darmon.Application.DTOs.User;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasherService passwordHasher,
        ITokenService tokenService,
        IEmailService emailService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _emailService = emailService;
        _mapper = mapper;
    }



    public async Task<AuthResponse> RegisterAsync(UserRequestDto userDto)
    {
        if (await _userRepository.GetByEmailAsync(userDto.Email) != null)
            throw new ApplicationException("Bu email allaqachon ro'yxatdan o'tgan");

        var user = _mapper.Map<User>(userDto);
        user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.Role=userDto.Role;

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        await _emailService.SendWelcomeEmailAsync(user.Email, user.FirstName);

        return new AuthResponse(
     accessToken: _tokenService.GenerateAccessToken(user),
     expiresAt: _tokenService.GetAccessTokenExpiration(),
     refreshToken: _tokenService.GenerateRefreshToken(),
     user: _mapper.Map<UserResponseDto>(user)
 );
    }


    public async Task<AuthResponse> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new ApplicationException("Email yoki parol noto'g'ri");

        return new AuthResponse(
    accessToken: _tokenService.GenerateAccessToken(user),
    expiresAt: _tokenService.GetAccessTokenExpiration(),
    refreshToken: _tokenService.GenerateRefreshToken(),
    user: _mapper.Map<UserResponseDto>(user)
);
    }


    public async Task RequestPasswordResetAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return; // Yoki throw new ApplicationException("Foydalanuvchi topilmadi");

        user.ResetToken = Guid.NewGuid().ToString();
        user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);

		await _userRepository.UpdateAsync(user);
		await _userRepository.SaveChangesAsync();

		await _emailService.SendPasswordResetEmailAsync(email, user.ResetToken);
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


    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        // 1. Refresh tokenni tekshirish
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshTokenExpires < DateTime.UtcNow)
            throw new ApplicationException("Invalid or expired refresh token");

        // 2. Yangi tokenlar generatsiya qilish
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // 3. Yangi refresh tokenni bazaga saqlash
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpires = _tokenService.GetRefreshTokenExpiration();
        await _userRepository.SaveChangesAsync();

        // 4. Yangi tokenlarni qaytarish
        return new AuthResponse(
            newAccessToken,
            _tokenService.GetAccessTokenExpiration(),
            newRefreshToken,
            _mapper.Map<UserResponseDto>(user)
        );
    }

}
