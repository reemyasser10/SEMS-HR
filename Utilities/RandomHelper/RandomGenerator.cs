using System.Security.Cryptography;

namespace Utilities.RandomHelper
{
    public static class RandomGenerator
    {
        public static byte[] GenerateBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static int GenerateInteger(int length, int minVal, int maxVal)
        {
            byte[] randomBytes = GenerateBytes(length);

            int random = Math.Abs(BitConverter.ToInt32(randomBytes, 0));
            return Convert.ToInt32((random % (maxVal - minVal + 1)) + minVal);
        }
        public static string GenerateStringAsInteger(int length, int minVal, int maxVal) =>
          GenerateInteger(length, minVal, maxVal).ToString();

        public static string GenerateString(int length, string? allowableChars = null)
        {
            if (string.IsNullOrWhiteSpace(allowableChars))
            {
                allowableChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            }

            // Generate random data
            byte[] randomBytes = GenerateBytes(length);

            // Generate the output string
            char[] allowable = allowableChars.ToCharArray();
            int l = allowable.Length;
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowable[randomBytes[i] % l];
            }

            return new string(chars);
        }
    }
}
