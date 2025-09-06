using Darmon.Application.DTOs.AddressDtos;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darmon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // faqat admin ishlata oladi
    [Produces("application/json")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressService addressService, ILogger<AddressController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        /// <summary>
        /// Get address by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAddressById(int id)
        {
            try
            {
                var address = await _addressService.GetAddressByIdAsync(id);
                if (address == null)
                {
                    _logger.LogWarning("Address {AddressId} not found", id);
                    return NotFound(new { Error = $"Address with ID {id} not found." });
                }

                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving address {AddressId}", id);
                return StatusCode(500, new { Error = "Internal server error while retrieving address." });
            }
        }

        /// <summary>
        /// Create new address
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { Error = "Invalid address data." });

                var address = await _addressService.CreateAddressAsync(dto);
                return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input while creating address");
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new address");
                return StatusCode(500, new { Error = "Internal server error while creating address." });
            }
        }

        /// <summary>
        /// Update existing address
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateAddressDto dto)
        {
            try
            {
                var updated = await _addressService.UpdateAddressAsync(id, dto);
                if (updated == null)
                {
                    _logger.LogWarning("Address {AddressId} not found for update", id);
                    return NotFound(new { Error = $"Address with ID {id} not found." });
                }

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input while updating address {AddressId}", id);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address {AddressId}", id);
                return StatusCode(500, new { Error = "Internal server error while updating address." });
            }
        }

        /// <summary>
        /// Delete address
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var result = await _addressService.DeleteAddressAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Address {AddressId} not found for deletion", id);
                    return NotFound(new { Error = $"Address with ID {id} not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting address {AddressId}", id);
                return StatusCode(500, new { Error = "Internal server error while deleting address." });
            }
        }

        /// <summary>
        /// Get all addresses by branchId
        /// </summary>
        [HttpGet("branch/{branchId:int}")]
        [ProducesResponseType(typeof(IEnumerable<AddressDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAddressesByBranch(int branchId)
        {
            try
            {
                var addresses = await _addressService.GetUserAddressesAsync(branchId);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving addresses for branch {BranchId}", branchId);
                return StatusCode(500, new { Error = "Internal server error while retrieving addresses." });
            }
        }
    }
}
