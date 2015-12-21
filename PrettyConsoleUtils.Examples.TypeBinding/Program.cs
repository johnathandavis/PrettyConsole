using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyConsoleUtils.Examples.TypeBinding
{
    class Program
    {
        static void Main(string[] args)
        {
            var pc = new PrettyConsole();
            pc.WriteLine("Hello world!", ConsoleColor.Green, ConsoleColor.Blue);
            pc.ReadKey();
        }
    }
}
