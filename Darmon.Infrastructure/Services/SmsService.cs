using Darmon.Infrastructure.Services.IServices;
using Darmon.Infrastructure.SettingModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Infrastructure.Services;

public class SmsService : ISmsService
{
    private readonly SmsSettings _settings;
    private readonly HttpClient _httpClient;
    public SmsService(IOptions<SmsSettings> settings, HttpClient httpClient)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
    }
    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        var response = await _httpClient.GetAsync(
                $"{_settings.ApiUrl}?phone={phoneNumber}&text={message}&key={_settings.ApiKey}");

        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> VerifyOtpAsync(string phoneNumber, string otpCode)
    {
        var response = await _httpClient.GetAsync(
                $"{_settings.OtpVerifyUrl}?phone={phoneNumber}&code={otpCode}");

        return response.IsSuccessStatusCode;
    }
}
