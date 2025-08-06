using Darmon.Application.DTOs;
using Darmon.Application.DTOs.AuthResponse;
using Darmon.Application.DTOs.PagedDto;
using Darmon.Application.DTOs.User;
using Darmon.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetUserByIdAsync(int userId);
    Task<UserResponseDto> UpdateUserAsync(int userId, UserRequestDto updateDto);
    Task<bool> DeleteUserAsync(int userId);
    Task<PagedResult<UserResponseDto>> GetAllUsersAsync(PaginationParams pagination);
    Task<UserResponseDto> ChangeUserRoleAsync(int userId, UserRole newRole);

}
