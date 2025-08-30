using Darmon.Application.Interfaces;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Services.Auth;

public class BCryptPasswordHasher : IPasswordHasherService
{
    private readonly int _workFactor;
    public BCryptPasswordHasher(int workFactor)
    {
        _workFactor = workFactor;

    }
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password,workFactor: _workFactor);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

