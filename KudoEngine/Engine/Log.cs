using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudoEngine.Engine
{
    internal class Log
    {
        /// <summary>
        /// Normal Console Log
        /// </summary>
        public static void write(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Highlighted Console Log
        /// </summary>
        public static void mark(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Warning Console Log
        /// </summary>
        public static void warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"[WARN] {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Error Console Log
        /// </summary>
        public static void error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERR] {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
