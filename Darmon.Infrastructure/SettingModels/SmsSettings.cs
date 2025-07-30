using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.SettingModels;

public class SmsSettings
{
    public string ApiUrl { get; set; }
    public string OtpVerifyUrl { get; set; }
    public string ApiKey { get; set; }
}
