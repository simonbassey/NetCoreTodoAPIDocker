using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
namespace TodoDockerAPI.Data.Core
{
    public static class TodoDbInitializer
    {
        public static async Task Seed(TodoDbContext dbContext)
        {
            try
            {
                var pendingMigration = await dbContext.Database.GetAppliedMigrationsAsync();
                if (pendingMigration.Any())
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"DbInitialization Failed - {exception}");
            }
        }
    }
}
