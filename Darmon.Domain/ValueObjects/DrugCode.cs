public sealed record DrugCode
{
    public string Value { get; }

    private DrugCode(string value) => Value = value;

    public static DrugCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 8)
            throw new ArgumentException("Dorining kodi 8 ta belgidan iborat bo'lishi kerak");

        return new DrugCode(code);
    }
}