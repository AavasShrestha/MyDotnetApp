using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sample.Data
{
    public class RoutingDbContextFactory : IDesignTimeDbContextFactory<RoutingDbContext>
    {
        public RoutingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RoutingDbContext>();

            // Use your connection string from appsettings.json
            optionsBuilder.UseSqlServer("Server=DESKTOP-9ECDUQE\\SQLEXPRESS;Database=RoutingDb;User Id=sa;Password=test123;TrustServerCertificate=True;");

            return new RoutingDbContext(optionsBuilder.Options);
        }
    }
}
