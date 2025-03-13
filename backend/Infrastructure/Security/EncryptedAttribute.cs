using System;

namespace Infrastructure.Security
{
    /// <summary>
    /// Атрибут, указывающий, что поле должно быть зашифровано перед сохранением
    /// и расшифровано при чтении из базы данных
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EncryptedAttribute : Attribute
    {
    }
}