using Darmon.Application.DTOs.User;
using Darmon.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Darmon.Application.DTOs.AuthResponse;

namespace Darmon.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(UserRequestDto userDto);
    Task<AuthResponse> LoginAsync(UserLoginDto loginDto);
    Task RequestPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetDto);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
}
