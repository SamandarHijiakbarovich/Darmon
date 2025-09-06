using Darmon.Application.DTOs.BranchDtos;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darmon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // faqat Admin ishlata oladi
    [Produces("application/json")]
    public class PharmacysController : ControllerBase
    {
        private readonly IBranchService _pharmacyService;
        private readonly ILogger<PharmacysController> _logger;

        public PharmacysController(IBranchService pharmacyService, ILogger<PharmacysController> logger)
        {
            _pharmacyService = pharmacyService;
            _logger = logger;
        }

        /// <summary>
        /// Get all pharmacies
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BranchDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var pharmacys = await _pharmacyService.GetAllAsync();
                return Ok(pharmacys);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting pharmacy list");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Get pharmacy by ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BranchDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var pharmacy = await _pharmacyService.GetByIdAsync(id);
                if (pharmacy == null)
                {
                    _logger.LogWarning("Pharmacy {PharmacyId} not found", id);
                    return NotFound(new { Error = $"Pharmacy with ID {id} not found" });
                }

                return Ok(pharmacy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting pharmacy {PharmacyId}", id);
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Create new pharmacy
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BranchDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdPharmacy = await _pharmacyService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdPharmacy.Id }, createdPharmacy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating pharmacy");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Update existing pharmacy
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(BranchDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _pharmacyService.UpdateAsync(id, dto);
                if (updated == null)
                {
                    _logger.LogWarning("Pharmacy {PharmacyId} not found for update", id);
                    return NotFound(new { Error = $"Pharmacy with ID {id} not found" });
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating pharmacy {PharmacyId}", id);
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }

        /// <summary>
        /// Delete pharmacy
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _pharmacyService.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Pharmacy {PharmacyId} not found for deletion", id);
                    return NotFound(new { Error = $"Pharmacy with ID {id} not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting pharmacy {PharmacyId}", id);
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }
    }
}
