using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTable;

namespace Shared.VMs.Common
{
    public class TranslationFilter : DtParameters
    {
        public string? LanguageCode { get; set; }
        public ApplicationEnum? ApplicationEnum { get; set; }


    }
}
