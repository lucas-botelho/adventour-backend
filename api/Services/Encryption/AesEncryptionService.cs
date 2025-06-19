using System.Security.Cryptography;
using System.Text;

namespace Adventour.Api.Services.Encryption
{
    public class AesEncryptionService : IEncryptionService
    {
        private readonly string key;
        private readonly string iv;

        public AesEncryptionService(IConfiguration configuration)
        {
            var key = Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY") ?? "";
            var iv = Environment.GetEnvironmentVariable("AES_INIT_VECTOR") ?? "";
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
