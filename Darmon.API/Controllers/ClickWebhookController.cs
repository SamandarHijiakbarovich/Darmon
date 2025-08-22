using Darmon.Application.DTOs.PaymentDtos;
using Darmon.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/click")]
public class ClickWebhookController : ControllerBase
{
    private readonly IClickPaymentService _clickService;

    public ClickWebhookController(IClickPaymentService clickService)
    {
        _clickService = clickService;
    }

    [HttpPost("payment")]
    public async Task<IActionResult> HandlePayment([FromForm] PrepareRequestDto dto)
    {
        return dto.Action switch
        {
            "0" => Ok(await _clickService.PrepareAsync(dto)),

            "1" => Ok(await _clickService.CompleteAsync(new CompleteRequestDto(
                ClickTransId: dto.ClickTransId,
                ServiceId: dto.ServiceId,
                MerchantTransId: dto.MerchantTransId,
                SignTime: dto.SignTime,
                SignString: dto.SignString,
                Amount: dto.Amount,
                Action: dto.Action,
                Error: dto.Error,
                MerchantPrepareId: dto.MerchantPrepareId,
                ClickPayDocId: dto.ClickPayDocId
            ))),

            _ => BadRequest(new ClickResponseDto(-8, "Invalid action", dto.MerchantTransId, ""))
        };
    }
}
