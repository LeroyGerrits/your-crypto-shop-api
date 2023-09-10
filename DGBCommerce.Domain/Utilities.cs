using System.Security.Cryptography;
using System.Text;

namespace DGBCommerce.Domain
{
    public static class Utilities
    {
        public static DateTime? DBNullableDateTime(object value)
            => value != null && value != DBNull.Value && DateTime.TryParse(value.ToString(), out var valueDateTime) ? valueDateTime : null;

        public static decimal? DbNullableDecimal(object value)
            => value != null && value != DBNull.Value && decimal.TryParse(value.ToString(), out var valueDecimal) ? valueDecimal : null;

        public static Guid? DbNullableGuid(object value)
            => value != null && value != DBNull.Value &&  Guid.TryParse(value.ToString(), out var valueGuid) ? valueGuid: null;

        public static int? DbNullableInt(object value)
            => value != null && value != DBNull.Value && int.TryParse(value.ToString(), out var valueInt) ? valueInt : null;

        public static string DbNullableString(object value)
            => value != null && value != DBNull.Value ? value.ToString()! : string.Empty;

        public static DateTime? NullableDateTime(object value)
                => value != null && DateTime.TryParse(value.ToString(), out var valueDateTime) ? valueDateTime : null;

        public static decimal? NullableDecimal(object value)
            => value != null && decimal.TryParse(value.ToString(), out var valueDecimal) ? valueDecimal : null;

        public static Guid? NullableGuid(object value)
            => value != null && Guid.TryParse(value.ToString(), out var valueGuid) ? valueGuid : null;

        public static int? NullableInt(object value)
            => value != null && int.TryParse(value.ToString(), out var valueInt) ? valueInt : null;

        public static string NullableString(object value)
            => value != null ? value.ToString()! : string.Empty;

        public static string HashStringSha256(string value)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            StringBuilder builder = new();

            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }

        public static string GenerateRandomString(int length)
        {
            string allowed = "123456789abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ";
            char[] randomChars = new char[length];

            for (int i = 0; i < length; i++)
                randomChars[i] = allowed[RandomNumberGenerator.GetInt32(0, allowed.Length)];

            return new string(randomChars);
        }

        public static string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }
    }
}
