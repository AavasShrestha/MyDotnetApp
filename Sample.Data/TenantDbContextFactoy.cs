using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Sample.Data
{
    public class TenantDbContextDesignFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        public TenantDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));

            return new TenantDbContext(optionsBuilder.Options);
        }
    }
}
