using HotelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data
{
    public class DataInitializer
    {
        public void MigrateAndSeed(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
            SeedRoomTypes(dbContext);
            SeedRooms(dbContext);
            SeedGuests(dbContext);
            dbContext.SaveChanges();
        }
        public void SeedRoomTypes(AppDbContext dbContext)
        {
            if (!dbContext.RoomTypes.Any(rt => rt.Id == "Single"))
            {
                dbContext.RoomTypes.Add(new RoomType
                {
                    Id = "Single",
                    Bed = 1
                });
            }
            if (!dbContext.RoomTypes.Any(rt => rt.Id == "Double"))
            {
                dbContext.RoomTypes.Add(new RoomType
                {
                    Id = "Double",
                    Bed = 2
                });
            }

            dbContext.SaveChanges();
        }
        public void SeedRooms(AppDbContext dbContext)
        {           

            if (!dbContext.Rooms.Any(r => r.RoomId == 1))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = dbContext.RoomTypes.First(rt=> rt.Id == "Single"),
                    Size = 37
                });
            }
            if (!dbContext.Rooms.Any(r => r.RoomId == 2))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = dbContext.RoomTypes.First(rt => rt.Id == "Single"),
                    Size = 40
                });
            }
            if (!dbContext.Rooms.Any(r => r.RoomId == 3))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = dbContext.RoomTypes.First(rt => rt.Id == "Double"),
                    Size = 45
                });
            }
            if (!dbContext.Rooms.Any(r => r.RoomId == 4))
            {
                dbContext.Rooms.Add(new Room
                {
                    Type = dbContext.RoomTypes.First(rt => rt.Id == "Double"),
                    Size = 60
                });
            }
        }
        public void SeedGuests(AppDbContext dbContext)
        {
            if (!dbContext.Guests.Any(g => g.GuestId == 1))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Steve Smith",
                    Age = 30,
                    Phone = "123-456-7894",
                    Street = "Test Road 1",
                    City = "Rockley",
                    PostalCode = 12345
                });
            }
            if (!dbContext.Guests.Any(g => g.GuestId == 2))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Glen Mcgrath",
                    Age = 20,
                    Phone = "128-656-6891",
                    Street = "Demo Road 7",
                    City = "Chatford",
                    PostalCode = 67890
                });
            }
            if (!dbContext.Guests.Any(g => g.GuestId == 3))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Adam Gilchrist",
                    Age = 20,
                    Phone = "928-657-3002",
                    Street = "Main Boulv. 11",
                    City = "Transview",
                    PostalCode = 14725
                });
            }
            if (!dbContext.Guests.Any(g => g.GuestId == 4))
            {
                dbContext.Guests.Add(new Guest
                {
                    Name = "Ricky Ponting",
                    Age = 20,
                    Phone = "628-676-1009",
                    Street = "Super Street 89",
                    City = "Elside",
                    PostalCode = 25836
                });
            }
        }
    }
}