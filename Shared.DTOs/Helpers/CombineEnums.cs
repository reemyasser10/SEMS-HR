using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enums.SeedDataEnum;

namespace Shared.DTOs.Helpers
{
    public static class CombineEnums
    {
        public static int GetCombinedCode<TStatus>(LookupEntityTypeEnum entityType, TStatus status) where TStatus : Enum
        {
          
            return int.Parse($"{(int)entityType}{Convert.ToInt32(status)}");
        }
    }
}
