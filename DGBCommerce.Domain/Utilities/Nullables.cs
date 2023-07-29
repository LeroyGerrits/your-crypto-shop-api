namespace DGBCommerce.Domain.Utilities
{
    public static class Nullables
    {
        public static DateTime? NullableDate(object value)
            => value != null && DateTime.TryParse(value.ToString(), out var valueDateTime) ? valueDateTime : null;

        public static decimal? NullableDecimal(object value)
            => value != null && decimal.TryParse(value.ToString(), out var valueDecimal) ? valueDecimal : null;

        public static int? NullableInt(object value)
            => value != null && int.TryParse(value.ToString(), out var valueInt) ? valueInt : null;

        public static string NullableString(object value)
        {
            if (value == null)
                return string.Empty;

            return value.ToString()!;
        }
    }
}