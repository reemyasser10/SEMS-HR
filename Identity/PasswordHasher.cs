using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        using HMACSHA512 hmac = new();
        byte[] saltBytes = hmac.Key;
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        passwordSalt = Convert.ToBase64String(saltBytes);
        passwordHash = Convert.ToBase64String(hashBytes);
    }
    public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.", nameof(password));
        }

        if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
        {
            throw new ArgumentException("Stored hash or salt cannot be empty.");
        }

        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        byte[] storedHashBytes = Convert.FromBase64String(storedHash);

        using HMACSHA512 hmac = new(saltBytes);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return CryptographicEquals(computedHash, storedHashBytes);
    }
    public static string CreateCodeHash(string code)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code));
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyCodeHash(string code, string storedHash)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code cannot be empty.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(storedHash))
        {
            throw new ArgumentException("Stored hash cannot be empty.");
        }

        string computedHash = CreateCodeHash(code);
        return storedHash == computedHash;
    }



    private static bool CryptographicEquals(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        int diff = 0;
        for (int i = 0; i < a.Length; i++)
        {
            diff |= a[i] ^ b[i];  // Prevents timing attacks
        }
        return diff == 0;
    }
}
