public static class StringExtensions
{
    public static bool InIgnoreCase(this string? value, params string[] allowedValues)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return allowedValues.Any(v =>
            v.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
