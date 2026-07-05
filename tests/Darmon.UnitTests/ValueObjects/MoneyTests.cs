using System;
using FluentAssertions;
using Xunit;

namespace Darmon.UnitTests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_WithPositiveAmount_SetsAmountAndDefaultCurrency()
    {
        var money = Money.Create(1500m);

        money.Amount.Should().Be(1500m);
        money.Currency.Should().Be("UZS");
    }

    [Fact]
    public void Create_WithZero_IsAllowed()
    {
        var money = Money.Create(0m);

        money.Amount.Should().Be(0m);
    }

    [Fact]
    public void Create_WithNegativeAmount_Throws()
    {
        var act = () => Money.Create(-1m);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Add_TwoAmounts_ReturnsSum()
    {
        var result = Money.Create(1000m) + Money.Create(250m);

        result.Amount.Should().Be(1250m);
        result.Currency.Should().Be("UZS");
    }

    [Fact]
    public void Equality_SameAmount_AreEqual()
    {
        Money.Create(500m).Should().Be(Money.Create(500m));
    }
}
