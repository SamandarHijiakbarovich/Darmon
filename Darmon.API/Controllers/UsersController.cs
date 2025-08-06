using Darmon.Application.DTOs;
using Darmon.Application.DTOs.User;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darmon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUserService userService,
        ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user with ID {id}");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Update user information
    /// </summary>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UserRequestDto updateDto)
    {
        try
        {
            await _userService.UpdateUserAsync(id, updateDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user with ID {id}");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            return Ok(new { Success = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user with ID {id}");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Change user role (Admin only)
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/role")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeRole(int id, [FromBody] UserRole newRole)
    {
        try
        {
            var updatedUser = await _userService.ChangeUserRoleAsync(id, newRole);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error changing role for user with ID {id}");
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}