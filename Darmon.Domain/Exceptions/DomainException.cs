using System;
using System.Collections.Generic;

namespace Darmon.Domain.Exceptions;

/// <summary>
/// Barcha biznes-mantiq (domain) xatoliklari uchun bazaviy sinf.
/// Har bir hosila sinf mos HTTP status kodini belgilaydi, shu tufayli
/// global xatolik boshqaruvchisi javoblarni bir xil ko'rinishda qaytaradi.
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>Ushbu xatolikka mos keladigan HTTP status kodi.</summary>
    public abstract int StatusCode { get; }

    /// <summary>Mashina o'qiy oladigan qisqa xatolik turi (masalan, "not_found").</summary>
    public abstract string ErrorType { get; }

    protected DomainException(string message) : base(message) { }

    protected DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>So'ralgan resurs topilmaganda (HTTP 404).</summary>
public sealed class NotFoundException : DomainException
{
    public override int StatusCode => 404;
    public override string ErrorType => "not_found";

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string name, object key)
        : base($"\"{name}\" (identifikator: {key}) topilmadi.") { }
}

/// <summary>Kiritilgan ma'lumotlar validatsiyadan o'tmaganda (HTTP 400).</summary>
public sealed class ValidationException : DomainException
{
    public override int StatusCode => 400;
    public override string ErrorType => "validation_error";

    /// <summary>Maydon nomi -> xatolik xabarlari ko'rinishidagi tafsilotlar.</summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("Bir yoki bir nechta validatsiya xatoliklari yuz berdi.")
    {
        Errors = errors;
    }
}

/// <summary>Resurs holati bilan ziddiyat yuzaga kelganda (HTTP 409).</summary>
public sealed class ConflictException : DomainException
{
    public override int StatusCode => 409;
    public override string ErrorType => "conflict";

    public ConflictException(string message) : base(message) { }
}

/// <summary>Autentifikatsiya muvaffaqiyatsiz bo'lganda (HTTP 401).</summary>
public sealed class UnauthorizedException : DomainException
{
    public override int StatusCode => 401;
    public override string ErrorType => "unauthorized";

    public UnauthorizedException(string message) : base(message) { }
}

/// <summary>Foydalanuvchida ruxsat bo'lmaganda (HTTP 403).</summary>
public sealed class ForbiddenException : DomainException
{
    public override int StatusCode => 403;
    public override string ErrorType => "forbidden";

    public ForbiddenException(string message) : base(message) { }
}

/// <summary>Umumiy noto'g'ri so'rov (HTTP 400).</summary>
public sealed class BadRequestException : DomainException
{
    public override int StatusCode => 400;
    public override string ErrorType => "bad_request";

    public BadRequestException(string message) : base(message) { }
}
