using HotelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data
{
    public class DataInitializer
    {
        public void MigrateAndSeed(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
            SeedRooms(dbContext);
            SeedGuests(dbContext);
            dbContext.SaveChanges();
        }
        public void SeedRooms(AppDbContext dbContext)
        {
            if (!dbContext.Rooms.Any(r => r.RoomId == 1))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = "Single",
                    Beds = 1,
                    Size = 37
                });
            }
            if (!dbContext.Rooms.Any(r => r.RoomId == 2))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = "Single",
                    Beds = 1,
                    Size = 37
                });
            }
            if (!dbContext.Rooms.Any(r => r.RoomId == 3))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = "Double",
                    Beds = 2,
                    Size = 45
                });
            }
            if (!dbContext.Rooms.Any(r => r.RoomId == 4))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = "Double",
                    Beds = 2,
                    Size = 60
                });
            }
        }
        public void SeedGuests(AppDbContext dbContext)
        {
            if (!dbContext.Guests.Any(g => g.Id == 1))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Steve Smith",
                    Age = 30,
                    Phone = "123-456-7894",
                    Address = "Test Road 1"
                }); ;
            }
            if (!dbContext.Guests.Any(g => g.Id == 2))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Glen Mcgrath",
                    Age = 20,
                    Phone = "128-656-6891",
                    Address = "Demo Road 7"
                });
            }
            if (!dbContext.Guests.Any(g => g.Id == 3))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Adam Gilchrist",
                    Age = 20,
                    Phone = "928-657-3002",
                    Address = "Main Boulv. 11"
                });
            }
            if (!dbContext.Guests.Any(g => g.Id == 4))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Ricky Ponting",
                    Age = 20,
                    Phone = "628-676-1009",
                    Address = "Super Street 89"
                });
            }
        }
    }
}