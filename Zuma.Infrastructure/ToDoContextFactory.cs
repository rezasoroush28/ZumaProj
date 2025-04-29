using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Zuma.Infrastructure.Context
{
    public class ToDoContextFactory : IDesignTimeDbContextFactory<ToDoContext>
    {
        public ToDoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ToDoContext>();

            // اتصال مستقیم به SQL Server
            var connectionString = "Server=.;Database=Zuma;Trusted_Connection=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connectionString);

            return new ToDoContext(optionsBuilder.Options);
        }
    }
}
