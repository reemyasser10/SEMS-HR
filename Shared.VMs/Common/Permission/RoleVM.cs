using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTable;

namespace Shared.VMs.Common.Permission
{
    public class RoleFilter : DtParameters
    {
        public bool? IsActive { get; set; }
    }
}
