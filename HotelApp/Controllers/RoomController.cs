using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using ConsoleTables;
using HotelApp.System;

namespace HotelApp.Controllers
{
    public class RoomController
    {
        private AppDbContext dbContext { get; set; }
        private RoomManager _roomManager;
        private readonly RoomTypeManager _roomTypeManager;
        
        public RoomController(AppDbContext context)
        {
            dbContext = context;            
            _roomTypeManager = new RoomTypeManager(dbContext);
            _roomManager = new RoomManager(dbContext);
        }
        public void Create()
        {
            Console.Clear();

            Console.WriteLine("REGISTER a new Room");

            Console.Write("\nEnter room type (Single/Double): ");
            var newRoomType = Console.ReadLine();

            Console.Write("\nEnter room type area in (m2): ");
            int.TryParse(Console.ReadLine(), out var newRoomSize);
                        
            dbContext.Rooms.Add(new Room
            {
                Type = _roomTypeManager.GetRoomType(newRoomType),
                Size = newRoomSize,
            });

            dbContext.SaveChanges();

            Console.WriteLine("\nNew Room added successfully!!!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
        public void ShowAll()
        {
            var table = new ConsoleTable("Number", "Type", "Beds", "Extra Bed", "Size");
            var roomsList = dbContext.Rooms.
                Include(r => r.Type).ToList();
            
            foreach (var room in roomsList)
                table.AddRow(room.RoomId, 
                    room.Type.Id, 
                    room.Type.Bed, 
                    room.ExtraBed, 
                    room.Size);                    
           
            table.Write();

            Console.ReadLine();
            //System.Threading.Thread.Sleep(5000);
        }
        public void Update()
        {
            ShowAll();

            Console.WriteLine("\nEnter room number to EDIT: ");
            int.TryParse(Console.ReadLine(), out var roomIdToEdit);

            var roomToEdit = dbContext.Rooms.
                FirstOrDefault(r => r.RoomId == roomIdToEdit);

            Console.Write("\nEnter new type (Single/Double): ");
            string roomToEditType = Console.ReadLine();
            
            roomToEdit.Type = _roomTypeManager.GetRoomType(roomToEditType);
            
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

            if (_roomManager.HasBooking(roomToDelete))
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
    }
}
