public abstract class ValueObject:IEquatable<ValueObject>
{
    // IEquatable<ValueObject> talab qiladigan metod
    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    // Equals() va GetHashCode() override qilish
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        return Equals((ValueObject)obj);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    // Har bir ValueObject uchun tenglik komponentlarini qaytaruvchi metod
    protected abstract IEnumerable<object?> GetEqualityComponents();
}