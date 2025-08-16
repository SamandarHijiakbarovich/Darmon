using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Domain.Entities.Enums;

public enum PaymentProvider
{
    Unknown = 0,         // Default fallback
    Click = 1,           // https://click.uz
    Payme = 2,           // https://payme.uz
    UzumBank = 3,        // Uzum Bank to‘lov tizimi
    Apelsin = 4,         // Apelsin to‘lov tizimi
    Stripe = 5,          // Stripe (international)
    PayPal = 6,          // PayPal (international)
    Cash = 7,            // Naqd to‘lov
    Card = 8             // Mahalliy karta orqali to‘lov
}
