    # 🌿 Darmon – Dori Yetkazib Berish Xizmati
 
**Darmon** — bu dorixonalardan foydalanuvchilarga tez, qulay va ishonchli tarzda dori-darmon yetkazib berishni maqsad qilgan veb-ilova. Ushbu loyiha O‘zbekistonda tibbiyot xizmatlarini raqamlashtirishga hissa qo‘shadi.

---

## 📌 Loyihaning Maqsadi

Foydalanuvchilarga atrofdagi dorixonalardagi mavjud dori vositalarini topish, narxlarni solishtirish, va ularni onlayn tarzda buyurtma berish orqali vaqt va kuchni tejash imkonini yaratish. Shu bilan birga, dorixonalarga o‘z mahsulotlarini kengroq auditoriyaga taklif qilish, savdo hajmini oshirish va xizmat sifatini yaxshilash imkonini beradi.

---

## 🚀 Asosiy Funksiyalar
## Tizimning Asosiy Funksiyalari:

1. **Dorilar Katalogi**:
   - Dorilarni kategoriyalar bo'yicha qidirish
   - Dori tafsilotlarini ko'rish
   - Dori mavjudligini tekshirish

2. **Buyurtma Boshqaruvi**:
   - Savat funksiyasi
   - Buyurtma yaratish
   - Buyurtma holatini kuzatish
   - Buyurtma bekor qilish

3. **Yetkazib Berish**:
   - Yetkazib berish manzilini belgilash
   - Yetkazib berish vaqtini tanlash
   - Yetkazib berish holatini kuzatish

4. **Foydalanuvchi Boshqaruvi**:
   - Ro'yxatdan o'tish/kirish
   - Profilni boshqarish
   - Buyurtmalar tarixi

## Qo'shimcha Tavsiyalar:

1. **Payment Gateway** integrasiyasi (Payme, Click kabi)
2. **Telegram Bot** integratsiyasi bildirishnomalar uchun
3. **GIS Xarita** integratsiyasi yetkazib berish uchun
4. **Mobile App** uchun API endpointlarni alohida guruhlash

Loyiha uchun kerak bo'ladigan asosiy package'lar:
- MediatR (CQRS patterni uchun)
- Entity Framework Core (ORM)
- AutoMapper (Mappinglar uchun)
- FluentValidation (Validation uchun)
- JWT (Autentifikatsiya uchun)
                                                                                              #
#### ###### ###################################################################################
### 👤 Foydalanuvchi Uchun:
- 🔍 Dorilarni nomi bo‘yicha qidirish
- 📋 Dorining narxi, ishlab chiqaruvchisi va qo‘llanilishi haqida ma’lumot olish
- 📍 Joylashuv asosida yaqin dorixonalarni ko‘rish
- 🛒 Onlayn buyurtma berish va to‘lov qilish
- 📦 Buyurtma yetkazib berilishi bo‘yicha real vaqt monitoring
- 🔐 Ro‘yxatdan o‘tish, login, shaxsiy kabinet

### 🏪 Dorixona Egasi Uchun:
- ➕ Dorilar ro‘yxatini boshqarish (qo‘shish, tahrirlash, o‘chirish)
- 📦 Zaxiradagi mahsulotlarni kuzatish
- 📈 Buyurtmalar statistikasini ko‘rish
- 🧾 Foydalanuvchi so‘rovlarini ko‘rib chiqish va bajarish


## 🧩 Modullar (rejalashtirilgan)

### 1. Foydalanuvchilar (Users)
- Ro‘yxatdan o‘tish / Kirish (JWT)
- Rollar (mijoz, dorixona, admin)

### 2. Dorixona (Pharmacy)
- Dorixona profili
- Joylashuv

### 3. Dori vositalari (Products)
- Nomi, dozasi, narxi, yaroqlilik muddati
- Dorixona omboriga biriktirilgan

### 4. Buyurtmalar (Orders)
- Dori tanlash, buyurtma berish
- Holati: Yangi, Yetkazilmoqda, Yetkazildi

### 5. Yetkazib beruvchilar (Couriers)
- Foydalanuvchi profilingiz orqali ro‘yxatdan o‘tadi
- Buyurtmalarni qabul qilish

### 6. To‘lov (Payments)
- Naqd / online (rejalashtirilmoqda)
---

## 🧱 Arxitektura

Loyiha **Clean Architecture** asosida ishlab chiqilgan:
<img width="356" height="303" alt="image" src="https://github.com/user-attachments/assets/aaf8377c-2baa-4d75-8bfd-6695d9982ed1" />




Darmon.sln
 ├── Darmon.Api
 ├── Darmon.Application
 ├── Darmon.Domain
 └── Darmon.Infrastructure


---

## ⚙️ Texnologiyalar

| Texnologiya       | Izoh                                 |
|-------------------|---------------------------------------|
| .NET 8            | Backend platformasi                   |
| C#                | Asosiy dasturlash tili                |
| ASP.NET Core Web API | RESTful API yaratish uchun         |
| Blazor (rejalashtirilgan) | Foydalanuvchi interfeysi (UI) |
| PostgreSQL / SQL Server | Ma’lumotlar bazasi              |
| Entity Framework Core | ORM vositasi                      |
| JWT               | Autentifikatsiya va Avtorizatsiya     |
| Mapster / AutoMapper | DTO va Model mapping uchun         |
| Swagger / Swashbuckle | API dokumentatsiyasi             |

---

## 🛠 O‘rnatish va Ishga tushurish

1. Repozitoriyani klonlash:
   ```bash
   git clone https://github.com/yourusername/Darmon.git

   
######

🔒 Autentifikatsiya
Ro'yxatdan o'tgan kuchga JWT token oladi

Har bir so'rovda Authorization: Bearer <token>yuboriladi

samaralilar roli (Admin, Dorixona, Mijoz) asosida access boshqariladi

######  ###### ###### #######  #####

📌 Rejalashtirilgan Qismlar
 📱 Mobil versiyasi (Flutter yoki React Native)

 🗺 Geolokatsiyaga dorixona tanlash

 🔔 Bildirishnoma (notifikatsiya) tizimi

 📦 Yetkazib beruvchilar moduli

 👨‍⚕️ Retseptni yuklash va natija tizimi

---

## 🛡 Ishlab Chiqarishga Tayyorlik va Xavfsizlik

Loyiha quyidagi mustahkamlashtiruvchi (hardening) qatlamlar bilan ta'minlangan:

### Global Xatolik Boshqaruvi
Barcha istisnolar bitta markazlashgan middleware (`GlobalExceptionHandlingMiddleware`)
orqali tutiladi va RFC 7807 (ProblemDetails) uslubidagi bir xil JSON javobga
aylantiriladi. Controller'larda takrorlanuvchi `try/catch` bloklariga ehtiyoj yo'q.

Domen istisnolari mos HTTP status kodlariga o'giriladi:

| Istisno                 | HTTP | `error`             |
|-------------------------|------|---------------------|
| `NotFoundException`     | 404  | `not_found`         |
| `ValidationException`   | 400  | `validation_error`  |
| `BadRequestException`   | 400  | `bad_request`       |
| `UnauthorizedException` | 401  | `unauthorized`      |
| `ForbiddenException`    | 403  | `forbidden`         |
| `ConflictException`     | 409  | `conflict`          |

Xatolik javobi namunasi:

```json
{
  "status": 409,
  "error": "conflict",
  "title": "Bu email allaqachon ro'yxatdan o'tgan",
  "traceId": "0HN...:00000001"
}
```

> ℹ️ Ichki (5xx) xatoliklarning texnik tafsilotlari faqat `Development`
> muhitida ochib beriladi.

### Xavfsizlik Sarlavhalari
Har bir javobga `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`
va `Permissions-Policy` sarlavhalari qo'shiladi.

### Rate Limiting
IP manzil bo'yicha daqiqasiga 100 ta so'rov cheklovi (`429 Too Many Requests`).

### JWT Validatsiyasi
Ilova ishga tushishida `JwtSettings` (Secret ≥ 256 bit, Issuer, Audience)
majburiy tekshiriladi. Token muddati `ClockSkew = 0` bilan aniq baholanadi.
Maxfiy kalitlar hech qachon jurnalga (log) yozilmaydi.

### CORS
Ruxsat etilgan manzillar `appsettings.json` dagi `Cors:AllowedOrigins`
orqali sozlanadi.

### Health Check
Ilova holatini kuzatish uchun `GET /health` endpoint'i mavjud.

### Testlar
Domen qatlami (`Money`, `PhoneNumber` value-object'lari va istisno ierarxiyasi)
uchun xUnit + FluentAssertions asosidagi birlik testlari `tests/Darmon.UnitTests`
katalogida joylashgan.


