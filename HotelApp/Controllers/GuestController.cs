using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Controllers
{
    public class GuestController
    {
        private AppDbContext dbContext { get; set; }
        public GuestController(AppDbContext context)
        {
            dbContext = context;
        }
        public void CreateGuest()
        {
            Console.Clear();

            Console.WriteLine("REGISTER a new Guest");

            Console.Write("\nEnter Guest Name: ");
            var newGuestName = Console.ReadLine();

            Console.Write("\nEnter Guest Age: ");
            int.TryParse(Console.ReadLine(), out var newGuestAge);

            Console.Write("\nEnter Guest phone number: ");
            var newGuestPhone = Console.ReadLine();

            Console.Write("\nEnter Guest address: ");
            var newGuestAddress = Console.ReadLine();

            dbContext.Guests.Add(new Guest
            {
                Name = newGuestName,
                Age = newGuestAge,
                Phone = newGuestPhone,
                Address = newGuestAddress
            });

            dbContext.SaveChanges();
        }
        public void ShowAllGuests()
        {
            Console.WriteLine("\nId\tName\t\tAge\tPhone\t\tAddress");
            foreach (var guest in dbContext.Guests.ToList())
                Console.WriteLine($"{guest.Id}\t{guest.Name}\t{guest.Age}\t{guest.Phone}\t{guest.Address}");
            System.Threading.Thread.Sleep(5000);
        }
        public void EditGuest()
        {
            ShowAllGuests();

            Console.WriteLine("\nEnter Guest Id to EDIT: ");
            int.TryParse(Console.ReadLine(), out var guestIdToEdit);

            var guestToEdit = dbContext.Guests.FirstOrDefault(g => g.Id == guestIdToEdit);

            Console.Write("\nEnter Guest Name: ");
            guestToEdit.Name = Console.ReadLine();

            Console.Write("\nEnter Guest Age: ");
            guestToEdit.Age = Convert.ToInt32(Console.ReadLine());

            Console.Write("\nEnter Guest phone number: ");
            guestToEdit.Phone = Console.ReadLine();

            Console.Write("\nEnter Guest address: ");
            guestToEdit.Address = Console.ReadLine();

            dbContext.SaveChanges();
        }
        public void DeleteGuest()
        {
            ShowAllGuests();

            Console.WriteLine("\nEnter guest Id to DELETE: ");
            int.TryParse(Console.ReadLine(), out var guestIdToDelete);

            var guestToDelete = dbContext.Guests.First(r => r.Id == guestIdToDelete);

            if (dbContext.Bookings.Include(b => b.Guest).Any(b => b.Guest.Id == guestIdToDelete))
                Console.WriteLine("\nThis guest can not be deleted because it has an associated booking.");
            else
                dbContext.Guests.Remove(guestToDelete);

            dbContext.SaveChanges();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
    }
}
