using ClassLibrary;
using HotelApp.Controllers;
using HotelApp.Presentation;

namespace HotelApp
{
    public class App
    {
        public void Run()
        {
            var builder = new Build();
            var dbContext = builder.BuildApp();
            var controller = new Controller(dbContext);

            while (true)
            {
                var mainSel = Menu.ShowMainMenu();    
                if (mainSel == 4) return;
                
                if (mainSel == 1)
                {                    
                    var bookingSel = Menu.ShowBookingMenu();
                    var bookingController = controller.GetController("booking");

                    if (bookingSel == 1)
                        bookingController.Create();

                    else if (bookingSel == 2) { 
                        bookingController.ShowAll(); 
                        Input.PressAnyKey(); }                        
                     
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
                    var roomController = controller.GetController("room");

                    if (roomSel == 1)
                        roomController.Create();

                    else if (roomSel == 2) { 
                        roomController.ShowAll(); 
                        Input.PressAnyKey(); }
                                            
                    else if (roomSel == 3)
                        roomController.Update();

                    else if (roomSel == 4)
                        roomController.Delete();
                }
                else if (mainSel == 3)
                {
                    var guestSel = Menu.ShowGuestMenu();
                    var guestController = controller.GetController("guest");

                    if (guestSel == 1)
                        guestController.Create();

                    else if (guestSel == 2) { 
                        guestController.ShowAll(); 
                        Input.PressAnyKey(); }
                        
                    else if (guestSel == 3)
                        guestController.Update();

                    else if (guestSel == 4)
                        guestController.Delete();
                }
            }
        }
    }
}
