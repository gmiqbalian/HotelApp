using HotelApp.Controllers;
using HotelApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelApp
{
    public class App
    {
        public void Run()
        {
            var builder = new Build();
            var dbContext = builder.BuildApp();

            using (dbContext)
            {
                foreach (var guest in dbContext.Guests)
                {
                    Console.WriteLine(guest);
                }
            }
            

        }
    }
}
