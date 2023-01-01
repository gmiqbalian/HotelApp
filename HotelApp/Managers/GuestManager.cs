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
    public class GuestManager
    {
        private AppDbContext dbContext { get; set; }
        public GuestManager(AppDbContext context)
        {
            dbContext = context;
        }
        public bool HasBookig(Guest guestToDelete)
        {
            if (dbContext.Bookings.
                Include(b => b.Guest).
                Any(b => b.Guest.GuestId == guestToDelete.GuestId))
                return true;
            return false;
        }
        public Guest GetGuestData(Guest guest)
        {

            Console.Write("\nEnter Guest Name: ");
            guest.Name = Console.ReadLine();

            Console.Write("\nEnter Guest Age: ");
            guest.Age = Convert.ToInt32(Console.ReadLine());

            Console.Write("\nEnter Guest phone number: ");
            guest.Phone = Console.ReadLine();

            Console.Write("\nEnter Guest street address: ");
            guest.Street = Console.ReadLine();

            Console.Write("\nEnter Guest City: ");
            guest.City = Console.ReadLine();

            Console.Write("\nEnter Guest postal code: ");
            guest.PostalCode = Convert.ToInt32(Console.ReadLine());

            return guest;
        }
    }
}
