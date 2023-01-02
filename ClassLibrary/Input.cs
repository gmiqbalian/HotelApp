﻿namespace ClassLibrary
{
    public static class Input
    {
        public static string GetString()
        {
            var input = Console.ReadLine().ToLower().Trim();
            while (!IsLetter(input))
            {
                Console.Write("\nEnter valid input: ");
                input = Console.ReadLine();
            }
            return input;
        }
        public static int GetInt()
        {
            var input = Console.ReadLine().ToLower().Trim();
            int returnedInput = 0;

            while (!IsInt(input))
            {
                Console.Write("\nEnter valid input: ");
                input = Console.ReadLine();
            }

            int.TryParse(input, out returnedInput);

            return returnedInput;
        }
        public static DateTime GetDateTime()
        {
            var input = Console.ReadLine();
            return Convert.ToDateTime(input);
        }
        public static bool IsInt(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                foreach (var c in input.ToCharArray())
                {
                    if (char.IsLetter(c))
                        return false;
                }
            }
            return true;
        }
        public static bool IsLetter(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                foreach (var c in input.ToCharArray())
                {
                    if (char.IsDigit(c))
                        return false;
                }
            }
            return true;
        }
        public static int ValidateRange(int input, int startRange, int endRange)
        {
            int returnedInput = input;
            while (returnedInput < startRange || returnedInput > endRange)
            {
                Console.Write("\nInput is out of range. Please enter from given options: ");
                returnedInput = GetInt();
            }
            return returnedInput;
        }
        public static void PressAnyKey()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nPress any key to continue...");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey();
        }
    }
}