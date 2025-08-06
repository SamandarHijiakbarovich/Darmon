using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Interfaces;

public interface IUserRepository:IRepository<User>
{
    Task<User> GetByResetTokenAsync(string resetToken);
    Task<User> GetByEmailAsync(string email);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
}
