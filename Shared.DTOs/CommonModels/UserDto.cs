using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.RequestHandler;

namespace Shared.DTOs.CommonModels
{
    public class UserParameters : RequestParameters
    {
        public string? Name { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsLockedOut { get; set; }

        public int Fk_Tenant { get; set; }
        public List<int> Ids { get; set; }
        public List<int>? Fk_Roles { get; set; }
        public bool GetAdministrators { get; set; }
    }
    public class CustomUserLookUpDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
