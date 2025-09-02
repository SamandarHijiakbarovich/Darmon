using Darmon.Application.DTOs.ClickDtos.ClickrequestDto;
using Darmon.Application.DTOs.ClickDtos.ClickResponseDto;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Darmon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClickPaymentsController : ControllerBase
{
    private readonly IClickPaymentService _clickPaymentService;
    private readonly ILogger<ClickPaymentsController> _logger;

    public ClickPaymentsController(
        IClickPaymentService clickPaymentService,
        ILogger<ClickPaymentsController> logger)
    {
        _clickPaymentService = clickPaymentService;
        _logger = logger;
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
                request.Click_Trans_Id, request.Merchant_Trans_Id, request.Amount);

            var result = await _clickPaymentService.ProcessPrepareRequestAsync(request);

            if (result.Error != 0)
            {
                _logger.LogWarning("Click prepare failed: {Error} - {ErrorNote}", result.Error, result.Error_Note);
                return BadRequest(result);
            }

            _logger.LogInformation("Click prepare successful: {MerchantPrepareId}", result.Merchant_Prepare_Id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Click prepare error: {ClickTransId}", request.Click_Trans_Id);
            return StatusCode(500, new ClickPrepareResponseDto
            {
                Error = -8,
                Error_Note = "Server error"
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
                request.Click_Trans_Id, request.Merchant_Trans_Id, request.Error);

            var result = await _clickPaymentService.ProcessCompleteRequestAsync(request);

            if (result.Error != 0)
            {
                _logger.LogWarning("Click complete failed: {Error} - {ErrorNote}", result.Error, result.Error_Note);
                return BadRequest(result);
            }

            _logger.LogInformation("Click complete successful: {MerchantConfirmId}", result.Merchant_Confirm_Id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Click complete error: {ClickTransId}", request.Click_Trans_Id);
            return StatusCode(500, new ClickCompleteResponseDto
            {
                Error = -8,
                Error_Note = "Server error"
            });
        }
    }

    /// <summary>
    /// Clickdan kelgan notificationni qayta ishlash
    /// </summary>
    [HttpPost("notification")]
    public async Task<ActionResult> HandleNotification([FromForm] ClickCompleteRequestDto notification)
    {
        try
        {
            _logger.LogInformation("Click notification: {ClickTransId}, {MerchantTransId}, {Error}",
                notification.Click_Trans_Id, notification.Merchant_Trans_Id, notification.Error);

            // Notificationni qayta ishlash (complete bilan bir xil)
            var result = await _clickPaymentService.ProcessCompleteRequestAsync(notification);

            if (result.Error == 0)
            {
                _logger.LogInformation("Notification processed successfully: {MerchantTransId}", notification.Merchant_Trans_Id);
                return Ok(new { Success = true });
            }
            else
            {
                _logger.LogWarning("Notification processing failed: {Error}", result.Error);
                return BadRequest(new { Success = false, Error = result.Error });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Notification processing error: {ClickTransId}", notification.Click_Trans_Id);
            return StatusCode(500, new { Success = false, Message = "Server error" });
        }
    }

    /// <summary>
    /// Signature tekshirish (test uchun)
    /// </summary>
    [HttpPost("verify-signature")]
    public async Task<ActionResult> VerifySignature([FromBody] SignatureVerifyRequest request)
    {
        try
        {
            // Test uchun signature tekshirish
            var isValid = await _clickPaymentService.ValidateClickSignatureAsync(new ClickPrepareRequestDto
            {
                Click_Trans_Id = request.ClickTransId,
                Service_Id = request.ServiceId,
                Merchant_Trans_Id = request.MerchantTransId,
                Amount = request.Amount,
                Action = request.Action,
                Sign_Time = request.SignTime,
                Sign_String = request.SignString
            });

            return Ok(new
            {
                Success = true,
                IsValid = isValid
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Signature verification error");
            return StatusCode(500, new { Success = false, Message = "Server error" });
        }
    }
}

// Signature tekshirish uchun DTO
public class SignatureVerifyRequest
{
    public int ClickTransId { get; set; }
    public int ServiceId { get; set; }
    public int MerchantTransId { get; set; }
    public decimal Amount { get; set; }
    public int Action { get; set; }
    public string SignTime { get; set; }
    public string SignString { get; set; }
}