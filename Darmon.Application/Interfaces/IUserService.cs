using Darmon.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> RegisterAsync(UserRequestDto userDto);
    Task<string> LoginAsync(UserLoginDto loginDto);
    Task<UserResponseDto> GetUserByIdAsync(int userId);
    Task UpdateUserAsync(int userId, UserRequestDto updateDto);
    Task<bool> DeleteUserAsync(int userId);
    Task RequestPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetDto);
}
