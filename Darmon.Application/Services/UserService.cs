using AutoMapper;
using Darmon.Application.DTOs;
using Darmon.Application.DTOs.AuthResponse;
using Darmon.Application.DTOs.User;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;
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

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<UserResponseDto> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> UpdateUserAsync(int userId, UserRequestDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        _mapper.Map(updateDto, user);
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        await _userRepository.DeleteByIdAsync(userId);
        return await _userRepository.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto> ChangeUserRoleAsync(int userId, UserRole newRole)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        user.Role = newRole;
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserResponseDto>(user);
    }


}
