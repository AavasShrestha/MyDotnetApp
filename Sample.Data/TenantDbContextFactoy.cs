using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Data
{
    public interface ITenantDbContextFactory
    {
        TenantDbContext Create(string connectionString);
    }

    public class TenantDbContextFactory : ITenantDbContextFactory
    {
        public TenantDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new TenantDbContext(optionsBuilder.Options);
        }
    }
}
