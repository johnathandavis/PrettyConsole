using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyConsoleUtils
{
    public abstract class ConsoleProvider
    {
        public abstract int CursorX { get; set; }
        public abstract int CursorY { get; set; }
        public abstract ConsoleColor Foreground { get; set; }
        public abstract ConsoleColor Background { get; set; }

        public abstract void Write(string txt);

        public abstract int Read();
        public abstract ConsoleKeyInfo ReadKey(bool intercept);
        public abstract string ReadLine();
    }
}
