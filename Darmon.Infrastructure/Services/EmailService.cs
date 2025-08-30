using Darmon.Infrastructure.Services.IServices;
using Darmon.Infrastructure.SettingModels;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace Darmon.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> settings)
    {
        _smtpSettings = settings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        message.Body = new TextPart("html") { Text = htmlContent };

        await SendEmailAsync(message);
    }

    public async Task SendEmailWithAttachmentsAsync(string toEmail, string subject, string htmlContent, byte[] attachment, string fileName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlContent
        };
        bodyBuilder.Attachments.Add(fileName, attachment);
        message.Body = bodyBuilder.ToMessageBody();

        await SendEmailAsync(message);
    }


    public async Task SendWelcomeEmailAsync(string email, string firstName)
    {
        var subject = "Xush kelibsiz!";
        var htmlContent = $@"
            <h1>Xush kelibsiz, {firstName}!</h1>
            <p>D'armon farmatsevtika tizimiga muvaffaqiyatli ro'yxatdan o'tdingiz.</p>
            <p>Sizning hisobingiz endi aktiv holatda.</p>";

        await SendEmailAsync(email, subject, htmlContent);
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        var subject = "Parolni tiklash";
        var resetLink = $"{_smtpSettings.BaseUrl}/reset-password?token={resetToken}";
        var htmlContent = $@"
            <h1>Parolni tiklash</h1>
            <p>Parolingizni tiklash uchun quyidagi havolaga bosing:</p>
            <a href='{resetLink}'>{resetLink}</a>
            <p>Agar siz bu so'rovni yubormagan bo'lsangiz, ushbu xabarga e'tibor bermang.</p>";

        await SendEmailAsync(email, subject, htmlContent);
    }


    private async Task SendEmailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
