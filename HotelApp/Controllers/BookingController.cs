using ConsoleTables;
using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using Microsoft.IdentityModel.Tokens;
using HotelApp.System;
using System.Dynamic;
using ClassLibrary;

namespace HotelApp.Controllers
{
    public class BookingController
    {
        private AppDbContext dbContext { get; set; }
        private readonly BookingManager _bookingManager;        
        private readonly GuestController _guestController;
        
        public BookingController(AppDbContext context)
        {
            dbContext = context;
            _bookingManager = new BookingManager(dbContext);
            _guestController = new GuestController(dbContext);            
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

            Console.WriteLine("\nBooking is created successfully!!!");
            Input.PressAnyKey();
        }
        public void ShowAll()
        {
            Console.Clear();
            Console.WriteLine("\nCurrent BOOKINGS\n");
            var bookingsList = dbContext.Bookings.
                Include(b => b.Room).
                Include(b => b.Guest).
                Include(b=> b.Room.Type).ToList();
            
            if(bookingsList.Count() == 0)
                Console.WriteLine("\nThere is no booking to show.");
            else 
            { 
                var table = new ConsoleTable("Id", 
                    "BookingDate", 
                    "CheckInDate", 
                    "CheckOutDate", 
                    "Guest", 
                    "Room",
                    "Type",
                    "Extra Bed");

                foreach (var booking in bookingsList)
                    table.AddRow(booking.BookingId,
                        booking.BookingDate.ToShortDateString(),
                        booking.CheckInDate.ToShortDateString(),
                        booking.CheckOutDate.ToShortDateString(),
                        booking.Guest.Name,
                        booking.Room.RoomId,
                        booking.Room.Type.Id,
                        booking.Room.ExtraBed);

                table.Write();                
            }            
        }
        public void Update()
        {

            ShowAll();

            Console.Write("\nEnter booking id to edit: ");
            var bookingIdToEdit = Input.GetInt();
            
            var bookingToUpdate = dbContext.Bookings.
                First(b => b.BookingId == bookingIdToEdit);

            bookingToUpdate.BookingDate = DateTime.Now;
            bookingToUpdate.CheckInDate = _bookingManager.GetCheckInDate();
            bookingToUpdate.CheckOutDate = _bookingManager.GetCheckOutDate(bookingToUpdate);
            bookingToUpdate.Room = _bookingManager.GetRoom(bookingToUpdate);            
            bookingToUpdate.Guest = _bookingManager.GetGuest();

            dbContext.SaveChanges();
            
            Console.WriteLine("\nBooking is updated successfully!!!");
            Input.PressAnyKey();
        }
        public void Delete()
        {
            ShowAll();
            Console.Write("\nEnter booking id to delete: ");
            var bookingIdToEdit = Input.GetInt();
                        
            var bookingToDelete = dbContext.Bookings.
                First(b => b.BookingId == bookingIdToEdit);
            
            if(bookingToDelete != null)
            {
                dbContext.Bookings.Remove(bookingToDelete);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nBooking is deleted successfully!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Input.PressAnyKey();            
            dbContext.SaveChanges();
        }
        public void Search()
        {

            Console.Write("\nEnter FROM date (Format: YYYY-MM-DD): ");
            var fromDate = Input.GetDateTime();

            var toDate = new DateTime(2000, 01, 01);

            while (toDate < fromDate)
            {
                Console.Write("\nEnter TO date (Format: YYYY-MM-DD): ");
                toDate = Input.GetDateTime();
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
                "Room");

            foreach (var booking in searchedBookingList)
                table.AddRow(booking.BookingId,
                    booking.BookingDate.ToShortDateString(),
                    booking.CheckInDate.ToShortDateString(),
                    booking.CheckOutDate.ToShortDateString(),
                    booking.Guest.Name,
                    booking.Room.RoomId);

            table.Write();

            Input.PressAnyKey();
        }

    }
}
