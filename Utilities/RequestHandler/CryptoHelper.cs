using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Utilities.RequestHandler
{
    public class CryptoService
    {
        private readonly byte[] _key;

        public CryptoService(IConfiguration config)
        {
            string? keyString = config["Crypto:AESKey"];
            if (string.IsNullOrWhiteSpace(keyString) || (keyString.Length != 16 && keyString.Length != 24 && keyString.Length != 32))
            {
                throw new ArgumentException("AES key must be 16, 24, or 32 characters long.");
            }

            _key = Encoding.UTF8.GetBytes(keyString);
        }

        public string EncryptObject(object obj)
        {
            string json = JsonSerializer.Serialize(obj);
            using Aes aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();

            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using MemoryStream ms = new();
            ms.Write(aes.IV, 0, aes.IV.Length);
            using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
            using (StreamWriter sw = new(cs))
            {
                sw.Write(json);
            }

            return Convert.ToBase64String(ms.ToArray())
                          .Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        public T DecryptToObject<T>(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new ArgumentNullException(nameof(encryptedText));
            }

            string base64 = encryptedText.Replace("-", "+").Replace("_", "/");
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            byte[] fullCipher = Convert.FromBase64String(base64);
            using Aes aes = Aes.Create();
            aes.Key = _key;

            byte[] iv = new byte[16];
            Array.Copy(fullCipher, iv, 16);
            aes.IV = iv;

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            using MemoryStream ms = new(fullCipher, 16, fullCipher.Length - 16);
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            string json = sr.ReadToEnd();

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}