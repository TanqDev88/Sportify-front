using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Sportify_back.Models
{
    public class SportifyDbContextFactory : IDesignTimeDbContextFactory<SportifyDbContext>
    {
        public SportifyDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SportifyDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("AppConnectionString"));

            return new SportifyDbContext(optionsBuilder.Options);
        }
    }
}
