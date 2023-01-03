using ClassLibrary;
using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelApp.System;

namespace HotelApp.Controllers
{
    public class RoomController : IController
    {
        private AppDbContext dbContext { get; set; }
        private readonly IRoomManager _roomManager;       

        public RoomController(AppDbContext context, IRoomManager roomManager)
        {
            dbContext = context;            
            _roomManager = roomManager;
        }
        public void Create()
        {
            Console.Clear();

            Console.WriteLine("\nREGISTER a new Room");

            Console.Write("\nEnter room type (Single/Double): ");
            var newRoomType = Input.GetStringWithOptions("single", "double");

            Console.Write("\nEnter room type area in (m2): ");
            var newRoomSize = Input.GetInt();

            dbContext.Rooms.Add(new Room
            {
                Type = _roomManager.GetRoomType(newRoomType),
                Size = newRoomSize,
            });

            dbContext.SaveChanges();

            Console.WriteLine("\nNew Room added successfully!!!");
            Input.PressAnyKey();
        }
        public void ShowAll()
        {
            Console.Clear();

            Console.WriteLine("\nCurrent registered ROOMS\n");
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
        }
        public void Update()
        {
            ShowAll();

            Console.Write("\nEnter room number to EDIT: ");
            var roomIdToEdit = Input.GetInt();

            var roomToEdit = dbContext.Rooms.
                FirstOrDefault(r => r.RoomId == roomIdToEdit);

            Console.Write("\nEnter new type (Single/Double): ");
            var roomToEditType = Input.GetStringWithOptions("single", "double");

            roomToEdit.Type = _roomManager.GetRoomType(roomToEditType);

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

        public void Search()
        {
            throw new NotImplementedException();
        }
    }
}
