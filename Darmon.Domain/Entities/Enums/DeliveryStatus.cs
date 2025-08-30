public enum DeliveryStatus
{
    Pending,        // Buyurtma yaratilgan
    Accepted,       // Kuryer tomonidan qabul qilingan
    Preparing,      // Tayyorlanmoqda
    OnTheWay,       // Yo‘lda
    Arrived,        // Yetib kelgan (lekin hali topshirilmagan)
    Delivered,      // Yetkazilgan
    Failed,         // Yetkazib bo‘lmagan
    Cancelled       // Buyurtma bekor qilingan
}
