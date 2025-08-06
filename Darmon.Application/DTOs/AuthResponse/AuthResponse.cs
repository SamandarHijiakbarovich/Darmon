using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.AuthResponse;


public class AuthResponse
{
    public string TokenType { get; } = "Bearer";
    public string AccessToken { get; }
    public DateTime ExpiresAt { get; }
    public string RefreshToken { get; }
    public UserResponseDto User { get; }

    public AuthResponse(string accessToken, DateTime expiresAt, string refreshToken, UserResponseDto user)
    {
        AccessToken = accessToken;
        ExpiresAt = expiresAt;
        RefreshToken = refreshToken;
        User = user;
    }
}
