using HotelApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HotelApp.Controllers
{
    public class DataInitializer
    {
        public void MigrateAndSeed(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
            //Seed();
            dbContext.SaveChanges();
        }

        //private void Seed()
        //{
        //    throw new NotImplementedException();
        //}
    }
}