using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Service.Service.Tenant
{
    public class TenantService
    {
        private readonly ITenantConnectionProvider _tenantConnectionProvider;

        public TenantService(ITenantConnectionProvider tenantConnectionProvider)
        {
            _tenantConnectionProvider = tenantConnectionProvider;
        }

        public async Task<IEnumerable<dynamic>> QueryTenantTableAsync(
            string tenantKey, string tableName, string whereClause = "")
        {
            var connString = await _tenantConnectionProvider.GetConnectionStringAsync(tenantKey);

            using var conn = new SqlConnection(connString);
            await conn.OpenAsync();

            var sql = $"SELECT * FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
                sql += $" WHERE {whereClause}";

            return await conn.QueryAsync(sql); // returns dynamic objects
        }
    }
}
