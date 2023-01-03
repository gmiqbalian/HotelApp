using ClassLibrary;
using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using HotelApp.System;

namespace HotelApp.Controllers
{
    public class GuestController : IController
    {
        private AppDbContext dbContext { get; set; }
        private readonly IGuestManager _guestManager;
        public GuestController(AppDbContext context, IGuestManager guestManager)
        {
            dbContext = context;
            _guestManager = guestManager;
        }
        public void Create()
        {
            Console.Clear();

            Console.WriteLine("\nREGISTER a new Guest");

            var newGuest = new Guest();
            var guestToAdd = _guestManager.GetGuestData(newGuest);

            dbContext.Guests.Add(guestToAdd);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nThe Guest is added succesfully.");

            Input.PressAnyKey();

            dbContext.SaveChanges();
        }
        public void ShowAll()
        {
            Console.Clear();
            Console.WriteLine("\nCurrent registered GUESTS\n");
            var table = new ConsoleTable("Id", "Name", "Age", "Phone", "Address");
            
            foreach (var guest in dbContext.Guests.ToList())
                table.AddRow(guest.GuestId, 
                    guest.Name, 
                    guest.Age, 
                    guest.Phone, 
                    guest.Street+ " " +guest.City + " " + guest.PostalCode);

            table.Write();            
        }
        public void Update()
        {
            ShowAll();

            Console.Write("\nEnter Guest Id to EDIT: ");
            var guestIdToEdit = Input.GetInt();

            var guestToEdit = dbContext.Guests.
                FirstOrDefault(g => g.GuestId == guestIdToEdit);

            _guestManager.GetGuestData(guestToEdit);

            dbContext.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nThe Guest is udpated succesfully.");

            Input.PressAnyKey();
        }
        public void Delete()
        {
            ShowAll();

            Console.Write("\nEnter guest Id to DELETE: ");
            var guestIdToDelete = Input.GetInt();

            var guestToDelete = dbContext.Guests.
                First(r => r.GuestId == guestIdToDelete);

            if (_guestManager.HasBookig(guestToDelete))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThis guest can not be deleted because it has an associated booking.");
            }
            else
            {
                dbContext.Guests.Remove(guestToDelete);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nThe Guest with id: {guestToDelete.GuestId} is deleted.");
            }

            dbContext.SaveChanges();

            Input.PressAnyKey();
        }

        public void Search()
        {
            throw new NotImplementedException();
        }
    }
}
