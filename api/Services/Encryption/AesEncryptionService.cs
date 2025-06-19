using System.Security.Cryptography;
using System.Text;

namespace Adventour.Api.Services.Encryption
{
    public class AesEncryptionService : IEncryptionService
    {
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();

            string base64Key = Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY");
            string base64IV = Environment.GetEnvironmentVariable("AES_INIT_VECTOR");

            aes.Key = Convert.FromBase64String(base64Key).Take(32).ToArray();
            aes.IV = Convert.FromBase64String(base64IV).Take(16).ToArray();

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
            aes.Key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY"));
            aes.IV = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AES_INIT_VECTOR"));

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
