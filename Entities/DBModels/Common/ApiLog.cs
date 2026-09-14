using Entities.Constants;
using Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Common
{
    [Table(nameof(ApiLog), Schema = SchemaNames.Common)]
    public class ApiLog : TenantBaseEntity
    {
        public required string HttpMethod { get; set; }

        public required string Url { get; set; }

        public string? RequestBody { get; set; }

        public string? ResponseBody { get; set; }

        public int StatusCode { get; set; }
    }
}
