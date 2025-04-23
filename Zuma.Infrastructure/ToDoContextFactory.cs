using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Zuma.Infrastructure.Context;

namespace Infrastructure
{
    public class ToDoContextFactory : IDesignTimeDbContextFactory<ToDoContext>
    {
        public ToDoContext CreateDbContext(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ZumaProj.Api"))
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ToDoContext>();
            var connectionString = "Server=.;Database=Zuma;Trusted_Connection=True;TrustServerCertificate=true";

            optionsBuilder.UseSqlServer(connectionString);

            return new ToDoContext(optionsBuilder.Options);
        }
    }
}
