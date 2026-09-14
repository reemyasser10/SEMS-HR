using System.Text;

namespace Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string SafeTrim(this string value)
        {
            return value == null ? string.Empty : value.Trim();
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }



        public static string SafeReplace(this string value, string oldChar, string newChar)
        {
            return value == null ? string.Empty : value.Replace(oldChar, newChar);
        }

        public static string SafeLower(this string value)
        {
            return value == null ? string.Empty : value.ToLowerInvariant();
        }

        public static bool IsExisting(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static int ParseToInt(this string value)
        {
            value = value.SafeLower().SafeTrim();

            _ = int.TryParse(value, out int result);

            return result;
        }

        public static DateTime? ParseToDateTime(this string value)
        {
            value = value.SafeLower().SafeTrim();

            if (DateTime.TryParse(value, out DateTime result))
            {
                return result;
            }

            return null;
        }

        public static string CommaEncode(this string value)
        {
            return value == null ? string.Empty : value.Replace(",", @"\002C");
        }

        public static string CommaDecode(this string value)
        {
            return value == null ? string.Empty : value.Replace(@"\002C", ",");
        }

        public static bool IsBase64String(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            // Remove whitespace, padding is allowed at the end
            value = value.Trim();

            // Base64 strings must be a multiple of 4
            if (value.Length % 4 != 0)
            {
                return false;
            }

            try
            {
                _ = Convert.FromBase64String(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToBase64(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(value);

            return Convert.ToBase64String(plainTextBytes);
        }

        public static string FromBase64(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (!IsBase64String(value))
            {
                return string.Empty;
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(value);

            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string SplitCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            string result = System.Text.RegularExpressions.Regex.Replace(value, @"(\p{Ll})(\p{Lu})", "$1 $2");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"(\p{Lu}+)(\p{Lu}\p{Ll})", "$1 $2");
            return result;
        }
    }
}
