using HotelApp.Controllers;
using HotelApp.Data;
using HotelApp.Models;
using HotelApp.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Figgle;
using Spectre.Console;
using System.Linq.Expressions;

namespace HotelApp
{
    public class App
    {
        public void Run()
        {
            var builder = new Build();
            var dbContext = builder.BuildApp();
                        
            while (true)    //add go back to main menu
            {
                Menu.ShowMainMenu();
                var sel = Menu.GetSelection();
                if (sel == 0) return;
               
                if (sel == 1)
                {
                    Menu.ShowBookingMenu();
                    var bookingSel = Menu.GetSelection();
                    var bookingController = new BookingController(dbContext);

                    if (bookingSel == 1)
                        bookingController.Create();

                    else if (bookingSel == 2)
                        bookingController.ShowAll();

                    else if (bookingSel == 3)
                        bookingController.Update();

                    else if (bookingSel == 4)
                        bookingController.Delete();

                    else if (bookingSel == 5)
                        bookingController.SearchBooking();

                }
                else if (sel == 2)
                {
                    Menu.ShowRoomMenu();
                    var roomSel = Menu.GetSelection();
                    var roomController = new RoomController(dbContext);

                    if (roomSel == 1)
                        roomController.Create();

                    else if (roomSel == 2)
                        roomController.ShowAll();

                    else if (roomSel == 3)
                        roomController.Update();

                    else if (roomSel == 4)
                        roomController.Delete();
                }
                else if (sel == 3)
                {
                    Menu.ShowGuestMenu();

                    var guestSel = Menu.GetSelection();
                    var guestController = new GuestController(dbContext);

                    if (guestSel == 1)
                        guestController.Create();

                    else if (guestSel == 2)
                        guestController.ShowAll();

                    else if (guestSel == 3)
                        guestController.Update();

                    else if (guestSel == 4)
                        guestController.Delete();
                }
            }
        }
    }
}
