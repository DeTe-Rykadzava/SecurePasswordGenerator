using System.Security.Cryptography;
using System.Text;
using SecurePasswordManager.Core.Interfaces;

namespace SecurePasswordManager.Core.Services;

public class AesEncryptionService : IEncryptionService
{
     private const int KeySize = 256;
    private const int BlockSize = 128;
    private const int Iterations = 10000; // Рекомендуется минимум 10k для PBKDF2

    public string Decrypt(string cipherText, string password)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[BlockSize / 8];
        
        // Извлекаем IV из начала
        Array.Copy(cipherBytes, 0, iv, 0, iv.Length);

        using (var passwordDeriveBytes = new Rfc2898DeriveBytes(password, iv, Iterations, HashAlgorithmName.SHA256))
        {
            byte[] key = passwordDeriveBytes.GetBytes(KeySize / 8);

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    // Пропускаем байты IV при расшифровке
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, iv.Length, cipherBytes.Length - iv.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }

    public string Encrypt(string plainText, string password)
    {
        byte[] iv = new byte[BlockSize / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(iv);
        }

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (var passwordDeriveBytes = new Rfc2898DeriveBytes(password, iv, Iterations, HashAlgorithmName.SHA256))
        {
            byte[] key = passwordDeriveBytes.GetBytes(KeySize / 8);

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    // Сохраняем IV в начало массива байтов, он нужен будет при расшифровке
                    ms.Write(iv, 0, iv.Length); 
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
}
