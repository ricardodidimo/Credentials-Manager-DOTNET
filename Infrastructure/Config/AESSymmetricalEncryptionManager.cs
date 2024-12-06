using Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Config
{
    public class AESSymmetricalEncryptionManager : ISymmetricEncryptionManager
    {
        private IConfiguration configuration;
        public AESSymmetricalEncryptionManager()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "InMemoryKey", "InMemoryValue" }
                });

            this.configuration = configurationBuilder.Build();
        }

        public string Decrypt(byte[] cipherText)
        {
            string? key = configuration["ENCRYPTION_KEY"] ?? throw new ArgumentNullException(nameof(key));
            string? IV = configuration["ENCRYPTION_IV"] ?? throw new ArgumentNullException(nameof(IV));

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        public byte[] Encrypt(string plainText)
        {
            string? key = configuration["ENCRYPTION_KEY"] ?? throw new ArgumentNullException(nameof(key));
            string? IV = configuration["ENCRYPTION_IV"] ?? throw new ArgumentNullException(nameof(IV));

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return msEncrypt.ToArray();
        }
    }
}
