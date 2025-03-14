using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService(IConfiguration configuration)
        {
            // Безопасное получение ключей из конфигурации
            var encryptionKey = configuration["Security:EncryptionKey"];
            var encryptionIv = configuration["Security:EncryptionIv"];

            if (string.IsNullOrEmpty(encryptionKey) || string.IsNullOrEmpty(encryptionIv))
            {
                throw new InvalidOperationException("Encryption key or IV is not configured properly.");
            }

            // Преобразование строковых ключей в байтовые массивы
            _key = Convert.FromBase64String(encryptionKey);
            _iv = Convert.FromBase64String(encryptionIv);
        }

        public string Encrypt(string plainText)
        {
            Console.WriteLine($"Attempting to encrypt: {plainText}");
    
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            try 
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;

                    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }

                        var encryptedValue = Convert.ToBase64String(ms.ToArray());
                        Console.WriteLine($"Encrypted value: {encryptedValue}");
                        return encryptedValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Encryption error: {ex.Message}");
                throw;
            }
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                var buffer = Convert.FromBase64String(cipherText);

                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;

                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (var ms = new MemoryStream(buffer))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO Логирование ошибки расшифровки, но не раскрытие деталей
                return "";
            }
        }
    }
}