using Darmon.Application.DTOs.PaymentDtos;
using Darmon.Application.Interfaces;

public class ClickPaymentService : IClickPaymentService
{
    public async Task<ClickResponseDto> PrepareAsync(PrepareRequestDto dto)
    {
        if (!IsValidSignature(dto))
            return new ClickResponseDto(-1, "Invalid signature", dto.MerchantTransId, "");

        var order = await GetOrderAsync(dto.MerchantTransId);
        if (order is null)
            return new ClickResponseDto(-5, "Order not found", dto.MerchantTransId, "");

        var prepareId = Guid.NewGuid().ToString();

        return new ClickResponseDto(0, "Success", dto.MerchantTransId, prepareId);
    }

    public async Task<ClickResponseDto> CompleteAsync(CompleteRequestDto dto)
    {
        if (!IsValidSignature(dto))
            return new ClickResponseDto(-1, "Invalid signature", dto.MerchantTransId, dto.MerchantPrepareId);

        var success = await CompleteOrderAsync(dto.MerchantTransId, dto.Amount);
        if (!success)
            return new ClickResponseDto(-6, "Failed to complete order", dto.MerchantTransId, dto.MerchantPrepareId);

        return new ClickResponseDto(0, "Success", dto.MerchantTransId, dto.MerchantPrepareId);
    }

    private bool IsValidSignature(PrepareRequestDto dto)
    {
        // TODO: Implement signature verification using secret key
        return true;
    }

    private Task<Order?> GetOrderAsync(string merchantTransId)
    {
        // TODO: Fetch order from DB
        return Task.FromResult<Order?>(new Order { Id = merchantTransId });
    }

    private Task<bool> CompleteOrderAsync(string merchantTransId, string amount)
    {
        // TODO: Update order status in DB
        return Task.FromResult(true);
    }

    // Dummy Order class for illustration
    private class Order
    {
        public string Id { get; set; } = default!;
    }
}
