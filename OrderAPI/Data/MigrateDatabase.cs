using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Data
{
    public class MigrateDatabase
    {
        public static void EnsureCreated(OrderContext context)
        {
            System.Console.WriteLine("Creating database...");
            context.Database.Migrate();


            System.Console.WriteLine("Database and tables' creation complete.....");
        }
    }
}
