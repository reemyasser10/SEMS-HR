using Entities.DBModels.Common;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Entities.Enums.SeedDataEnum;

namespace Data.Configurations
{
    public class LookupConfiguration : IEntityTypeConfiguration<Lookup>
    {
        public void Configure(EntityTypeBuilder<Lookup> builder)
        {
            _ = builder.HasData(GenerateLookupData<GenderEnum>(LookupEntityTypeEnum.Gender));
           
        }

        private List<Lookup> GenerateLookupData<TEnum>(LookupEntityTypeEnum entityType) where TEnum : Enum
        {
            int baseId = (int)entityType;
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                      .Select((value, index) => new Lookup
                      {
                          Id = int.Parse($"{baseId}{Convert.ToInt32(value)}"), // Unique ID per entity type
                          EntityId = Convert.ToInt32(value),
                          EntityType = entityType,
                          Name = value.ToString(),
                      }).ToList();
        }
    }

}
