using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetArchObjectResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("address")]
        public string? Address { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("identifier")]
        public string? Identifier { get; set; }
        
        [JsonProperty("district_id")]
        public int? DistrictId { get; set; }
        
        [JsonProperty("district_name")]
        public string? DistrictName { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("x_coordinate")]
        public double? XCoordinate { get; set; }
        
        [JsonProperty("y_coordinate")]
        public double? YCoordinate { get; set; }

        internal static GetArchObjectResponse FromDomain(Domain.Entities.ArchObject domain)
        {
            return new GetArchObjectResponse
            {
                Id = domain.Id,
                Address = domain.Address,
                Name = domain.Name,
                Identifier = domain.Identifier,
                DistrictId = domain.DistrictId,
                DistrictName = domain.DistrictName,
                Description = domain.Description,
                XCoordinate = domain.XCoordinate,
                YCoordinate = domain.YCoordinate
            };
        }
    }

    public class CreateArchObjectRequest
    {
        public string? Address { get; set; }
        public string? Name { get; set; }
        public string? Identifier { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? Description { get; set; }
        public double? XCoordinate { get; set; }
        public double? YCoordinate { get; set; }

        internal Domain.Entities.ArchObject ToDomain()
        {
            return new Domain.Entities.ArchObject
            {
                Address = Address,
                Name = Name,
                Identifier = Identifier,
                DistrictId = DistrictId,
                DistrictName = DistrictName,
                Description = Description,
                XCoordinate = XCoordinate,
                YCoordinate = YCoordinate
            };
        }
    }

    public class UpdateArchObjectRequest
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public string? Identifier { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? Description { get; set; }
        public double? XCoordinate { get; set; }
        public double? YCoordinate { get; set; }
        internal Domain.Entities.ArchObject ToDomain()
        {
            return new Domain.Entities.ArchObject
            {
                Id = Id,
                Address = Address,
                Name = Name,
                Identifier = Identifier,
                DistrictId = DistrictId,
                DistrictName = DistrictName,
                Description = Description,
                XCoordinate = XCoordinate,
                YCoordinate = YCoordinate
            };
        }
    }
}