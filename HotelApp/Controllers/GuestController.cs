using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using HotelApp.System;

namespace HotelApp.Controllers
{
    public class GuestController
    {
        private AppDbContext dbContext { get; set; }
        private GuestManager _guestManager { get; set; }
        public GuestController(AppDbContext context)
        {
            dbContext = context;
            _guestManager = new GuestManager(dbContext);
        }
        public void Create()
        {
            Console.Clear();

            Console.WriteLine("REGISTER a new Guest");

            var newGuest = new Guest();
            var guestToAdd = _guestManager.GetGuestData(newGuest);

            dbContext.Guests.Add(guestToAdd);

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
            Console.WriteLine("\nCurrent registered GUESTS\n");
            var table = new ConsoleTable("Id", "Name", "Age", "Phone", "Address");
            
            foreach (var guest in dbContext.Guests.ToList())
                table.AddRow(guest.GuestId, 
                    guest.Name, 
                    guest.Age, 
                    guest.Phone, 
                    guest.Street+ " " +guest.City + " " + guest.PostalCode);

            table.Write();
            Console.ReadLine();
            //System.Threading.Thread.Sleep(5000);
        }
        public void Update()
        {
            ShowAll();

            Console.Write("\nEnter Guest Id to EDIT: ");
            int.TryParse(Console.ReadLine(), out var guestIdToEdit);

            var guestToEdit = dbContext.Guests.
                FirstOrDefault(g => g.GuestId == guestIdToEdit);

            _guestManager.GetGuestData(guestToEdit);

            dbContext.SaveChanges();
        }
        public void Delete()
        {
            ShowAll();

            Console.Write("\nEnter guest Id to DELETE: ");
            int.TryParse(Console.ReadLine(), out var guestIdToDelete);

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

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
       
    }
}
