using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.HR.PermissionModels
{
    public class UserPermissionCacheDto
    {
        public HashSet<string>? FunctionalityCodes { get; set; }
    }

}
