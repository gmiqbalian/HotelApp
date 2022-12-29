using HotelApp.Controllers;
using HotelApp.Data;
using HotelApp.Models;
using HotelApp.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Figgle;
using Spectre.Console;


namespace HotelApp
{
    public class App
    {
        public void Run()
        {
            var builder = new Build();
            var dbContext = builder.BuildApp();

            while (true)
            {                
                Menu.ShowMainMenu();
                var sel = Menu.GetSelection();
                if (sel == 0) return;

                if (sel == 1)
                {
                    Menu.ShowBookingMenu();
                    var bookingSel = Menu.GetSelection();
                    var action = new BookingController(dbContext);

                    if (bookingSel == 1)
                        action.CreateBooking();

                    else if (bookingSel == 2)
                        action.ShowAllBookings();

                    else if (bookingSel == 3)
                        action.EditBooking();

                    else if (bookingSel == 4)
                        action.DeleteBooking();

                    else if (bookingSel == 5)
                        action.SearchBooking();

                    else if (bookingSel == 5)
                        return;
                }
                else if (sel == 2)
                {
                    Menu.ShowRoomMenu();
                    var roomSel = Menu.GetSelection();
                    var action = new RoomController(dbContext);

                    if (roomSel == 1)
                        action.CreateRoom();

                    else if (roomSel == 2)
                        action.ShowAllRooms();

                    else if (roomSel == 3)
                        action.EditRoom();

                    else if (roomSel == 4)
                        action.DeleteRoom();

                    else if (roomSel == 5)
                        return;
                }
                else if (sel == 3)
                {
                    Menu.ShowGuestMenu();

                    var guestSel = Menu.GetSelection();
                    var action = new GuestController(dbContext);

                    if (guestSel == 1)
                        action.CreateGuest();

                    else if (guestSel == 2)
                        action.ShowAllGuests();

                    else if (guestSel == 3)
                        action.EditGuest();

                    else if (guestSel == 4)
                        action.DeleteGuest();

                    else if (guestSel == 5)
                        return;
                }
            }
        }
    }
}
