using Darmon.Application.DTOs.ProductDTos;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darmon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductService _productService;

    public ProductsController(ILogger<ProductsController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    // ✅ USER endpoints (hamma foydalanishi mumkin)
    // GET: api/products
    [HttpGet]
    [AllowAnonymous] // login bo‘lmagan ham ko‘ra oladi
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all products");
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null)
                return NotFound();

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // GET: api/products/search?keyword=aspirin
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        try
        {
            var products = await _productService.SearchAsync(keyword);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products with keyword '{Keyword}'", keyword);
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // GET: api/products/category/{categoryId}
    [HttpGet("category/{categoryId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        try
        {
            var products = await _productService.GetByCategoryAsync(categoryId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products for category ID {CategoryId}", categoryId);
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // GET: api/products/out-of-stock
    [HttpGet("out-of-stock")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOutOfStock()
    {
        try
        {
            var products = await _productService.GetOutOfStockAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving out-of-stock products");
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // GET: api/products/low-stock?threshold=5
    [HttpGet("low-stock")]
    [Authorize(Roles = "Seller")] // faqat Seller ko‘radi
    public async Task<IActionResult> GetLowStock([FromQuery] int threshold)
    {
        try
        {
            var products = await _productService.GetLowStockAsync(threshold);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving low stock products with threshold {Threshold}", threshold);
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // ✅ SELLER endpoints (faqat Seller foydalanadi)
    // POST: api/products
    [HttpPost]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var createdProduct = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var updatedProduct = await _productService.UpdateAsync(id, dto);
            if (updatedProduct is null)
                return NotFound();

            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID {ProductId}", id);
            return StatusCode(500, new { Error = "Server error" });
        }
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
            return StatusCode(500, new { Error = "Server error" });
        }
    }
}
