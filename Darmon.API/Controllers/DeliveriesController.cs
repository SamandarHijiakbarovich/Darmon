using Darmon.Application.DTOs.DeliveryDtos;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/deliveries")]
[Authorize]
public class DeliveriesController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public DeliveriesController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    // POST: api/deliveries
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryDto dto)
    {
        var delivery = await _deliveryService.CreateDeliveryAsync(dto);
        return CreatedAtAction(nameof(GetDeliveryById), new { id = delivery.Id }, delivery);
    }

    // GET: api/deliveries/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeliveryById(int id)
    {
        var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
        return delivery != null ? Ok(delivery) : NotFound();
    }

    // GET: api/deliveries/order/{orderId}
    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetDeliveryByOrderId(int orderId)
    {
        var delivery = await _deliveryService.GetDeliveryByOrderIdAsync(orderId);
        return delivery != null ? Ok(delivery) : NotFound();
    }

    // PUT: api/deliveries/{id}/status
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Courier,Admin")]
    public async Task<IActionResult> UpdateDeliveryStatus(int id, [FromBody] UpdateDeliveryDto dto)
    {
        var updated = await _deliveryService.UpdateDeliveryStatusAsync(id, dto);
        return updated != null ? Ok(updated) : BadRequest("Statusni yangilab bo'lmadi.");
    }

    // POST: api/deliveries/{id}/assign-courier/{courierId}
    [HttpPost("{id}/assign-courier/{courierId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignCourier(int id, int courierId)
    {
        var success = await _deliveryService.AssignCourierAsync(id, courierId);
        return success ? NoContent() : BadRequest("Kurierni tayinlab bo'lmadi.");
    }

    // GET: api/deliveries/active
    [HttpGet("active")]
    [Authorize(Roles = "Courier,Admin")]
    public async Task<IActionResult> GetActiveDeliveries()
    {
        var deliveries = await _deliveryService.GetActiveDeliveriesAsync();
        return Ok(deliveries);
    }

    // GET: api/deliveries/calculate-fee?addressId=1&branchId=2
    [HttpGet("calculate-fee")]
    public async Task<IActionResult> CalculateDeliveryFee([FromQuery] int addressId, [FromQuery] int branchId)
    {
        var fee = await _deliveryService.CalculateDeliveryFeeAsync(addressId, branchId);
        return Ok(new { Fee = fee });
    }



}
