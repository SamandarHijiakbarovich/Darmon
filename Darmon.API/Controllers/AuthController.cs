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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userDto">User registration data</param>
        /// <returns>Authentication response with JWT tokens</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRequestDto userDto)
        {
            try
            {
                var authResponse = await _authService.RegisterAsync(userDto);
                return Ok(authResponse);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error registering user");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error registering user");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Internal server error" });
            }
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
            try
            {
                var authResponse = await _authService.LoginAsync(loginDto);
                return Ok(authResponse);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Authentication failed");
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Internal server error" });
            }
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
            try
            {
                await _authService.RequestPasswordResetAsync(request.Email);
                return Ok(new { Message = "Password reset email sent if account exists" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting password reset");
                return BadRequest(new { Error = ex.Message });
            }
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
            try
            {
                var result = await _authService.ResetPasswordAsync(resetDto);

                if (!result)
                {
                    return BadRequest(new { Error = "Invalid or expired token" });
                }

                return Ok(new { Message = "Password successfully reset" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password");
                return BadRequest(new { Error = ex.Message });
            }
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
            try
            {
                // You'll need to implement this method in your AuthService
                var authResponse = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(authResponse);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Token refresh failed");
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during token refresh");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "Internal server error" });
            }
        }
    }
}