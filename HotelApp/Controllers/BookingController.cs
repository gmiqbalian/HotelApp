using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using HotelApp.System;
using System;
using Microsoft.IdentityModel.Tokens;

namespace HotelApp.Controllers
{
    public class BookingController
    {
        private AppDbContext dbContext { get; set; }
        private readonly GuestController _guestController;
        private readonly RoomController _roomController;
        private readonly RoomManager _roomManager;
        private readonly BookingManager _bookingManager;
        private Booking newBooking { get; set; }
        
        public BookingController(AppDbContext context)
        {
            dbContext = context;                 
            _guestController = new GuestController(dbContext);
            _roomController = new RoomController(dbContext);
            _roomManager = new RoomManager(dbContext);
            _bookingManager = new BookingManager(dbContext);
        }
        public void Create()
        {
            Console.Clear();
            Console.WriteLine("REGISTER a new Booking");
            var newBooking = new Booking();

            newBooking.BookingDate = DateTime.Now;
            newBooking.CheckInDate = _bookingManager.GetCheckInDate();
            newBooking.CheckOutDate = _bookingManager.GetCheckOutDate(newBooking);
            _bookingManager.ShowCurrentBooking(newBooking);
            newBooking.Room = _bookingManager.GetRoom(newBooking);
            newBooking.Guest = _bookingManager.GetGuest();
            
            dbContext.Add(newBooking);
            dbContext.SaveChanges();
        }
        public void ShowAll()
        {
            Console.Clear();
            Console.WriteLine("\nCurrent BOOKINGS\n");
            var bookingsList = dbContext.Bookings.
                Include(b => b.Room).
                Include(b => b.Guest).ToList();
            
            if(bookingsList.Count() == 0)
                Console.WriteLine("\nThere is no booking to show.");
            else 
            { 
                var table = new ConsoleTable("Id", 
                    "BookingDate", 
                    "CheckInDate", 
                    "CheckOutDate", 
                    "Guest", 
                    "Room Number");

                foreach (var booking in bookingsList)
                    table.AddRow(booking.BookingId,
                        booking.BookingDate.ToShortDateString(),
                        booking.CheckInDate.ToShortDateString(),
                        booking.CheckOutDate.ToShortDateString(),
                        booking.Guest.Name,
                        booking.Room.RoomId);

                table.Write();
            //System.Threading.Thread.Sleep(5000);
            }
            Console.ReadLine();
        }
        public void Update()
        {

            ShowAll();

            Console.Write("\nEnter booking id to edit: ");
            int.TryParse(Console.ReadLine(), out var bookingIdToEdit);
            
            var bookingToUpdate = dbContext.Bookings.
                First(b => b.BookingId == bookingIdToEdit);

            bookingToUpdate.BookingDate = DateTime.Now;
            bookingToUpdate.CheckInDate = _bookingManager.GetCheckInDate();
            bookingToUpdate.CheckOutDate = _bookingManager.GetCheckOutDate(bookingToUpdate);
            bookingToUpdate.Room = _bookingManager.GetRoom(bookingToUpdate);
            bookingToUpdate.Guest = _bookingManager.GetGuest();

            Console.WriteLine("\nBooking is successful!!!");
            Console.WriteLine("\nPress any key to continue...");
            dbContext.SaveChanges();
        }
        public void Delete()
        {
            ShowAll();
            Console.Write("\nEnter booking id to delete: ");
            int.TryParse(Console.ReadLine(), out var bookingIdToEdit);
                        
            var bookingToDelete = dbContext.Bookings.
                First(b => b.BookingId == bookingIdToEdit);
            
            if(bookingToDelete != null)
            {
                dbContext.Bookings.Remove(bookingToDelete);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nBooking is deleted successfully!");
            }

            
            Console.ForegroundColor = ConsoleColor.Gray;
            dbContext.SaveChanges();
        }
        public void SearchBooking()
        {

            Console.Write("\nEnter FROM date (Format: YYYY-MM-DD): ");
            DateTime.TryParse(Console.ReadLine(), out var fromDate);

            var toDate = new DateTime(2000, 01, 01);

            while (toDate < fromDate)
            {
                Console.Write("\nEnter TO date (Format: YYYY-MM-DD): ");
                DateTime.TryParse(Console.ReadLine(), out toDate);
            }

            var searchedBookingList = dbContext.Bookings.
                Include(b => b.Room).
                Include(b => b.Guest).
                Where(b => b.CheckInDate >= fromDate && b.CheckOutDate <= toDate).
                OrderBy(b => b.CheckInDate).
                ToList();

            var table = new ConsoleTable(
                "Id",
                "BookingDate",
                "CheckInDate",
                "CheckOutDate",
                "Guest",
                "Room Number");

            foreach (var booking in searchedBookingList)
                table.AddRow(booking.BookingId,
                    booking.BookingDate.ToShortDateString(),
                    booking.CheckInDate.ToShortDateString(),
                    booking.CheckOutDate.ToShortDateString(),
                    booking.Guest.Name,
                    booking.Room.RoomId);

            table.Write();

            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadLine();
        }

    }
}
