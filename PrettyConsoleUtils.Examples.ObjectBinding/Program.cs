using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyConsoleUtils.Examples.ObjectBinding
{
    class Program
    {
        static void Main(string[] args)
        {
            var prettyConsole = new PrettyConsole();
            prettyConsole.WriteLine("Hello world!", ConsoleColor.Black, ConsoleColor.White);
            prettyConsole.WriteLine("How are you doing?", ConsoleColor.Green);
            prettyConsole.ReadKey();
        }
    }
}
