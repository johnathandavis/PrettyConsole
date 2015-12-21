using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyConsoleUtils
{
    public class DefaultConsoleWrapper : ConsoleProvider
    {
        public override int CursorY
        {
            get
            {
                return Console.CursorTop;
            }
            set
            {
                Console.CursorTop = value;
            }
        }
        public override int CursorX
        {
            get
            {
                return Console.CursorLeft;
            }
            set
            {
                Console.CursorLeft = value;
            }
        }
        public override ConsoleColor Background
        {
            get
            {
                return Console.BackgroundColor;
            }
            set
            {
                Console.BackgroundColor = value;
            }
        }
        public override ConsoleColor Foreground
        {
            get
            {
                return Console.ForegroundColor;
            }
            set
            {
                Console.ForegroundColor = value;
            }
        }

        public override void Write(string txt)
        {
            Console.Write(txt);
        }
        public override ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }
        public override string ReadLine()
        {
            return Console.ReadLine();
        }
        public override int Read()
        {
            return Console.Read();
        }
    }
}
