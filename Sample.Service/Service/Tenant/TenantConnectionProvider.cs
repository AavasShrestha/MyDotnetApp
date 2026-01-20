using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Service.Service.Tenant
{
    public class TenantConnectionProvider : ITenantConnectionProvider
    {
        private readonly MasterDbContext _masterDb;

        public TenantConnectionProvider(MasterDbContext masterDb)
        {
            _masterDb = masterDb ?? throw new ArgumentNullException(nameof(masterDb));
        }

        public async Task<string> GetConnectionStringAsync(string tenantKey)
        {
            var tenant = await _masterDb.Tenants
                .Where(t => t.TenantKey == tenantKey && t.IsActive)
                .FirstOrDefaultAsync();

            if (tenant == null)
                throw new Exception($"Tenant '{tenantKey}' not found or inactive.");

            // Use your fixed SQL Server settings here
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = @"DESKTOP-9ECDUQE\SQLEXPRESS", // fixed server
                InitialCatalog = tenant.DatabaseName,       // tenant-specific DB
                UserID = "sa",                              // fixed username
                Password = "test123",                       // fixed password
                TrustServerCertificate = true,
                Encrypt = true
            };

            return builder.ConnectionString;
        }
    }
}
