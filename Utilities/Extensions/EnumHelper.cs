using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Extensions
{
    public static class EnumHelper
    {
        public static Dictionary<string, List<EnumDto>> GetAllEnums(params Type[] enumTypes)
        {
            var result = new Dictionary<string, List<EnumDto>>();

            foreach (var type in enumTypes)
            {
                if (!type.IsEnum) continue;

                var values = Enum.GetValues(type)
                    .Cast<Enum>()
                    .Select(e => new EnumDto
                    {
                        Name = e.ToString(),
                        Value = Convert.ToInt32(e)
                    })
                    .ToList();

                result[type.Name] = values;
            }

            return result;
        }
    }

    public class EnumDto
    {
        public required string Name { get; set; }
        public int Value { get; set; }
    }
}
