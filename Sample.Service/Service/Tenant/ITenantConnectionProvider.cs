using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Service.Tenant
{
    public interface ITenantConnectionProvider
    {
        /// <summary>
        /// Returns the connection string for the tenant identified by tenantKey.
        /// Example tenantKey: "KAMANA" or "ICONSOFT"
        /// </summary>
        /// <param name="tenantKey">Unique tenant identifier</param>
        /// <returns>SQL Server connection string for the tenant database</returns>
        Task<string> GetConnectionStringAsync(string tenantKey);
    }
}
