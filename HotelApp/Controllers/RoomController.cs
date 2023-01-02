using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using ConsoleTables;
using HotelApp.System;
using ClassLibrary;

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
            var newRoomType = Input.GetString();

            Console.Write("\nEnter room type area in (m2): ");
            var newRoomSize = Input.GetInt(); ;
                        
            dbContext.Rooms.Add(new Room
            {
                Type = _roomTypeManager.GetRoomType(newRoomType),
                Size = newRoomSize,
            });

            dbContext.SaveChanges();

            Console.WriteLine("\nNew Room added successfully!!!");
            Input.PressAnyKey();
        }
        public void ShowAll()
        {
            Console.Clear();

            Console.WriteLine("Current registered ROOMS\n");
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

            Input.PressAnyKey();            
        }
        public void Update()
        {
            ShowAll();

            Console.Write("\nEnter room number to EDIT: ");
            var roomIdToEdit = Input.GetInt();

            var roomToEdit = dbContext.Rooms.
                FirstOrDefault(r => r.RoomId == roomIdToEdit);

            Console.Write("\nEnter new type (Single/Double): ");
            var roomToEditType = Input.GetString();
            
            roomToEdit.Type = _roomTypeManager.GetRoomType(roomToEditType);
            
            Console.Write("\nEnter the room size: ");
            roomToEdit.Size = Input.GetInt(); ;

            dbContext.SaveChanges();
        }
        public void Delete()
        {
            ShowAll();

            Console.Write("\nEnter room number to DELETE: ");
            var roomIdToDelete = Input.GetInt();

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

            Input.PressAnyKey();

            dbContext.SaveChanges();
        }        
    }
}
