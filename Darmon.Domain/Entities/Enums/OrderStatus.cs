using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities.Enums;

/// <summary>
///  buyurtma holatlari
/// </summary>
public enum OrderStatus
{
    [Description("Pending")]
    Pending,       // Yangi buyurtma, hali tasdiqlanmagan

    [Description("Confirmed")]
    Confirmed,     // Foydalanuvchi tomonidan tasdiqlangan

    [Description("Processing")]
    Processing,    // Buyurtma tayyorlanmoqda

    [Description("Shipped")]
    Shipped,       // Yetkazib berish jarayonida

    [Description("Delivered")]
    Delivered,     // Yetkazib berilgan

    [Description("Cancelled")]
    Cancelled,     // Bekor qilingan

    [Description("Returned")]
    Returned       // Qaytarilgan
}
