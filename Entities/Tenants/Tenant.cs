using Entities.Constants;
using Entities.Shared;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DBModels.Tenants
{
    [Table(nameof(Tenant), Schema = SchemaNames.Tenant)]
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Code), IsUnique = true)]
    public class Tenant : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new int Id { get; set; }

        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Domain { get; set; }
        public string? Description { get; set; }
    }
}
