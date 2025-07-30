using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Services.IServices;

public interface ISmsService
{
    Task SendSmsAsync(string phoneNumber, string message);
    Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode);
}

