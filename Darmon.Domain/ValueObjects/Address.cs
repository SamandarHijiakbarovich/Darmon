namespace Darmon.Domain.ValueObjects;

internal sealed record Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }

    private Address(string street, string city, string postalcode)
    {
        Street = street;
        City = city;
        PostalCode = postalcode;
    }

    public static Address Create(string street, string city, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Ko'cha nomi bo'sh bo'lishi mumkin emas");
        
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Shahar nomi bo'sh bo'lishi mumkin emas");

        if (!IsValidPostalCode(postalCode))
            throw new ArgumentException("Noto'g'ri pochta indeksi formati");

        return new Address(street, city, postalCode);
    }

    private static bool IsValidPostalCode(string postalCode) 
        => !string.IsNullOrWhiteSpace(postalCode) && postalCode.Length == 6;
}
