using ClassLibrary;
using ConsoleTables;
using HotelApp.Controllers;
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
    public class BookingManager
    {
        private AppDbContext dbContext;
        private GuestController _guestController;
        private RoomManager _roomManager;
        private int numberOfDays { get; set; }        
        public BookingManager(AppDbContext context)
        {
            dbContext = context;
            _guestController = new GuestController(dbContext);
            _roomManager = new RoomManager(dbContext);
        }
        public DateTime GetCheckInDate()
        {
            Console.WriteLine("\nHow many nights would you like to book?");
            numberOfDays = Input.GetInt();

            var checkInDate = new DateTime(2000, 01, 01);
            while (checkInDate < DateTime.Now)
            {
                Console.WriteLine("\nEnter the check in date (Format: yyyy-mm-dd)");
                checkInDate = Input.GetDateTime();
            }

            return checkInDate;
        }
        public DateTime GetCheckOutDate(Booking forBooking)
        {
            var checkOutDate = new DateTime();

            if (numberOfDays == 1)
                checkOutDate = forBooking.CheckInDate;
            else if (numberOfDays > 1)
                checkOutDate = forBooking.CheckInDate.AddDays(numberOfDays);

            return checkOutDate;
        }
        public void ShowCurrentBooking(Booking forBooking)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Your booking details");

            var bookingTable = new ConsoleTable("Start", "End", "No.of days");

            bookingTable.AddRow(forBooking.CheckInDate.ToShortDateString(),
                forBooking.CheckOutDate.ToShortDateString(),
                numberOfDays);

            bookingTable.Write();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public Room GetRoom(Booking forBooking)
        {
            _roomManager.CheckAvailableRooms(forBooking);
            _roomManager.ShowAvailableRooms();

            Console.WriteLine("\nEnter room number to choose from the available rooms:");
            var roomNumber = Input.GetInt();

            var room = dbContext.Rooms.
                Include(r=> r.Type).
                Where(r => r.RoomId == roomNumber).
                First();
            
            AskExtraBed(room);

            return room;
        }
        public void AskExtraBed(Room forRoom)
        {
            if (forRoom.Type.Id == "Double")
            {
                Console.WriteLine("\nWould you like to add extra bed? (Y/N)");
                var sel = Input.GetString();

                if (sel == "y" && forRoom.Size <= 45)
                {
                    Console.WriteLine("\nOne extra bed is added.");
                    forRoom.ExtraBed = 1;
                }
                else if (sel == "y" && forRoom.Size > 45)
                {
                    Console.WriteLine("\nHow many beds to add? (1 or 2)");
                    var extraBeds = Input.GetInt();
                    forRoom.ExtraBed = extraBeds;
                    Console.WriteLine($"\n{extraBeds} extra beds added");
                }
            }

            Input.PressAnyKey();
        }
        public Guest GetGuest()
        {
            _guestController.ShowAll();

            Console.WriteLine("\nEnter guest id who is booking or write \"NEW\" to add new guest");
            var sel = Console.ReadLine().Trim();
            int guestId;

            if (sel.ToLower() == "new")
            {
                _guestController.Create();
                _guestController.ShowAll();
                Console.WriteLine("\nEnter guest id who is booking");
                guestId = Input.GetInt();
            }
            else
                int.TryParse(sel, out guestId);

            var guest = dbContext.Guests.
                Where(g => g.GuestId == guestId).
                First();

            return guest;
        }
        public void ShowBooking(Booking newBooking)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine("Booking successful!");
            Console.WriteLine("=============================");

            var table = new ConsoleTable("Start", "End", "No. of Days");
            table.AddRow(newBooking.CheckInDate.ToShortDateString(),
                newBooking.CheckOutDate.ToShortDateString(),
                numberOfDays);

            table.Write();

            Input.PressAnyKey();
        }

    }
}
