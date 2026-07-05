using Darmon.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Darmon.UnitTests.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void NotFoundException_HasCorrectStatusAndType()
    {
        var ex = new NotFoundException("User", 42);

        ex.StatusCode.Should().Be(404);
        ex.ErrorType.Should().Be("not_found");
        ex.Message.Should().Contain("42");
    }

    [Fact]
    public void ConflictException_HasCorrectStatusAndType()
    {
        var ex = new ConflictException("mavjud");

        ex.StatusCode.Should().Be(409);
        ex.ErrorType.Should().Be("conflict");
    }

    [Fact]
    public void UnauthorizedException_HasCorrectStatusAndType()
    {
        var ex = new UnauthorizedException("noto'g'ri parol");

        ex.StatusCode.Should().Be(401);
        ex.ErrorType.Should().Be("unauthorized");
    }

    [Fact]
    public void ValidationException_WithErrors_ExposesThem()
    {
        var errors = new Dictionary<string, string[]>
        {
            ["Email"] = new[] { "Email noto'g'ri" }
        };

        var ex = new ValidationException(errors);

        ex.StatusCode.Should().Be(400);
        ex.ErrorType.Should().Be("validation_error");
        ex.Errors.Should().ContainKey("Email");
    }

    [Fact]
    public void AllDomainExceptions_DeriveFromDomainException()
    {
        new NotFoundException("x").Should().BeAssignableTo<DomainException>();
        new ConflictException("x").Should().BeAssignableTo<DomainException>();
        new UnauthorizedException("x").Should().BeAssignableTo<DomainException>();
        new ForbiddenException("x").Should().BeAssignableTo<DomainException>();
        new BadRequestException("x").Should().BeAssignableTo<DomainException>();
        new ValidationException("x").Should().BeAssignableTo<DomainException>();
    }
}
