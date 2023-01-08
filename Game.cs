using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class Game
	{
		int winCount;
		bool isGameover;
		bool winTheBattle;
		string gameState;

		private void LoadData()
		{

		}

		public void PlayGame()
		{
            Console.CursorVisible = false;
            Screen.Initialize();

            Window selectionWindow = new Window(2, 2, 10, 10, ' ');
            Window viewWindow = new Window(0, 15, 45, 10, ' ');
            Screen.Render(viewWindow);

            Console.ReadLine();

            InputManager.SelectableUIList.Add(new TextArea(1, 1, 5, 1, "AAAAA"));
            InputManager.SelectableUIList.Add(new TextArea(1, 2, 5, 1, "BBBBB"));
            InputManager.SelectableUIList.Add(new TextArea(1, 3, 5, 1, "CCCCC"));
            foreach (var ui in InputManager.SelectableUIList)
                Screen.Render(ui);

            Animation animation = new Animation();
            switch (InputManager.GetSelection())
            {
                case 0:
                    (InputManager.SelectableUIList[0] as TextArea)?.Rewrite("1.새로 시작하기, 2.불러오기");
                    break;

                case 1:
                    (InputManager.SelectableUIList[1] as TextArea)?.Rewrite("1.새로 불러오기");
                    break;

                case 2:
                    (InputManager.SelectableUIList[2] as TextArea)?.Rewrite("1 2 3 4 5");
                    break;
            }
            InputManager.SelectableUIList.Clear();

            Console.WriteLine("????????????????");
            Console.ReadLine();

            Animation.Blink(viewWindow, 5);

            Console.ReadLine();

            ConsoleKeyInfo k = Console.ReadKey();
            if (k.Key == ConsoleKey.D1)
            {

            }

            Console.ReadLine();
        }
	}
}