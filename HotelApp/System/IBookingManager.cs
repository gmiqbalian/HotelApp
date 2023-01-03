using HotelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.System
{
    public interface IBookingManager
    {
        public DateTime GetCheckInDate();
        public DateTime GetCheckOutDate(Booking forBooking);
        public void ShowCurrentBooking(Booking forBooking);
        public Room GetRoom(Booking forBooking);
        public void AskExtraBed(Room forRoom);
        public Guest GetGuest();
        public void ShowBooking(Booking newBooking);        
    }
}
