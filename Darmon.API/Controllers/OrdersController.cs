using Darmon.Application.DTOs.OrderDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darmon.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // barcha endpointlarga authentication talab qilinadi
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    // ====================== USER (Mijoz) ======================

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetUserOrders), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating order");
            return StatusCode(500, "An error occurred while creating the order.");
        }
    }

    [HttpGet("my")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        try
        {
            var result = await _orderService.GetUserOrdersAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching user orders");
            return StatusCode(500, "An error occurred while fetching user orders.");
        }
    }

    [HttpPut("{id}/cancel")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var result = await _orderService.UpdateAsync(
                id,
                new UpdateOrderDto { Status = OrderStatus.Cancelled } // <-- enum bilan
            );

            if (result == null)
                return NotFound(new { Error = $"Order {id} not found or cannot be cancelled." });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while cancelling order {id}");
            return StatusCode(500, "An error occurred while cancelling the order.");
        }
    }

    // ====================== SELLER (Apteka) ======================

    [HttpPut("{id}/accept")]
    [Authorize(Roles = "PharmacyStaff")]
    public async Task<IActionResult> Accept(int id, int userId)
    {
        try
        {
            var result = await _orderService.AcceptOrderAsync(id, userId);
            if (!result)
                return NotFound(new { Error = $"Order {id} not found." });

            return Ok(new { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while accepting order {id}");
            return StatusCode(500, "An error occurred while accepting the order.");
        }
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "PharmacyStaff")]
    public async Task<IActionResult> Reject(int id, int userId)
    {
        try
        {
            var result = await _orderService.RejectOrderAsync(id, userId);
            if (!result)
                return NotFound(new { Error = $"Order {id} not found." });

            return Ok(new { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while rejecting order {id}");
            return StatusCode(500, "An error occurred while rejecting the order.");
        }
    }

    [HttpPut("{id}/ready")]
    [Authorize(Roles = "PharmacyStaff")]
    public async Task<IActionResult> MarkReady(int id, int userId)
    {
        try
        {
            var result = await _orderService.MarkAsReadyAsync(id, userId);
            if (!result)
                return NotFound(new { Error = $"Order {id} not found." });

            return Ok(new { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while marking order {id} as ready");
            return StatusCode(500, "An error occurred while updating the order.");
        }
    }

    // ====================== COURIER ======================

    [HttpGet("courier/{courierId}")]
    [Authorize(Roles = "Courier")]
    public async Task<IActionResult> GetCourierOrders(int courierId)
    {
        try
        {
            var result = await _orderService.GetCourierOrdersAsync(courierId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching courier orders");
            return StatusCode(500, "An error occurred while fetching courier orders.");
        }
    }

    [HttpPut("{id}/delivered")]
    [Authorize(Roles = "Courier")]
    public async Task<IActionResult> MarkDelivered(int id, int courierId)
    {
        try
        {
            var result = await _orderService.MarkAsDeliveredAsync(id, courierId);
            if (!result)
                return NotFound(new { Error = $"Order {id} not found." });

            return Ok(new { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while marking order {id} as delivered");
            return StatusCode(500, "An error occurred while updating the order.");
        }
    }

    // ====================== ADMIN ======================

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _orderService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching all orders");
            return StatusCode(500, "An error occurred while fetching all orders.");
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _orderService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { Error = $"Order {id} not found." });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while fetching order {id}");
            return StatusCode(500, "An error occurred while fetching the order.");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result)
                return NotFound(new { Error = $"Order {id} not found." });

            return Ok(new { Success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error while deleting order {id}");
            return StatusCode(500, "An error occurred while deleting the order.");
        }
    }

    // ====================== STATISTICS & QUERIES ======================

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        try
        {
            var result = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching orders by status");
            return StatusCode(500, "An error occurred while fetching orders by status.");
        }
    }

    [HttpGet("date-range")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByDateRange(DateTime start, DateTime end)
    {
        try
        {
            var result = await _orderService.GetOrdersByDateRangeAsync(start, end);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching orders by date range");
            return StatusCode(500, "An error occurred while fetching orders by date range.");
        }
    }

    [HttpGet("stats/total")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTotalOrders()
    {
        try
        {
            var count = await _orderService.GetTotalOrdersCountAsync();
            return Ok(new { TotalOrders = count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching total orders count");
            return StatusCode(500, "An error occurred while fetching total orders count.");
        }
    }

    [HttpGet("stats/status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetCountByStatus(string status)
    {
        try
        {
            var count = await _orderService.GetOrdersCountByStatusAsync(status);
            return Ok(new { Status = status, Count = count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching orders count by status");
            return StatusCode(500, "An error occurred while fetching orders count by status.");
        }
    }

    [HttpGet("stats/revenue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRevenue()
    {
        try
        {
            var revenue = await _orderService.GetTotalRevenueAsync();
            return Ok(new { TotalRevenue = revenue });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching total revenue");
            return StatusCode(500, "An error occurred while fetching total revenue.");
        }
    }
}
