using FluentResults;
using FluentValidation.Results;
using System.Security.Cryptography;

namespace Core
{
    public class Common
    {
        public static readonly string LACK_OWNERSHIP_ERR = "unauthorized action";
        public static readonly string VAULT_REFERENCE_NOT_FOUND_ERR = "unable to find referenced vault";
        public static readonly string CREDENTIALS_REFERENCE_NOT_FOUND_ERR = "unable to find referenced credentials";
        public static readonly string CATEGORY_REFERENCE_NOT_FOUND_ERR = "unable to find referenced category";
        public static readonly string VAULT_ACCESS_DENIED_ERR = "wrong credentials for vault access";
        public static readonly string CREDENTIAL_ENCRYPT_ERR = "unable to encrypt credential pair";
        public static readonly string CREDENTIAL_DECRYPT_ERR = "unable to decrypt credential pair";

        public static List<IError> ToResultErrorList(List<ValidationFailure> results)
        {
            List<IError> errors = results.Select(e => (IError)new Error(e.ErrorMessage)).ToList();
            return errors;
        }


        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return msEncrypt.ToArray();
        }

        public static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }
}
