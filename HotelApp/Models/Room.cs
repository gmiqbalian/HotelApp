using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Room 
    {        
        public int RoomId { get; set; }
        public string Type { get; set; } = null!;
        public int Beds { get; set; }
        public float Size { get; set; } //in square meters (m2)
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
