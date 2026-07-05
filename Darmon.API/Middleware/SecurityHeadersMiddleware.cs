namespace Darmon.API.Middleware;

/// <summary>
/// Har bir javobga keng tarqalgan xavfsizlik sarlavhalarini qo'shadi.
/// Bu clickjacking, MIME sniffing va ma'lumot sizib chiqishi kabi
/// hujumlarga qarshi himoyani kuchaytiradi.
/// </summary>
public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;

        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-Frame-Options"] = "DENY";
        headers["Referrer-Policy"] = "no-referrer";
        headers["X-XSS-Protection"] = "0";
        headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
        // Server texnologiyasini oshkor qiluvchi sarlavhani olib tashlaymiz.
        headers.Remove("X-Powered-By");

        await _next(context);
    }
}
