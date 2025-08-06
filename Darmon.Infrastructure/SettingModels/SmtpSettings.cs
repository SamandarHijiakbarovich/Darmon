using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.SettingModels;

public class SmtpSettings
{
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string BaseUrl { get; set; } // Base URL for the application, used in email links
}
