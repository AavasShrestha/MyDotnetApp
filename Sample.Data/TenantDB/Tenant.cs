using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data.TenantDB
{
    public class Tenant
    {
        public Guid TenantId { get; set; }
        public string TenantKey { get; set; }
        public string DatabaseName { get; set; }
        public bool IsActive { get; set; }
    }
}
