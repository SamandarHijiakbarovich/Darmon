using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using Darmon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetByResetTokenAsync(string resetToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.ResetToken == resetToken &&
                    x.ResetTokenExpires > DateTime.UtcNow);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.RefreshToken == refreshToken &&
                    x.RefreshTokenExpires > DateTime.UtcNow);
        }


        
    }
}