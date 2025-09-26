using Darmon.Application.DTOs.ClickDtos.ClickrequestDto;
using Darmon.Application.DTOs.ClickDtos.ClickResponseDto;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Darmon.Application.DTOs.ClickDtos.RequestResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Darmon.Application.DTOs.ClickDtos.GetPaymentUrlDto;

namespace Darmon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClickPaymentsController : ControllerBase
{
    private readonly IClickPaymentService _clickPaymentService;
    private readonly IUserService _userService;
    private readonly ILogger<ClickPaymentsController> _logger;

    public ClickPaymentsController(
        IClickPaymentService clickPaymentService,
        IUserService userService,
        ILogger<ClickPaymentsController> logger)
    {
        _clickPaymentService = clickPaymentService;
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Yangi to'lov havolasini yaratish.
    /// </summary>
    [HttpGet("create")]
    public async Task<ActionResult<GetPaymentUrlDto>> CreatePayment(float amount, int UserId)
    {
        try
        {
            var userId = UserId;

            // To'lov miqdori minimal chegara (1000) dan kam bo'lmasligi kerak
            if (amount < 1000)
            {
                _logger.LogWarning("Invalid amount: {Amount}", amount);
                return BadRequest("Miqdor Min 1000 bo'lishi kerak");
            }

            // Bu metod avtomatik ravishda yangi tranzaksiya yaratadi va URL qaytaradi
            var urlResult = await _clickPaymentService.Create(amount, userId);

            if (!urlResult.Success)
            {
                _logger.LogWarning("Payment URL creation failed: {Message}", urlResult.Message);
                return BadRequest(urlResult.Message);
            }

            var result = new GetPaymentUrlDto
            {
                PayUrl = urlResult.Data
            };
            _logger.LogInformation("Payment URL created for user {UserId}: {PayUrl}", userId, result.PayUrl);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment URL.");
            return StatusCode(500, $"Havolani yaratishda xatolik yuz berdi. {ex.Message}");
        }
    }

    /// <summary>
    /// Click prepare request - to'lovni tayyorlash
    /// </summary>
    [HttpPost("prepare")]
    public async Task<ActionResult<ClickPrepareResponseDto>> Prepare([FromForm] ClickPrepareRequestDto request)
    {
        try
        {
            _logger.LogInformation("Click prepare request: {ClickTransId}, {MerchantTransId}, {Amount}",
                request.click_trans_id, request.merchant_trans_id, request.amount);

            var result = await _clickPaymentService.Prepare(request);

            // Click.uz xato kodlarini qaytarishni talab qiladi, shuning uchun Ok() va BadRequest() emas, Ok() va StatusCode(200) dan foydalanish kerak.
            // Va butun ClickPrepareResponseDto obyektini qaytarish lozim.
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Click prepare error: {ClickTransId}", request.click_trans_id);
            return StatusCode(500, new ClickPrepareResponseDto
            {
                click_trans_id = request.click_trans_id,
                merchant_trans_id = request.merchant_trans_id,
                merchant_prepare_id = "",
                error = 1, // Umuman server xatosi
                error_note = "Internal Server Error"
            });
        }
    }

    /// <summary>
    /// Click complete request - to'lovni yakunlash
    /// </summary>
    [HttpPost("complete")]
    public async Task<ActionResult<ClickCompleteResponseDto>> Complete([FromForm] ClickCompleteRequestDto request)
    {
        try
        {
            _logger.LogInformation("Click complete request: {ClickTransId}, {MerchantTransId}, {Error}",
                request.click_trans_id, request.merchant_trans_id, request.error);

            var result = await _clickPaymentService.Complete(request);

            // Xuddi "prepare" kabi, Ok() ni ishlatib, butun javob obyektini qaytaramiz
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Click complete error: {ClickTransId}", request.click_trans_id);
            return StatusCode(500, new ClickCompleteResponseDto
            {
                click_trans_id = request.click_trans_id,
                merchant_trans_id = request.merchant_trans_id,
                error = 1, // Umuman server xatosi
                error_note = "Internal Server Error"
            });
        }
    }
}