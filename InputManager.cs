using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class InputManager
    {
        private static int cursor = -1;
        private static bool isSelected = false;

        public static int GetSelection(List<UI> container)
        {
            if (container == null)
                return -1;

            isSelected = false;
            while (!isSelected)
            {
                GetInput(container);
            }

            return cursor;
        }

        public static void SetCursorDefault()
        {
            cursor = -1;
        }

        private static void GetInput(List<UI> container)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    --cursor;
                    if (cursor < 0)
                        cursor = container.Count - 1;
                    Animation.OnCursorOver(container[cursor]);
                    break;

                case ConsoleKey.RightArrow:
                    ++cursor;
                    if (cursor > container.Count - 1)
                        cursor = 0;
                    Animation.OnCursorOver(container[cursor]);
                    break;

                case ConsoleKey.Spacebar:
                    if (cursor == -1)
                        return;
                    isSelected = true;
                    Animation.OnCursorOver(container[cursor]);
                    break;
                default:
                    break;
            }
        }

        public static void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }
    }
}