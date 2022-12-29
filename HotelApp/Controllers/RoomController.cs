using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HotelApp.Controllers
{
    public class RoomController
    {
        private AppDbContext dbContext { get; set; }
        public RoomController(AppDbContext context)
        {
            dbContext = context;
        }
        public void CreateRoom()
        {
            Console.Clear();

            Console.WriteLine("REGISTER a new Room");

            Console.Write("\nEnter room type (Single/Double): ");
            var newRoomType = Console.ReadLine();

            Console.Write("\nEnter number of beds: ");
            int.TryParse(Console.ReadLine(), out var newRoomBeds);

            Console.Write("\nEnter room type area in (m2): ");
            int.TryParse(Console.ReadLine(), out var newRoomSize);

            dbContext.Rooms.Add(new Room
            {
                Type = newRoomType,
                Beds = newRoomBeds,
                Size = newRoomSize,
            });

            dbContext.SaveChanges();
        }
        public void ShowAllRooms()
        {
            Console.WriteLine("\nNumber\tType\tBeds\tSize");
            foreach (var room in dbContext.Rooms.ToList())
                Console.WriteLine($"{room.RoomId}\t{room.Type}\t{room.Beds}\t{room.Size}");
            System.Threading.Thread.Sleep(5000);
        }
        public void EditRoom()
        {
            ShowAllRooms();

            Console.WriteLine("\nEnter room number to EDIT: ");
            int.TryParse(Console.ReadLine(), out var roomIdToEdit);

            var roomToEdit = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomIdToEdit);

            Console.Write("\nEnter new type (Single/Double): ");
            roomToEdit.Type = Console.ReadLine();
            Console.Write("\nEnter number of beds: ");
            roomToEdit.Beds = Convert.ToInt32(Console.ReadLine());
            Console.Write("\nEnter the room size: ");
            roomToEdit.Size = Convert.ToInt32(Console.ReadLine());

            dbContext.SaveChanges();
        }

        public void DeleteRoom()
        {
            ShowAllRooms();

            Console.WriteLine("\nEnter room number to DELETE: ");
            int.TryParse(Console.ReadLine(), out var roomIdToDelete);

            var roomToDelete = dbContext.Rooms.First(r => r.RoomId == roomIdToDelete);

            if (dbContext.Bookings.Include(b => b.Room).Any(b => b.Room.RoomId == roomToDelete.RoomId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThis room can not be deleted because it has an associated booking.");
            }
            else
            {
                dbContext.Rooms.Remove(roomToDelete);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The Room {roomToDelete.RoomId} is deleted.");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();

            dbContext.SaveChanges();
        }
    }
}
