using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Booking 
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }        
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
