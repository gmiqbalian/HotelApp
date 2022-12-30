using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ConsoleTables;
using Microsoft.Identity.Client;

namespace HotelApp.Controllers
{
    public class RoomController
    {
        private AppDbContext dbContext { get; set; }
        private List<Room> _availableRooms;
        public RoomController(AppDbContext context)
        {
            dbContext = context;
            _availableRooms = new List<Room>();
        }
        public void Create()
        {
            Console.Clear();

            Console.WriteLine("REGISTER a new Room");

            Console.Write("\nEnter room type (Single/Double): ");
            var newRoomType = Console.ReadLine();

            Console.Write("\nEnter room type area in (m2): ");
            int.TryParse(Console.ReadLine(), out var newRoomSize);

            var newRoomBeds = 0;    //assuming default no of beds for room type
            if (newRoomType.ToLower() == "single")
                newRoomBeds = 1;
            else
                newRoomBeds = 2;

            dbContext.Rooms.Add(new Room
            {
                Type = newRoomType,
                Beds = newRoomBeds,
                Size = newRoomSize,
            });

            dbContext.SaveChanges();

            Console.WriteLine("\nNew Room added successfully.");
            Console.WriteLine("\nPress any key to continue...");
        }
        public void ShowAll()
        {
            var table = new ConsoleTable("Number", "Type", "Beds", "Size");
            
            foreach (var room in dbContext.Rooms.ToList())
                table.AddRow(room.RoomId, room.Type, room.Beds, room.Size);                    
           
            table.Write();
            System.Threading.Thread.Sleep(5000);
        }
        public void Update()
        {
            ShowAll();

            Console.WriteLine("\nEnter room number to EDIT: ");
            int.TryParse(Console.ReadLine(), out var roomIdToEdit);

            var roomToEdit = dbContext.Rooms.
                FirstOrDefault(r => r.RoomId == roomIdToEdit);

            Console.Write("\nEnter new type (Single/Double): ");
            roomToEdit.Type = Console.ReadLine();

            Console.Write("\nChange number of beds to: ");
            roomToEdit.Beds = Convert.ToInt32(Console.ReadLine());

            Console.Write("\nEnter the room size: ");
            roomToEdit.Size = Convert.ToInt32(Console.ReadLine());

            dbContext.SaveChanges();
        }
        public void Delete()
        {
            ShowAll();

            Console.WriteLine("\nEnter room number to DELETE: ");
            int.TryParse(Console.ReadLine(), out var roomIdToDelete);

            var roomToDelete = dbContext.Rooms.
                First(r => r.RoomId == roomIdToDelete);

            if (HasBooking(roomToDelete))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThis room can not be deleted because it has an associated booking.");
            }
            else
            {
                dbContext.Rooms.Remove(roomToDelete);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThe Room Number: {roomToDelete.RoomId} is deleted.");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();

            dbContext.SaveChanges();
        }
        public void CheckAvailableRooms(Booking forBooking)
        {
            var newBookingDates = new List<DateTime>();
            for (var i = forBooking.CheckInDate; i <= forBooking.CheckOutDate; i = i.AddDays(1))
                newBookingDates.Add(i);


            foreach (var room in dbContext.Rooms.ToList())
            {
                bool roomIsFree = true;
                foreach (var booking in dbContext.Bookings.Include(b => b.Room).Where(b => b.Room == room))
                {
                    for (var i = booking.CheckInDate; i < booking.CheckOutDate; i = i.AddDays(1))
                    {
                        if (newBookingDates.Contains(i))
                            roomIsFree = false;
                    }
                    if (!roomIsFree)
                        break;
                }
                if (roomIsFree)
                    _availableRooms.Add(room);
            }            
        }
        public void ShowAvailableRooms()
        {
            if (_availableRooms.Count() < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nSorry! All rooms are booked for these dates. Please try another date");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n\n\nFollowing rooms are available for booking");

                var roomsTable = new ConsoleTable("Room", "Type", "Size", "Beds");

                foreach (var room in _availableRooms.OrderBy(r => r.RoomId))
                    roomsTable.AddRow(room.RoomId,
                        room.Type,
                        room.Size,
                        room.Beds);

                roomsTable.Write();
            }
            
        }
        private bool HasBooking(Room roomToDelete)
        {
            if (dbContext.Bookings.
                Include(b => b.Room).
                Any(b => b.Room.RoomId == roomToDelete.RoomId))
                return true;
            return false;
        }
    }
}
