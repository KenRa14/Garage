using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Garage.UserInterface
{
    public class ConsoleUI : IUI
    {
        public void Print<T>(T message)
        {
            Console.Write(message);
        }

        public void PrintLine(string line)
        {
            Console.WriteLine(line);
        }

        public void PrintTitle(string title)
        {
            string mark = "".PadLeft(title.Length + 2, '*');
            PrintLine($"{mark}"
                + $"\n*{title}*"
                + $"\n{mark}");
        }

        public void PrintError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            PrintLine($"Error: {errorMessage}");
            Console.ResetColor();
        }

        public string GetInput()
        {
            string text = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            return text;
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
