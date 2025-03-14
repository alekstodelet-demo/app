using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Infrastructure.Security
{
    public interface ITwoFactorService
    {
        string GenerateSecretKey();
        string GenerateTotpCode(string secretKey);
        bool ValidateTotpCode(string secretKey, string totpCode);
        string GetQrCodeUri(string email, string secretKey, string issuer);
    }

    public class TwoFactorService : ITwoFactorService
    {
        private readonly TwoFactorOptions _options;
        
        public TwoFactorService(IOptions<TwoFactorOptions> options)
        {
            _options = options.Value;
        }
        
        public string GenerateSecretKey()
        {
            var bytes = new byte[20]; // 20 байт = 160 бит
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            
            return Base32Encode(bytes);
        }
        
        public string GenerateTotpCode(string secretKey)
        {
            var counter = GetCurrentCounter();
            return GenerateCodeAtCounter(secretKey, counter);
        }
        
        public bool ValidateTotpCode(string secretKey, string totpCode)
        {
            if (string.IsNullOrEmpty(totpCode) || string.IsNullOrEmpty(secretKey))
                return false;
                
            // Получаем текущий счетчик
            var counter = GetCurrentCounter();
            
            // Проверяем текущий и соседние интервалы времени (для учета сдвига часов)
            for (int i = -1; i <= 1; i++)
            {
                var code = GenerateCodeAtCounter(secretKey, counter + i);
                if (string.Equals(code, totpCode, StringComparison.Ordinal))
                    return true;
            }
            
            return false;
        }
        
        public string GetQrCodeUri(string email, string secretKey, string issuer)
        {
            var encodedIssuer = Uri.EscapeDataString(issuer);
            var encodedEmail = Uri.EscapeDataString(email);
            
            return $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={secretKey}&issuer={encodedIssuer}&digits=6&period=30";
        }
        
        private long GetCurrentCounter()
        {
            // Текущее время в секундах с эпохи Unix
            var unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            // Количество 30-секундных интервалов с эпохи Unix
            return unixTime / 30;
        }
        
        private string GenerateCodeAtCounter(string secretKey, long counter)
        {
            // Декодируем секретный ключ из Base32
            var keyBytes = Base32Decode(secretKey);
            
            // Преобразуем счетчик в байтовый массив (big-endian)
            var counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(counterBytes);
                
            // Хеширование с использованием HMAC-SHA1
            using (var hmac = new HMACSHA1(keyBytes))
            {
                var hash = hmac.ComputeHash(counterBytes);
                
                // Получение смещения для динамического усечения
                int offset = hash[hash.Length - 1] & 0xf;
                
                // Преобразование 4 байт, начиная с offsetBytes, в 32-битное целое число
                int truncatedHash = ((hash[offset] & 0x7f) << 24) |
                                    ((hash[offset + 1] & 0xff) << 16) |
                                    ((hash[offset + 2] & 0xff) << 8) |
                                    (hash[offset + 3] & 0xff);
                
                // Получение 6-значного кода
                int code = truncatedHash % 1000000;
                
                // Возвращение кода с ведущими нулями
                return code.ToString("D6");
            }
        }
        
        private static string Base32Encode(byte[] data)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var bits = 0;
            var bitsRemaining = 0;
            var result = new StringBuilder();
            
            foreach (var b in data)
            {
                bits = (bits << 8) | b;
                bitsRemaining += 8;
                
                while (bitsRemaining >= 5)
                {
                    bitsRemaining -= 5;
                    var index = (bits >> bitsRemaining) & 31;
                    result.Append(alphabet[index]);
                }
            }
            
            if (bitsRemaining > 0)
            {
                bits = bits << (5 - bitsRemaining);
                var index = bits & 31;
                result.Append(alphabet[index]);
            }
            
            return result.ToString();
        }
        
        private static byte[] Base32Decode(string encoded)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            encoded = encoded.Trim().ToUpperInvariant();
            
            // Удаляем заполнение (padding)
            encoded = encoded.TrimEnd('=');
            
            var result = new List<byte>();
            var bits = 0;
            var bitsRemaining = 0;
            
            foreach (var c in encoded)
            {
                var value = alphabet.IndexOf(c);
                if (value < 0)
                    throw new FormatException("Invalid Base32 character.");
                    
                bits = (bits << 5) | value;
                bitsRemaining += 5;
                
                if (bitsRemaining >= 8)
                {
                    bitsRemaining -= 8;
                    result.Add((byte)((bits >> bitsRemaining) & 0xff));
                }
            }
            
            return result.ToArray();
        }
    }
    
    public class TwoFactorOptions
    {
        public int CodeDigits { get; set; } = 6;
        public int PeriodSeconds { get; set; } = 30;
        public string Issuer { get; set; } = "BGA-Application";
    }
}