using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class InputBufferCleaner
    {
		public static void Clear()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
            }
        }
    }
}