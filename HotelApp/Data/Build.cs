using HotelApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelApp.Controllers
{
    public class Build
    {
        public AppDbContext BuildApp()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);


            using (var dbContext = new AppDbContext(options.Options))
            {
                var dataInitializer = new DataInitializer();
                dataInitializer.MigrateAndSeed(dbContext);
            }

            var returnedDbContext = new AppDbContext(options.Options);
            return returnedDbContext;
        }
    }
}