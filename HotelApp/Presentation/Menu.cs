using Figgle;
using HotelApp.Data;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Presentation
{
    public class Menu
    {
        public static void ShowMainMenu()
        {
            Console.Clear();

            var text = FiggleFonts.Standard.Render("Hotel 4 U");
            AnsiConsole.Write(text);

            Console.WriteLine("1. Bookings");
            Console.WriteLine("2. Rooms");
            Console.WriteLine("3. Guests");
            Console.WriteLine("0. Exit");

            Console.Write("\nChoose an option: ");
        }
        public static void ShowBookingMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Register a new booking");
            Console.WriteLine("2. Show current bookings");
            Console.WriteLine("3. Edit current booking");
            Console.WriteLine("4. Delete a booking");
            Console.WriteLine("5. Search bookings");
            Console.WriteLine("6. Go to Main Menu");

            Console.Write("\nChoose an option: ");
        }
        public static void ShowRoomMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Register a new room");
            Console.WriteLine("2. Show current rooms");
            Console.WriteLine("3. Edit a room");
            Console.WriteLine("4. Delete a room");
            Console.WriteLine("5. Go to Main Menu");

            Console.Write("\nChoose an option: ");
        }
        public static void ShowGuestMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Register a new guest");
            Console.WriteLine("2. Show current guests");
            Console.WriteLine("3. Edit a current guest");
            Console.WriteLine("4. Delete a guest");
            Console.WriteLine("5. Go to Main Menu");

            Console.Write("\nChoose an option: ");
        }
        public static int GetSelection()
        {
            int.TryParse(Console.ReadLine(), out var sel);
            return sel;
        }
    }
}
