using HotelApp.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Controllers
{
    public class BookingController
    {
        private AppDbContext dbContext { get; set; }
        private Booking newBooking { get; set; }
        private List<Room> availableRooms { get; set; }
        private int numberOfDays { get; set; }        
        public BookingController(AppDbContext context)
        {
            dbContext = context;
            newBooking = new Booking();
            availableRooms = new List<Room>();
        }
        public void CreateBooking()
        {
            Console.Clear();

            newBooking.BookingDate = DateTime.Now;
            GetNumberOfDays();
            GetCheckInDate();
            GetCheckOutDate();
            CheckAvailableRooms();
            ShowAvailableRooms();
            ChooseRoom();
            AskExtraBed();
            ShowExistingGuests();
            ChooseGuest();
            SaveBooking();
        }
        public void ShowAllBookings()
        {
            var bookingsList = dbContext.Bookings.Include(b => b.Room).Include(b => b.Guest);

            Console.WriteLine("\nId\tBookingDate\tCheckInDate\tCheckOutDate\tGuest\t\tRoom Number\n");
            
            foreach (var booking in bookingsList)
                Console.WriteLine($"{booking.Id}" +
                    $"\t{booking.BookingDate.ToShortDateString()}" +
                    $"\t{booking.CheckInDate.ToShortDateString()}" +
                    $"\t{booking.CheckOutDate.ToShortDateString()}" +
                    $"\t{booking.Guest.Name}" +
                    $"\t{booking.Room.RoomId}");

            System.Threading.Thread.Sleep(5000);
        }
        public void EditBooking()
        {
            ShowAllBookings();

            newBooking.BookingDate = DateTime.Now;

            Console.WriteLine("\nEnter booking id to edit.");
            int.TryParse(Console.ReadLine(), out var bookingIdToEdit);
            newBooking = dbContext.Bookings.First(b => b.Id == bookingIdToEdit);

            GetNumberOfDays();
            GetCheckInDate();
            GetCheckOutDate();
            CheckAvailableRooms();
            ShowAvailableRooms();
            ChooseRoom();
            ShowExistingGuests();
            ChooseGuest();

            dbContext.SaveChanges();
        }
        public void DeleteBooking()
        {
            ShowAllBookings();
            Console.WriteLine("\nEnter booking id to delete.");
            int.TryParse(Console.ReadLine(), out var bookingIdToEdit);

            var bookingToDelete = dbContext.Bookings.First(b => b.Id == bookingIdToEdit);

            dbContext.Bookings.Remove(bookingToDelete);

            dbContext.SaveChanges();
        }
        public void SearchBooking()
        {
            Console.Write("\nEnter FROM date (Format: YYYY-MM-DD): ");
            DateTime.TryParse(Console.ReadLine(), out var fromDate);

            Console.Write("\nEnter TO date (Format: YYYY-MM-DD): ");
            DateTime.TryParse(Console.ReadLine(), out var toDate);

            var searchedBookingList = dbContext.Bookings.
                Include(b => b.Room).
                Include(b => b.Guest).
                Where(b => b.CheckInDate >= fromDate && b.CheckOutDate <= toDate).
                ToList();

            Console.WriteLine("\nId\tCheckInDate\tCheckOutDate\tGuest\t\tRoom Number");
            foreach (var booking in searchedBookingList)
                Console.WriteLine($"{booking.Id}\t{booking.CheckInDate.ToShortDateString()}" +
                    $"\t{booking.CheckOutDate.ToShortDateString()}" +
                    $"\t{booking.Guest.Name}" +
                    $"\t{booking.Room.RoomId}");

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
            newBooking.CheckInDate = new DateTime(2000, 01, 01); //edit this            
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
        private void CheckAvailableRooms()
        {
            var newBookingDates = new List<DateTime>();
            for (var i = newBooking.CheckInDate; i <= newBooking.CheckOutDate; i = i.AddDays(1))
                newBookingDates.Add(i);

            foreach (var room in dbContext.Rooms.ToList())
            {
                bool roomIsFree = true;
                foreach (var booking in dbContext.Bookings.Include(b => b.Room).Where(b => b.Room == room))
                {
                    for (var i = booking.CheckInDate; i < booking.CheckOutDate; i = i.AddDays(1))
                    {
                        if (newBookingDates.Contains(i))
                            roomIsFree = false;
                    }
                    if (!roomIsFree)
                        break;
                }
                if (roomIsFree)
                    availableRooms.Add(room);
            }
        }
        private void ShowAvailableRooms()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your booking details");
            Console.WriteLine("\nStart\t\tEnd\t\tNo. of days");
            Console.WriteLine($"{newBooking.CheckInDate.ToShortDateString()}\t{newBooking.CheckOutDate.ToShortDateString()}\t{numberOfDays}");

            if (availableRooms.Count() < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nSorry! All rooms are booked for these dates. Please try another date");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n\n\nFollowing rooms are available for booking");
                Console.WriteLine("\nRoom\tType\tSize\tBeds");

                foreach (var room in availableRooms.OrderBy(r => r.RoomId))
                    Console.WriteLine($"{room.RoomId}\t{room.Type}\t{room.Size}\t{room.Beds}");
            }
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
                var sel = Console.ReadLine().ToLower();

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
        private void ShowExistingGuests()
        {
            Console.Clear();
            

            Console.WriteLine("Id\tFull Name\tAddress\t\tPhone\n");
            foreach (var guest in dbContext.Guests.ToList())
                Console.WriteLine($"{guest.Id}\t{guest.Name}\t{guest.Address}\t\t{guest.Phone}");
        }
        private void ChooseGuest()
        {
            Console.WriteLine("\nEnter guest id who is booking or write \"NEW\" to add new guest");

            var sel = Console.ReadLine();
            int guestId;

            if (sel.ToLower() == "new")
            {
                var guestController = new GuestController(dbContext);
                guestController.CreateGuest();
                ShowExistingGuests();
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
            Console.WriteLine("Start\t\tEnd\t\tNo. of days");
            Console.WriteLine($"{newBooking.CheckInDate.ToShortDateString()}\t{newBooking.CheckOutDate.ToShortDateString()}\t{numberOfDays}");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
    }
}
