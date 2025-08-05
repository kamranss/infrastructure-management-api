using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence.Configuration;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MaintenanceDbContext>
    //{
    //    MaintenanceDbContext IDesignTimeDbContextFactory<MaintenanceDbContext>.CreateDbContext(string[] args)
    //    {
    //        DbContextOptionsBuilder<MaintenanceDbContext> dbContextOptionsBuilder = new();
    //        dbContextOptionsBuilder.UseNpgsql(DbConfiguration.ConnectionString);
    //        return new(dbContextOptionsBuilder.Options);
    //    }

    //}


    namespace Persistence
    {
        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MaintenanceDbContext>
        {
            public MaintenanceDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../infrastructure_management_app"))
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddUserSecrets(typeof(DesignTimeDbContextFactory).Assembly, optional: true) // ✅ This allows secrets to load
                    .Build();

                var builder = new DbContextOptionsBuilder<MaintenanceDbContext>();

                var connectionString = configuration.GetConnectionString("PostgreSQL");

                builder.UseNpgsql(connectionString, b => b.MigrationsAssembly("infrastructure_management_app"));

                return new MaintenanceDbContext(builder.Options);
            }
        }
    }

}
