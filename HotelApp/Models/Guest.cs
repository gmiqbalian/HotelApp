using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Guest 
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
