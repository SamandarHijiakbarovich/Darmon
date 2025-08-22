using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/deliveries")]
[Authorize]
public class DeliveriesController : ControllerBase
{
    // GET: api/deliveries/order/{orderId}
    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetDeliveryByOrder(int orderId)
    {
        // Buyurtma bo'yicha yetkazib berish ma'lumotlari
    }

    // GET: api/deliveries/active
    [HttpGet("active")]
    [Authorize(Roles = "Courier,Admin")]
    public async Task<IActionResult> GetActiveDeliveries()
    {
        // Faol yetkazib berishlar
    }

    // GET: api/deliveries/my-deliveries
    [HttpGet("my-deliveries")]
    [Authorize(Roles = "Courier")]
    public async Task<IActionResult> GetMyDeliveries()
    {
        // Kurierning o'z yetkazib berishlari
    }

    // PUT: api/deliveries/{id}/status
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Courier,Admin")]
    public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromBody] UpdateDeliveryStatusDto dto)
    {
        // Yetkazib berish statusini yangilash
    }

    // GET: api/deliveries/{id}/track
    [HttpGet("{id}/track")]
    public async Task<IActionResult> TrackDelivery(int id)
    {
        // Yetkazib berishni kuzatish
    }

    // POST: api/deliveries/{id}/location
    [HttpPost("{id}/location")]
    [Authorize(Roles = "Courier")]
    public async Task<IActionResult> UpdateDeliveryLocation(int id, [FromBody] LocationDto dto)
    {
        // Kurierning joylashuvini yangilash
    }

    // GET: api/deliveries/stats
    [HttpGet("stats")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDeliveryStats()
    {
        // Yetkazib berish statistikasi
    }
}