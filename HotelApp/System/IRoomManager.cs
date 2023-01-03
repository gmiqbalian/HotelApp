using HotelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.System
{
    public interface IRoomManager
    {
        public List<Room> CheckAvailableRooms(Booking forBooking);
        public void ShowAvailableRooms();
        public bool HasBooking(Room room);
        public RoomType GetRoomType(string roomType);
    }
}
