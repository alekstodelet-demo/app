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
        private static readonly AsyncLocal<ExecutionContext> _currentContext = new AsyncLocal<ExecutionContext>();

        public static void Initialize(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
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
            Console.WriteLine($"Encrypting value: {value}"); // Отладочная печать
    
            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else if (IsEncryptedProperty())
            {
                try 
                {
                    var encryptedValue = _encryptionService.Encrypt(value);
                    Console.WriteLine($"Encrypted value: {encryptedValue}");
                    parameter.Value = encryptedValue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Encryption error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                parameter.Value = value;
            }
        }

        private bool IsEncryptedProperty()
        {
            var context = _currentContext.Value;
            
            Console.WriteLine($"Context is null: {context == null}");
            
            if (context == null)
                return false;
        
            // Get properties for the model if not already cached
            if (!_encryptedProperties.TryGetValue(context.ModelType, out var properties))
            {
                ScanModelForEncryptedProperties(context.ModelType);
                properties = _encryptedProperties[context.ModelType];
            }
    
            // Check if the current property name matches any encrypted property
            return properties.Any(p => p.Name == context.PropertyName);
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

        public static IDisposable SetExecutionContext(Type type, string propertyName)
        {
            var context = new ExecutionContext
            {
                ModelType = type,
                PropertyName = propertyName
            };
    
            _currentContext.Value = context;
            return context;
        }
        
        private class ExecutionContext : IDisposable
        {
            public Type ModelType { get; set; }
            public string PropertyName { get; set; }
    
            public void Dispose()
            {
                _currentContext.Value = null;
            }
        }

    }
}