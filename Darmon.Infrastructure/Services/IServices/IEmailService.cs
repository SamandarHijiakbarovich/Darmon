using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Services.IServices;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlContent);
    Task SendEmailWithAttachmentsAsync(string toEmail, string subject, string htmlContent, byte[] attachment, string fileName);
    Task SendWelcomeEmailAsync(string email, string firstName); // Yangi method
    Task SendPasswordResetEmailAsync(string email, string resetToken); // Yangi method
}
