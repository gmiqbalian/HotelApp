using ConsoleTables;
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
        public void Create()
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nThe Guest is added succesfully.");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();

            dbContext.SaveChanges();
        }
        public void ShowAll()
        {
            Console.Clear();

            var table = new ConsoleTable("Id", "Name", "Age", "Phone", "Address");
            
            foreach (var guest in dbContext.Guests.ToList())
                table.AddRow(guest.Id, 
                    guest.Name, 
                    guest.Age, 
                    guest.Phone, 
                    guest.Address);

            table.Write();
            System.Threading.Thread.Sleep(5000);
        }
        public void Update()
        {
            ShowAll();

            Console.WriteLine("\nEnter Guest Id to EDIT: ");
            int.TryParse(Console.ReadLine(), out var guestIdToEdit);

            var guestToEdit = dbContext.Guests.
                FirstOrDefault(g => g.Id == guestIdToEdit);

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
        public void Delete()
        {
            ShowAll();

            Console.WriteLine("\nEnter guest Id to DELETE: ");
            int.TryParse(Console.ReadLine(), out var guestIdToDelete);

            var guestToDelete = dbContext.Guests.
                First(r => r.Id == guestIdToDelete);

            if (HasBookig(guestToDelete))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThis guest can not be deleted because it has an associated booking.");
            }
            else
                dbContext.Guests.Remove(guestToDelete);
            
            dbContext.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nThe Guest with id: {guestToDelete.Id} is deleted.");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
        private bool HasBookig(Guest guestToDelete)
        {
            if (dbContext.Bookings.
                Include(b => b.Guest).
                Any(b => b.Guest.Id == guestToDelete.Id))
                return true;
            return false;
        }
    }
}
