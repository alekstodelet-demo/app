using System.ComponentModel.DataAnnotations;

namespace Services.Microservice.Dtos
{
    /// <summary>
    /// DTO для передачи информации о сервисе
    /// </summary>
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? DayCount { get; set; }
        public int? WorkflowId { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }

    /// <summary>
    /// DTO для создания нового сервиса
    /// </summary>
    public class CreateServiceRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        
        [StringLength(20)]
        public string ShortName { get; set; }
        
        [Required]
        [StringLength(10, MinimumLength = 1)]
        [RegularExpression("^[A-Z0-9_]*$")]
        public string Code { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        
        [Range(0, int.MaxValue)]
        public int? DayCount { get; set; }
        
        public int? WorkflowId { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }
        
        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO для обновления существующего сервиса
    /// </summary>
    public class UpdateServiceRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
        
        [StringLength(20)]
        public string ShortName { get; set; }
        
        [Required]
        [StringLength(10, MinimumLength = 1)]
        [RegularExpression("^[A-Z0-9_]*$")]
        public string Code { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
        
        [Range(0, int.MaxValue)]
        public int? DayCount { get; set; }
        
        public int? WorkflowId { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }
        
        public bool? IsActive { get; set; }
    }
}