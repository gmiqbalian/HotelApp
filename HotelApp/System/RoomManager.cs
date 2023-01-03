using ClassLibrary;
using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.System
{
    public class RoomManager : IRoomManager
    {
        private AppDbContext dbContext;
        private List<Room> _availableRooms;
        public RoomManager(AppDbContext context)
        {
            dbContext = context;
        }
        public List<Room> CheckAvailableRooms(Booking forBooking)
        {
            _availableRooms = new List<Room>();

            var newBookingDates = new List<DateTime>();
            for (var i = forBooking.CheckInDate; i <= forBooking.CheckOutDate; i = i.AddDays(1))
                newBookingDates.Add(i);


            foreach (var room in dbContext.Rooms.Include(r=>r.Type).ToList())
            {
                bool roomIsFree = true;
                foreach (var booking in dbContext.Bookings.Include(b => b.Room).Where(b => b.Room == room))
                {
                    for (var i = booking.CheckInDate; i <= booking.CheckOutDate; i = i.AddDays(1))
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

            return _availableRooms;
        }
        public void ShowAvailableRooms()
        {            
            Console.WriteLine("\n\n\nFollowing rooms are available for booking");

            var roomsTable = new ConsoleTable("Room", "Type", "Size");

            foreach (var room in _availableRooms.OrderBy(r => r.RoomId))
                roomsTable.AddRow(room.RoomId,
                    room.Type.Id,
                    room.Size);

            roomsTable.Write();
        }

        public RoomType GetRoomType(string roomType)
        {
            if (roomType.ToLower() == "single")
                return dbContext.
                    RoomTypes.
                    First(rt => rt.Id == "Single");
            else if (roomType.ToLower() == "double")
                return dbContext.
                    RoomTypes.
                    First(rt => rt.Id == "Double");

            return null;
        }
        public bool HasBooking(Room room)
        {
            if (dbContext.Bookings.
                Include(b => b.Room).
                Any(b => b.Room.RoomId == room.RoomId))
                return true;
            return false;
        }

    }
}
