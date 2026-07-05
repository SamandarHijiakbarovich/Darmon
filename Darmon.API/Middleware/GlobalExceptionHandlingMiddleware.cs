using System.Text.Json;
using System.Text.Json.Serialization;
using Darmon.Domain.Exceptions;

namespace Darmon.API.Middleware;

/// <summary>
/// Butun ilova bo'ylab yuzaga keladigan istisnolarni tutib, ularni
/// bir xil (RFC 7807 ProblemDetails uslubidagi) JSON javobga aylantiradi.
/// Buning yordamida controller'larda takrorlanuvchi try/catch bloklariga
/// ehtiyoj qolmaydi va mijozlar doim bir xil formatdagi xatolik oladi.
/// </summary>
public sealed class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorType, title, errors) = MapException(exception);

        // 5xx xatoliklarni to'liq, mijoz xatoliklarini (4xx) esa qisqartirilgan
        // darajada jurnalga yozamiz.
        if (statusCode >= 500)
        {
            _logger.LogError(exception,
                "Ishlov berilmagan xatolik: {Message} ({Path})",
                exception.Message, context.Request.Path);
        }
        else
        {
            _logger.LogWarning(
                "So'rov xatoligi ({StatusCode}): {Message} ({Path})",
                statusCode, exception.Message, context.Request.Path);
        }

        var response = new ErrorResponse
        {
            Status = statusCode,
            Error = errorType,
            Title = title,
            TraceId = context.TraceIdentifier,
            Errors = errors,
            // Ichki xatolik tafsilotlari faqat Development muhitida ochiladi.
            Detail = statusCode >= 500 && !_environment.IsDevelopment()
                ? null
                : exception.Message
        };

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, SerializerOptions));
    }

    private static (int statusCode, string errorType, string title, IReadOnlyDictionary<string, string[]>? errors)
        MapException(Exception exception) => exception switch
    {
        ValidationException validation => (
            validation.StatusCode,
            validation.ErrorType,
            validation.Message,
            validation.Errors.Count > 0 ? validation.Errors : null),

        DomainException domain => (
            domain.StatusCode,
            domain.ErrorType,
            domain.Message,
            null),

        // Eski kod hali ham ApplicationException'dan foydalanadi -> 400.
        ApplicationException => (
            StatusCodes.Status400BadRequest,
            "bad_request",
            exception.Message,
            null),

        KeyNotFoundException => (
            StatusCodes.Status404NotFound,
            "not_found",
            exception.Message,
            null),

        UnauthorizedAccessException => (
            StatusCodes.Status401Unauthorized,
            "unauthorized",
            "Ruxsat berilmagan.",
            null),

        _ => (
            StatusCodes.Status500InternalServerError,
            "internal_server_error",
            "Ichki server xatoligi yuz berdi.",
            null)
    };

    private sealed class ErrorResponse
    {
        public int Status { get; init; }
        public string Error { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string? Detail { get; init; }
        public string? TraceId { get; init; }
        public IReadOnlyDictionary<string, string[]>? Errors { get; init; }
    }
}
