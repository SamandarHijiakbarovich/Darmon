using System;
using FluentAssertions;
using Xunit;

namespace Darmon.UnitTests.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("+998901234567")]
    [InlineData("+998001112233")]
    public void Create_WithValidUzbekNumber_Succeeds(string input)
    {
        var phone = PhoneNumber.Create(input);

        phone.Value.Should().Be(input);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("998901234567")]      // + belgisi yo'q
    [InlineData("+99890123456")]      // raqamlar yetishmaydi
    [InlineData("+9989012345678")]    // raqamlar ortiqcha
    [InlineData("+1234567890")]       // noto'g'ri kod
    public void Create_WithInvalidNumber_Throws(string input)
    {
        var act = () => PhoneNumber.Create(input);

        act.Should().Throw<ArgumentException>();
    }
}
