using Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DBModels.Common
{
    public class LangEntity<T> : BaseEntity
    {
        [DisplayName(nameof(Source))]
        [ForeignKey(nameof(Source))]
        public int Fk_Source { get; set; }

        [DisplayName(nameof(Source))]
        public T Source { get; set; }
    }
}
