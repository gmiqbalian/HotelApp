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
        public int GuestId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public int? PostalCode { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
