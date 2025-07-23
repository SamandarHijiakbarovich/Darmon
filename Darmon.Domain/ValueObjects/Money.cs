public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; } = "UZS";
    private Money(decimal amount)
    {
        Amount = amount;
    }

     public static Money Create(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Pul miqdori manfiy bo'lishi mumkin emas");

        return new Money(amount);
    }

    // Operator overloads
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Har xil valyutalarni qo'shib bo'lmaydi");

        return new Money(a.Amount + b.Amount);
    }
    
    
    
}