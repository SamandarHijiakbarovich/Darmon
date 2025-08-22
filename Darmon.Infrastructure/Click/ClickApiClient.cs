
using Darmon.Application.Interfaces;
using Darmon.Application.Services.Click;
using Darmon.Infrastructure.Helpers;
using Darmon.Infrastructure.SettingModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Click
{
    public class ClickApiClient:IClickApiClient
    {
        /*private readonly HttpClient _httpClient;
        private readonly ClickSettings _settings;

        public ClickApiClient(HttpClient httpClient, IOptions<ClickSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        private void SetAuthHeader()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var digest = ClickAuthHelper.GenerateSha1($"{timestamp}{_settings.SecretKey}");
            var auth = $"{_settings.MerchantUserId}:{digest}:{timestamp}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Auth", auth);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
        }

        // 1. Asosiy to‘lov operatsiyalari
        public async Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto request)
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/invoice/create", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InvoiceResponseDto>() ?? new();
        }

        public async Task<PaymentStatusResponseDto> CheckPaymentStatusAsync(long paymentId)
        {
            SetAuthHeader();
            var url = $"{_settings.BaseUrl}/payment/status/{_settings.ServiceId}/{paymentId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PaymentStatusResponseDto>() ?? new();
        }

        public async Task<PaymentStatusResponseDto> CheckStatusByMerchantTransIdAsync(string merchantTransId)
        {
            SetAuthHeader();
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd"); // yoki parametr sifatida olinsin
            var url = $"{_settings.BaseUrl}/payment/status_by_mti/{_settings.ServiceId}/{merchantTransId}/{date}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PaymentStatusResponseDto>() ?? new();
        }

        public async Task<PaymentStatusResponseDto> CancelPaymentAsync(long paymentId)
        {
            SetAuthHeader();
            var url = $"{_settings.BaseUrl}/payment/reversal/{_settings.ServiceId}/{paymentId}";
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PaymentStatusResponseDto>() ?? new();
        }

        // 2. Karta tokeni bilan ishlash
        public async Task<CardTokenResponseDto> CreateCardTokenAsync(CardTokenRequestDto request)
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/card_token/request", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CardTokenResponseDto>() ?? new();
        }

        public async Task<CardTokenVerifyResponseDto> VerifyCardTokenAsync(CardTokenVerifyDto request)
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/card_token/verify", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CardTokenVerifyResponseDto>() ?? new();
        }

        public async Task<CardTokenPaymentResponseDto> PayWithCardTokenAsync(CardTokenPaymentDto request)
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/card_token/payment", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CardTokenPaymentResponseDto>() ?? new();
        }

        // 3. Qo‘shimcha operatsiyalar
        public async Task<FiscalSubmitResponseDto> SubmitFiscalDataAsync(FiscalSubmitRequestDto request)
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/fiscal/submit", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FiscalSubmitResponseDto>() ?? new();
        }*/
    }
}
