using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CSharpConsoleAppGame
{
    internal class UIPreset
    {
        public const int PRINT_DELAY = 15;

        public const int WINDOW_X = 0;
        public const int WINDOW_Y = 15;
        public const int WINDOW_WIDTH = Screen.WIDTH;
        public const int WINDOW_HEIGHT = 10;

        public static Window CreateScriptWindow()
        {
            return new Window(WINDOW_X, WINDOW_Y, WINDOW_WIDTH, WINDOW_HEIGHT, ' ');
        }

        public static Window CreateScriptWindow(char paddingChar)
        {
            return new Window(0, 15, Screen.WIDTH, 10, paddingChar);
        }

        public static TextArea CreateScriptTextArea(string text, int line, bool isDelayed)
        {
            ClearScript(line);
            if (isDelayed)
                return new TextArea(WINDOW_X + 1, WINDOW_Y + line, text, PRINT_DELAY);
            else
                return new TextArea(WINDOW_X + 1, WINDOW_Y + line, text);
        }

		public static TextArea CreateScriptTextArea(string text, int line, int delayTerm)
		{
			ClearScript(line);
			return new TextArea(WINDOW_X + 1, WINDOW_Y + line, text, delayTerm);
		}

		public static void ClearScript(int line)
        {
            Screen.Clear(WINDOW_X + 1, WINDOW_Y + line, WINDOW_WIDTH - 2, 1);
        }

        public static void ClearAllScript()
        {
            Screen.Clear(WINDOW_X + 1, WINDOW_Y + 1, WINDOW_WIDTH - 2, WINDOW_HEIGHT - 2);
        }
    }
}