using System.Text.RegularExpressions;
public sealed record PhoneNumber
{
    public string Value { get; }

        private PhoneNumber(string value) => Value = value;

        public static PhoneNumber Create(string phoneNumber)
        {
            if (!IsValidPhoneNumber(phoneNumber))
                throw new ArgumentException("Telefon raqami +998XXXXXXXXX formatida bo'lishi kerak");

            return new PhoneNumber(phoneNumber);
        }

        private static bool IsValidPhoneNumber(string phone)
            => !string.IsNullOrWhiteSpace(phone) && 
               Regex.IsMatch(phone, @"^\+998\d{9}$");
    
}