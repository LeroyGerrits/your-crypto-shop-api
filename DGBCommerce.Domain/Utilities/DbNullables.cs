namespace DGBCommerce.Domain.Utilities
{
    public static class DbNullables
    {
        public static DateTime? NullableDate(object value)
            => value != null && value != DBNull.Value && DateTime.TryParse(value.ToString(), out var valueDateTime) ? valueDateTime : null;

        public static decimal? NullableDecimal(object value)
            => value != null && value != DBNull.Value && decimal.TryParse(value.ToString(), out var valueDecimal) ? valueDecimal : null;

        public static int? NullableInt(object value)
            => value != null && value != DBNull.Value && int.TryParse(value.ToString(), out var valueInt) ? valueInt : null;

        public static string NullableString(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            return value.ToString()!;
        }
    }
}