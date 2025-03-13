using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security
{
    /// <summary>
    /// Класс для регистрации конвертера шифрования при запуске приложения
    /// </summary>
    public static class DapperEncryptionSetup
    {
        public static void AddDapperEncryption(this IServiceCollection services)
        {
            // Настройка Dapper для использования нашего конвертера
            SqlMapper.AddTypeHandler(new EncryptedStringTypeHandler());
        }
    }

    /// <summary>
    /// Обработчик типов для Dapper, который автоматически шифрует/расшифровывает строки
    /// с атрибутом [Encrypted]
    /// </summary>
    public class EncryptedStringTypeHandler : SqlMapper.TypeHandler<string>
    {
        private static IEncryptionService _encryptionService;
        private static readonly Dictionary<Type, List<PropertyInfo>> _encryptedProperties = new Dictionary<Type, List<PropertyInfo>>();

        public static void Initialize(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public override string Parse(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            var rawValue = value.ToString();
            
            // Проверяем, требуется ли расшифровка
            if (IsEncryptedProperty())
            {
                return _encryptionService.Decrypt(rawValue);
            }

            return rawValue;
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, string value)
        {
            // Проверяем, требуется ли шифрование
            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else if (IsEncryptedProperty())
            {
                parameter.Value = _encryptionService.Encrypt(value);
            }
            else
            {
                parameter.Value = value;
            }
        }

        private bool IsEncryptedProperty()
        {
            // В реальной реализации необходимо проверить контекст выполнения
            // и определить, является ли текущее свойство помеченным атрибутом [Encrypted]
            // Это упрощенная версия для демонстрации
            
            // TODO: Реализовать проверку наличия атрибута [Encrypted] для текущего свойства
            return false;
        }
        
        // Метод для сканирования модели и поиска свойств с атрибутом [Encrypted]
        public static void ScanModelForEncryptedProperties(Type modelType)
        {
            if (_encryptedProperties.ContainsKey(modelType))
                return;
                
            var properties = modelType.GetProperties()
                .Where(p => p.PropertyType == typeof(string) && 
                            p.GetCustomAttribute<EncryptedAttribute>() != null)
                .ToList();
                
            _encryptedProperties[modelType] = properties;
        }
    }
}