using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PrettyConsoleUtils.Model;

namespace PrettyConsoleUtils
{
    public class PrettyConsole
    {
        private static string lockString = "helloWorld";
        private Dictionary<Type, Delegate> printingMethods;

        public PrettyConsole()
        {
            ConsoleProvider = new DefaultConsoleWrapper();
            printingMethods = new Dictionary<Type, Delegate>();
        }
        public PrettyConsole(ConsoleProvider provider)
        {
            ConsoleProvider = provider;
            printingMethods = new Dictionary<Type, Delegate>();
        }

        public ConsoleProvider ConsoleProvider { get; private set; }

        #region "Read / ReadKey / ReadLine"

        public int Read()
        {
            return ConsoleProvider.Read();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return ConsoleProvider.ReadKey(false);
        }
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return ConsoleProvider.ReadKey(intercept);
        }

        public string ReadLine()
        {
            return ConsoleProvider.ReadLine();
        }

        #endregion

        #region "Write / WriteLine"

        public void Write(string txt)
        {
            var args = CreateWriteArgs(txt, WriteType.Write, ConsoleProvider.Foreground, ConsoleProvider.Background, ConsoleProvider.CursorX, ConsoleProvider.CursorY);
            TryFireBeforeWrite(args);

            lock (lockString) ConsoleProvider.Write(txt);

            TryFireAfterWrite(args);
        }
        public void Write(string txt, ConsoleColor foreground)
        {
            var args = CreateWriteArgs(txt, WriteType.Write, foreground, ConsoleProvider.Background, ConsoleProvider.CursorX, ConsoleProvider.CursorY);
            TryFireBeforeWrite(args);

            lock (lockString)
            {
                var oldForeground = ConsoleProvider.Foreground;
                ConsoleProvider.Foreground = foreground;
                Write(txt);
                ConsoleProvider.Foreground = oldForeground;
            }

            TryFireAfterWrite(args);
        }
        public void Write(string txt, ConsoleColor foreground, ConsoleColor background)
        {
            var args = CreateWriteArgs(txt, WriteType.Write, foreground, background, ConsoleProvider.CursorX, ConsoleProvider.CursorY);
            TryFireBeforeWrite(args);

            lock (lockString)
            {
                var oldForeground = ConsoleProvider.Foreground;
                var oldBackground = ConsoleProvider.Background;
                ConsoleProvider.Foreground = foreground;
                ConsoleProvider.Background = background;
                Write(txt);
                ConsoleProvider.Foreground = oldForeground;
                ConsoleProvider.Background = oldBackground;
            }

            TryFireAfterWrite(args);
        }

        public void WriteAt(string txt, int x, int y)
        {
            var args = CreateWriteArgs(txt, WriteType.WriteAt, ConsoleProvider.Foreground, ConsoleProvider.Background, x, y);
            TryFireBeforeWrite(args);

            lock (lockString)
            {
                var oldX = Console.CursorLeft;
                var oldY = Console.CursorTop;
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.Write(txt);
                Console.CursorLeft = oldX;
                Console.CursorTop = oldY;
            }

            TryFireAfterWrite(args);
        }
        public void WriteAt(string txt, ConsoleColor foreground, int x, int y)
        {
            var args = CreateWriteArgs(txt, WriteType.WriteAt, foreground, ConsoleProvider.Background, x, y);
            TryFireBeforeWrite(args);

            lock (lockString)
            {
                var oldX = Console.CursorLeft;
                var oldY = Console.CursorTop;
                var oldForeground = Console.ForegroundColor;
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.ForegroundColor = foreground;
                Console.Write(txt);
                Console.CursorLeft = oldX;
                Console.CursorTop = oldY;
                Console.ForegroundColor = oldForeground;
            }

            TryFireAfterWrite(args);
        }
        public void WriteAt(string txt, ConsoleColor foreground, ConsoleColor background, int x, int y)
        {
            var args = CreateWriteArgs(txt, WriteType.WriteAt, foreground, background, x, y);
            TryFireBeforeWrite(args);

            lock (lockString)
            {
                var oldX = Console.CursorLeft;
                var oldY = Console.CursorTop;
                var oldForeground = Console.ForegroundColor;
                var oldBackground = Console.BackgroundColor;
                Console.CursorLeft = x;
                Console.CursorTop = y;
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                Console.Write(txt);
                Console.CursorLeft = oldX;
                Console.CursorTop = oldY;
                Console.ForegroundColor = oldForeground;
                Console.BackgroundColor = oldBackground;
            }

            TryFireAfterWrite(args);
        }

        public void WriteLine(string txt)
        {
            var args = CreateWriteArgs(txt, WriteType.WriteLine, ConsoleProvider.Foreground, ConsoleProvider.Background, ConsoleProvider.CursorX, ConsoleProvider.CursorY);
            TryFireBeforeWrite(args);

            lock (lockString) Write(txt + Environment.NewLine);

            TryFireAfterWrite(args);
        }
        public void WriteLine(string txt, ConsoleColor foreground)
        {
            var args = CreateWriteArgs(txt, WriteType.WriteLine, foreground, ConsoleProvider.Background, ConsoleProvider.CursorX, ConsoleProvider.CursorY);
            TryFireBeforeWrite(args);

            lock (lockString)
            {
                var oldForeground = ConsoleProvider.Foreground;
                ConsoleProvider.Foreground = foreground;
                WriteLine(txt);
                ConsoleProvider.Foreground = oldForeground;
            }

            TryFireAfterWrite(args);
        }
        public void WriteLine(string txt, ConsoleColor foreground, ConsoleColor background)
        {
            var args = CreateWriteArgs(txt, WriteType.WriteLine, foreground, background, ConsoleProvider.CursorX, ConsoleProvider.CursorY);
            TryFireBeforeWrite(args);

            lock (lockString)
            {
                var oldForeground = ConsoleProvider.Foreground;
                var oldBackground = ConsoleProvider.Background;
                ConsoleProvider.Foreground = foreground;
                ConsoleProvider.Background = background;
                WriteLine(txt);
                ConsoleProvider.Foreground = oldForeground;
                ConsoleProvider.Background = oldBackground;
            }

            TryFireAfterWrite(args);
        }
        
        #endregion

        #region "Print"

        public void Print(IPrettyConsoleObject obj)
        {
            obj.PrintToConsole(this);
        }
        public void PrintObject<T>(T obj)
        {
            var type = typeof(T);
            if (!IsTypeTaught<T>()) throw new TypeNotTaughtException(type);

            TypePrintingDelegate<T> printMethod = (TypePrintingDelegate<T>)printingMethods[type];
            printMethod(this, obj);
        }
        
        #endregion

        #region "Type Method Teaching"

        public bool IsTypeTaught<T>()
        {
            var type = typeof(T);
            return printingMethods.ContainsKey(type);
        }
        public void TeachType<T>(TypePrintingDelegate<T> typePrintingMethod)
        {
            var type = typeof(T);
            if (printingMethods.ContainsKey(type)) throw new TypeAlreadyTaughtException(type);

            printingMethods.Add(type, typePrintingMethod);
        }
        public void UnteachType<T>()
        {
            var type = typeof(T);
            printingMethods.Remove(type);

            if (!printingMethods.ContainsKey(type)) throw new TypeNotTaughtException();
        }

        #endregion

        #region "Events and Delegates"

        public delegate void WriteDelegate(PrettyConsole sender, WriteEventArgs e);
        public event WriteDelegate BeforeWrite;
        public event WriteDelegate AfterWrite;

        private void TryFireAfterWrite(WriteEventArgs args)
        {
            try
            {
                AfterWrite(this, args);
            }
            catch { }
        }
        private void TryFireBeforeWrite(WriteEventArgs args)
        {
            try
            {
                BeforeWrite(this, args);
            }
            catch { }
        }
        private WriteEventArgs CreateWriteArgs(object contents, WriteType type, ConsoleColor foreground, ConsoleColor background, int x, int y)
        {
            var args = new WriteEventArgs();
            args.Background = background;
            args.Foreground = foreground;
            args.X = x;
            args.Y = y;
            args.WriteType = type;
            args.Contents = contents;
            return args;
        }

        #endregion
    }
}
