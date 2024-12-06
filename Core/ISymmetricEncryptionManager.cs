namespace Core
{
    public interface ISymmetricEncryptionManager
    {
        public byte[] Encrypt(string plainText);
        public string Decrypt(byte[] cipherText);
    }
}
