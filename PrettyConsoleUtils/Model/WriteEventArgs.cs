using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyConsoleUtils.Model
{
    public class WriteEventArgs
    {
        public WriteType WriteType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }
        public object Contents { get; set; }
    }
}
