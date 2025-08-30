using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.SettingModels;

public class ClickSettings
{
    /// <summary>Merchant ID (beriladi ro'yxatdan o'tishda)</summary>
    public int MerchantId { get; set; }

    /// <summary>Service ID (har bir xizmat uchun alohida)</summary>
    public int ServiceId { get; set; }

    /// <summary>Merchant User ID (Auth headerda ishlatiladi)</summary>
    public string MerchantUserId { get; set; } = string.Empty;

    /// <summary>Secret Key (maxfiy, sha1 digest uchun ishlatiladi)</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Click API bazaviy URL (https://api.click.uz/v2/merchant)</summary>
    public string BaseUrl { get; set; } = "https://api.click.uz/v2/merchant";
}
