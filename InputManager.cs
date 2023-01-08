using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class InputManager
    {
        public static List<UI> SelectableUIList = new List<UI>();
        private static int cursor = 0;
        public static bool isblocked = false;
        private static bool isSelected = false;
        private static ConsoleKey lastKey = default;

        public static int GetSelection()
        {
            isSelected = false;
            Animation.OnCursorOver(SelectableUIList[cursor]);
            while (!isSelected)
            {
                GetInput();
            }

            return cursor;
        }

        private static void GetInput()
        {
            if (isblocked)
                return;

            ConsoleKey key;
            if (lastKey != default)
            {
                key = lastKey;
            }
            else
            {
                key = Console.ReadKey(true).Key;
            }
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    Console.WriteLine("111");
                    lastKey = default;
                    --cursor;
                    if (cursor < 0)
                        cursor = SelectableUIList.Count - 1;
                    Animation.OnCursorOver(SelectableUIList[cursor]);
                    break;

                case ConsoleKey.RightArrow:
                    Console.WriteLine("222");
                    lastKey = default;
                    ++cursor;
                    if (cursor > SelectableUIList.Count - 1)
                        cursor = 0;
                    Animation.OnCursorOver(SelectableUIList[cursor]);
                    break;

                case ConsoleKey.Spacebar:
                    Console.WriteLine("333");
                    lastKey = default;
                    isSelected = true;
                    break;
                default:
                    lastKey = default;
                    break;
            }
        }

        public static void BlockInput(Task task)
        {
            while (!task.IsCompleted)
            {
                isblocked = true;
                ConsoleKey key = Console.ReadKey(true).Key;
                if (task.IsCompleted)
                {
                    lastKey = key;
                    break;
                }
            }
            isblocked = false;
        }
    }
}