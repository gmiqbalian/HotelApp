using ClassLibrary;
using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.System
{
    public class RoomManager
    {
        private AppDbContext dbContext;
        private List<Room> _availableRooms;
        public RoomManager(AppDbContext context)
        {
            dbContext = context;
            _availableRooms = new List<Room>();
        }
        public void CheckAvailableRooms(Booking forBooking)
        {
            var newBookingDates = new List<DateTime>();
            for (var i = forBooking.CheckInDate; i <= forBooking.CheckOutDate; i = i.AddDays(1))
                newBookingDates.Add(i);


            foreach (var room in dbContext.Rooms.Include(r=>r.Type).ToList())
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
                Input.PressAnyKey();
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n\n\nFollowing rooms are available for booking");

                var roomsTable = new ConsoleTable("Room", "Type", "Size");

                foreach (var room in _availableRooms.OrderBy(r => r.RoomId))
                    roomsTable.AddRow(room.RoomId,
                        room.Type.Id,
                        room.Size);

                roomsTable.Write();
            }

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
