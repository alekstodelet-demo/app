using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetApplicationObjectResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("application_id")]
        public int ApplicationId { get; set; }
        
        [JsonProperty("arch_object_id")]
        public int ArchObjectId { get; set; }

        internal static GetApplicationObjectResponse FromDomain(Domain.Entities.ApplicationObject domain)
        {
            return new GetApplicationObjectResponse
            {
                Id = domain.Id,
                ApplicationId = domain.ApplicationId,
                ArchObjectId = domain.ArchObjectId
            };
        }
    }

    public class CreateApplicationObjectRequest
    {
        public int ApplicationId { get; set; }
        public int ArchObjectId { get; set; }

        internal Domain.Entities.ApplicationObject ToDomain()
        {
            return new Domain.Entities.ApplicationObject
            {
                ApplicationId = ApplicationId,
                ArchObjectId = ArchObjectId
            };
        }
    }

    public class UpdateApplicationObjectRequest
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ArchObjectId { get; set; }

        internal Domain.Entities.ApplicationObject ToDomain()
        {
            return new Domain.Entities.ApplicationObject
            {
                Id = Id,
                ApplicationId = ApplicationId,
                ArchObjectId = ArchObjectId
            };
        }
    }
}