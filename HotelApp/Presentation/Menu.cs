using ClassLibrary;
using Figgle;
using Spectre.Console;

namespace HotelApp.Presentation
{
    public class Menu
    {
        public static int ShowMainMenu()
        {
            Console.Clear();

            var text = FiggleFonts.Standard.Render("Hotel 4 U");
            AnsiConsole.Write(text);

            //var image = new CanvasImage("hotel.png");
            //image.MaxWidth(40);
            //AnsiConsole.Write(image);

            Console.WriteLine("1. Bookings");
            Console.WriteLine("2. Rooms");
            Console.WriteLine("3. Guests");
            Console.WriteLine("4. Exit");

            return GetSelection(1, 4);
        }
        public static int ShowBookingMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Register a new booking");
            Console.WriteLine("2. Show current bookings");
            Console.WriteLine("3. Edit current booking");
            Console.WriteLine("4. Delete a booking");
            Console.WriteLine("5. Search bookings");

            return GetSelection(1, 5);
        }
        public static int ShowRoomMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Register a new room");
            Console.WriteLine("2. Show current rooms");
            Console.WriteLine("3. Edit a room");
            Console.WriteLine("4. Delete a room");

            return GetSelection(1, 4);
        }
        public static int ShowGuestMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Register a new guest");
            Console.WriteLine("2. Show current guests");
            Console.WriteLine("3. Edit a current guest");
            Console.WriteLine("4. Delete a guest");

            return GetSelection(1, 4);
        }
        public static int GetSelection(int start, int end)
        {
            Console.Write("\nChoose an option: ");
            var sel = Input.GetInt();
            return Input.ValidateRange(sel, start, end);
        }
    }
}
