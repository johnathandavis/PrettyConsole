using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrettyConsoleUtils.Model
{
    public class PrintEventArgs : WriteEventArgs
    {
        public Type GivenType { get; set; }
        public Type PrintedType { get; set; }
        public Delegate PrintMethod { get; set; }
    }
}
