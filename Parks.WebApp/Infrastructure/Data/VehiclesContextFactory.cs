using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Parks.WebApp.Data;
using System.IO;

namespace Parks.WebApp.Infrastructure.Data
{
    public class VehiclesContextFactory: IDesignTimeDbContextFactory<VehicleDbContext>
    {
        public VehicleDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<VehicleDbContext> ();

            var ConnectionString = configuration.GetConnectionString("VehiclesConnection");

            builder.UseSqlServer(ConnectionString);

            return new VehicleDbContext(builder.Options);
        }
    }
}
