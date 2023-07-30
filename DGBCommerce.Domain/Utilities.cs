using System.Security.Cryptography;
using System.Text;

namespace DGBCommerce.Domain
{
    public static class Utilities
    {
        public static string HashStringSha256(string value)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            StringBuilder builder = new();

            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }

        public static DateTime? DBNullableDateTime(object value)
            => value != null && DateTime.TryParse(value.ToString(), out var valueDateTime) ? valueDateTime : null;

        public static decimal? DbNullableDecimal(object value)
            => value != null && decimal.TryParse(value.ToString(), out var valueDecimal) ? valueDecimal : null;

        public static int? DbNullableInt(object value)
            => value != null && int.TryParse(value.ToString(), out var valueInt) ? valueInt : null;

        public static string DbNullableString(object value)
        {
            if (value == null)
                return string.Empty;

            return value.ToString()!;
        }

        public static DateTime? NullableDateTime(object value)
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
