using HotelApp.Controllers;
using HotelApp.Data;
using HotelApp.Models;
using HotelApp.Presentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Figgle;
using Spectre.Console;
using System.Linq.Expressions;
using HotelApp.System;

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
                var mainSel = Menu.ShowMainMenu();    
                if (mainSel == 4) return;
               
                if (mainSel == 1)
                {                    
                    var bookingSel = Menu.ShowBookingMenu();
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
                        bookingController.Search();

                }
                else if (mainSel == 2)
                {
                    var roomSel = Menu.ShowRoomMenu();
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
                else if (mainSel == 3)
                {
                    var guestSel = Menu.ShowGuestMenu();
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
