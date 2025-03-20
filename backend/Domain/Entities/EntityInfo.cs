namespace Domain.Entities
{
    /// <summary>
    /// Информация о юридическом или физическом лице
    /// </summary>
    public class EntityInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string TaxCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // Тип сущности: "juridical" или "physical"
        public string EntityType { get; set; }
    }

    /// <summary>
    /// Результат проверки ИНН
    /// </summary>
    public class InnCheckResult
    {
        public bool Exists { get; set; }
        public EntityInfo EntityInfo { get; set; }
    }
}