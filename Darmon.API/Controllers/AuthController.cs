using Darmon.Application.DTOs;
using Darmon.Application.DTOs.AuthResponse;
using Darmon.Application.DTOs.User;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;

namespace Darmon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userDto">User registration data</param>
        /// <returns>Authentication response with JWT tokens</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] UserRequestDto userDto)
        {
            var authResponse = await _authService.RegisterAsync(userDto);
            return Ok(authResponse);
        }

        /// <summary>
        /// Authenticate user and return JWT tokens
        /// </summary>
        /// <param name="loginDto">User credentials</param>
        /// <returns>Authentication response with JWT tokens</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var authResponse = await _authService.LoginAsync(loginDto);
            return Ok(authResponse);
        }

        /// <summary>
        /// Request password reset
        /// </summary>
        /// <param name="request">Email address for password reset</param>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _authService.RequestPasswordResetAsync(request.Email);
            return Ok(new { Message = "Password reset email sent if account exists" });
        }

        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="resetDto">Password reset data</param>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
        {
            var result = await _authService.ResetPasswordAsync(resetDto);

            if (!result)
            {
                return BadRequest(new { Error = "Invalid or expired token" });
            }

            return Ok(new { Message = "Password successfully reset" });
        }

        /// <summary>
        /// Refresh JWT tokens
        /// </summary>
        /// <param name="refreshTokenDto">Refresh token data</param>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var authResponse = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            return Ok(authResponse);
        }
    }
}
