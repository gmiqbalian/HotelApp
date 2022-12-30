using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace HotelApp.Controllers
{
    public class BookingController
    {
        private AppDbContext dbContext { get; set; }
        private readonly GuestController _guestController;
        private readonly RoomController _roomController;
        private Booking newBooking { get; set; }
        private int numberOfDays { get; set; }
        public BookingController(AppDbContext context)
        {
            dbContext = context;
            newBooking = new Booking();            
            _guestController = new GuestController(dbContext);
            _roomController = new RoomController(dbContext);
        }
        public void Create()
        {
            Console.Clear();

            newBooking.BookingDate = DateTime.Now;
            GetNumberOfDays();
            GetCheckInDate();
            GetCheckOutDate();            
            ShowCurrentBooking();
            _roomController.CheckAvailableRooms(newBooking);
            _roomController.ShowAvailableRooms();
            ChooseRoom();
            AskExtraBed();
            _guestController.ShowAll();
            ChooseGuest();
            SaveBooking();
        }
        public void ShowAll()
        {
            var bookingsList = dbContext.Bookings.
                Include(b => b.Room).
                Include(b => b.Guest);
            
            if(bookingsList == null)
            {
                Console.WriteLine("There is no booking to show.");
                Console.ReadLine();
            }

            var table = new ConsoleTable("Id", "BookingDate", "CheckInDate", "CheckOutDate", "Guest", "Room Number");

            foreach (var booking in bookingsList)
                table.AddRow(booking.Id,
                    booking.BookingDate.ToShortDateString(),
                    booking.CheckInDate.ToShortDateString(),
                    booking.CheckOutDate.ToShortDateString(),
                    booking.Guest.Name,
                    booking.Room.RoomId);

            table.Write();
            System.Threading.Thread.Sleep(5000);
        }
        public void Update()
        {
            ShowAll();

            Console.WriteLine("\nEnter booking id to edit.");
            int.TryParse(Console.ReadLine(), out var bookingIdToEdit);
            newBooking = dbContext.Bookings.First(b => b.Id == bookingIdToEdit);

            newBooking.BookingDate = DateTime.Now;

            GetNumberOfDays();
            GetCheckInDate();
            GetCheckOutDate();
            ShowCurrentBooking();
            _roomController.CheckAvailableRooms(newBooking);
            _roomController.ShowAvailableRooms();
            ChooseRoom();
            _guestController.ShowAll();
            ChooseGuest();

            dbContext.SaveChanges();
        }
        public void Delete()
        {
            ShowAll();
            Console.WriteLine("\nEnter booking id to delete.");
            int.TryParse(Console.ReadLine(), out var bookingIdToEdit);
                        
            var bookingToDelete = dbContext.Bookings.First(b => b.Id == bookingIdToEdit);
            
            if(bookingToDelete != null)
            {
                dbContext.Bookings.Remove(bookingToDelete);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Booking is delete successfully!");
            }

            
            Console.ForegroundColor = ConsoleColor.Gray;
            dbContext.SaveChanges();
        }
        public void SearchBooking()
        {
            
            Console.Write("\nEnter FROM date (Format: YYYY-MM-DD): ");
            DateTime.TryParse(Console.ReadLine(), out var fromDate);

            var toDate = new DateTime(2000, 01, 01);

            while(toDate < fromDate)
            {
                Console.Write("\nEnter TO date (Format: YYYY-MM-DD): ");
                DateTime.TryParse(Console.ReadLine(), out toDate);
            }

            var searchedBookingList = dbContext.Bookings.
                Include(b => b.Room).
                Include(b => b.Guest).
                Where(b => b.CheckInDate >= fromDate && b.CheckOutDate <= toDate).
                OrderBy(b=> b.CheckInDate).                
                ToList();

            var table = new ConsoleTable(
                "Id", 
                "BookingDate", 
                "CheckInDate", 
                "CheckOutDate", 
                "Guest", 
                "Room Number");

            foreach (var booking in searchedBookingList)
                table.AddRow(booking.Id,
                    booking.BookingDate.ToShortDateString(),
                    booking.CheckInDate.ToShortDateString(),
                    booking.CheckOutDate.ToShortDateString(),
                    booking.Guest.Name,
                    booking.Room.RoomId);

            table.Write();

            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadLine();
        }

        private void GetNumberOfDays()
        {
            Console.WriteLine("\nHow many nights would you like to book?");
            numberOfDays = Convert.ToInt32(Console.ReadLine());
        }
        private void GetCheckInDate()
        {
            newBooking.CheckInDate = new DateTime(2000, 01, 01);           
            while (newBooking.CheckInDate < DateTime.Now)
            {
                Console.WriteLine("\nEnter the check in date (Format: yyyy-mm-dd)");
                newBooking.CheckInDate = Convert.ToDateTime(Console.ReadLine());
            }
        }
        private void GetCheckOutDate()
        {
            if (numberOfDays == 1)
                newBooking.CheckOutDate = newBooking.CheckInDate;
            else if (numberOfDays > 1)
                newBooking.CheckOutDate = newBooking.CheckInDate.AddDays(numberOfDays);
        }        
        private void ShowCurrentBooking()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Your booking details");
            
            var bookingTable = new ConsoleTable("Start", "End", "No.of days");
            
            bookingTable.AddRow(newBooking.CheckInDate.ToShortDateString(),
                newBooking.CheckOutDate.ToShortDateString(),
                numberOfDays);
            
            bookingTable.Write();
            Console.ForegroundColor = ConsoleColor.Gray;


        }
        private void ChooseRoom()
        {
            Console.WriteLine("\nEnter room number to choose from the available rooms");
            int.TryParse(Console.ReadLine(), out var roomNumber);

            newBooking.Room = dbContext.Rooms.
                Where(r => r.RoomId == roomNumber).
                First();
        }
        private void AskExtraBed()
        {
            if (newBooking.Room.Type == "Double")
            {
                Console.WriteLine("\nWould you like to add extra bed? (Y/N)");
                var sel = Console.ReadLine().Trim().ToLower();

                if (sel == "y" && newBooking.Room.Size <= 45)
                {
                    Console.WriteLine("\nOne extra bed is added.");
                    newBooking.Room.Beds += 1;
                }
                else if (sel == "y" && newBooking.Room.Size > 45)
                {
                    Console.WriteLine("\nHow many beds to add? (1 or 2)");
                    int.TryParse(Console.ReadLine(), out var extraBeds);
                    newBooking.Room.Beds += extraBeds;
                }
                else return;
            }
        }        
        private void ChooseGuest()
        {
            Console.WriteLine("\nEnter guest id who is booking or write \"NEW\" to add new guest");

            var sel = Console.ReadLine();
            int guestId;

            if (sel.ToLower() == "new")
            {                
                _guestController.Create();
                _guestController.ShowAll();
                Console.WriteLine("\nEnter guest id who is booking");
                int.TryParse(Console.ReadLine(), out guestId);
            }
            else
                int.TryParse(sel, out guestId);

            newBooking.Guest = dbContext.Guests.
                Where(g => g.Id == guestId).
                First();
        }
        private void SaveBooking()
        {
            dbContext.Add(newBooking);            
            dbContext.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine("Booking successful!");
            Console.WriteLine("=============================");

            var table = new ConsoleTable("Start", "End", "No. of Days");
            table.AddRow(newBooking.CheckInDate.ToShortDateString(),
                newBooking.CheckOutDate.ToShortDateString(),
                numberOfDays);
            
            
            table.Write();
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
    }
}
