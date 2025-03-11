using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetServiceResponse
    {
        [JsonProperty("id")] 
        public int Id { get; set; }
        [JsonProperty("name")] 
        public string? Name { get; set; }
        [JsonProperty("short_name")] 
        public string? ShortName { get; set; }
        [JsonProperty("code")] 
        public string? Code { get; set; }
        [JsonProperty("description")] 
        public string? Description { get; set; }
        [JsonProperty("day_count")] 
        public int? DayCount { get; set; }
        [JsonProperty("workflow_id")] 
        public int? WorkflowId { get; set; }
        [JsonProperty("price")] 
        public decimal? Price { get; set; }
        
        internal static GetServiceResponse FromDomain(Service domain)
        {
            return new GetServiceResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                ShortName = domain.ShortName,
                Code = domain.Code,
                Description = domain.Description,
                DayCount = domain.DayCount,
                WorkflowId = domain.WorkflowId,
                Price = domain.Price
            };
        }
    }

    public class CreateServiceRequest
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? DayCount { get; set; }
        public int? WorkflowId { get; set; }
        public decimal? Price { get; set; }
        
        internal Service ToDomain()
        {
            return new Service
            {
                Name = Name,
                ShortName = ShortName,
                Code = Code,
                Description = Description,
                DayCount = DayCount,
                WorkflowId = WorkflowId,
                Price = Price,
            };
        }
    }

    public class UpdateServiceRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? DayCount { get; set; }
        public int? WorkflowId { get; set; }
        public decimal? Price { get; set; }
        
        internal Service ToDomain()
        {
            return new Service
            {
                Id = Id,
                Name = Name,
                ShortName = ShortName,
                Code = Code,
                Description = Description,
                DayCount = DayCount,
                WorkflowId = WorkflowId,
                Price = Price,
            };
        }
    }
}
