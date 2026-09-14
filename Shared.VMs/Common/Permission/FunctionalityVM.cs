using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTable;

namespace Shared.VMs.Common.Permission
{
    public class FunctionalityFilter : DtParameters
    {
        public bool? IsActive { get; set; }
        public int? Fk_Resource { get; set; }
    }
}
