using Darmon.Application.DTOs.CartItemDtos;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Darmon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class CartItemsController : ControllerBase
{
    private readonly ICartItemService _cartItemService;
    private readonly ILogger<CartItemsController> _logger;

    public CartItemsController(ICartItemService cartItemService, ILogger<CartItemsController> logger)
    {
        _cartItemService = cartItemService;
        _logger = logger;
    }

    /// <summary>
    /// Get current user's cart items
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CartItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserCartItems()
    {
        try
        {
            var userId = GetCurrentUserId();
            var items = await _cartItemService.GetUserCartItemsAsync(userId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user cart items");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Add item to cart
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CartItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCartItem([FromBody] CreateCartItemDto  dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _cartItemService.AddCartItemAsync(userId, dto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid request to add item to cart");
            return BadRequest(new { Error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operation failed while adding item to cart");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding item to cart");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Update cart item quantity
    /// </summary>
    [HttpPut("{itemId}")]
    [ProducesResponseType(typeof(CartItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCartItem(int itemId, [FromBody] UpdateCartItemDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _cartItemService.UpdateCartItemQuantityAsync(userId, itemId, dto.Quantity);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid quantity for cart item {ItemId}", itemId);
            return BadRequest(new { Error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Cart item {ItemId} not found", itemId);
            return NotFound(new { Error = "Item not found in cart" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operation failed while updating cart item {ItemId}", itemId);
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item {ItemId}", itemId);
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Remove item from cart
    /// </summary>
    [HttpDelete("{itemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveCartItem(int itemId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _cartItemService.RemoveCartItemAsync(userId, itemId);

            if (!result)
            {
                _logger.LogWarning("Cart item {ItemId} not found for deletion", itemId);
                return NotFound(new { Error = "Item not found in cart" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cart item {ItemId}", itemId);
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Clear user's cart
    /// </summary>
    [HttpDelete("clear")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ClearCart()
    {
        try
        {
            var userId = GetCurrentUserId();
            await _cartItemService.ClearUserCartAsync(userId);
            return Ok(new { Message = "Cart cleared successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Get cart items count
    /// </summary>
    [HttpGet("count")]
    [ProducesResponseType(typeof(CartCountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCartItemsCount()
    {
        try
        {
            var userId = GetCurrentUserId();
            var count = await _cartItemService.GetUserCartItemsCountAsync(userId);
            return Ok(new CartCountDto { Count = count });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart items count");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Get cart summary
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(CartCountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCartSummary()
    {
        try
        {
            var userId = GetCurrentUserId();
            var summary = await _cartItemService.GetCartSummaryAsync(userId);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart summary");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    /// <summary>
    /// Check if product is in cart
    /// </summary>
    [HttpGet("check-product/{productId}")]
    [ProducesResponseType(typeof(ProductInCartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckProductInCart(int productId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isInCart = await _cartItemService.CheckProductInCartAsync(userId, productId);
            return Ok(new ProductInCartDto { IsInCart = isInCart, ProductId = productId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking product {ProductId} in cart", productId);
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in claims");
        }
        return userId;
    }
}

// DTO Definitions
public record CartCountDto
{
    public int Count { get; set; }
}

public record ProductInCartDto
{
    public int ProductId { get; set; }
    public bool IsInCart { get; set; }
}