using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	enum CursorMoveMode
	{
		Horizonal = 0,
		Vertical,
		Square
	}

	internal class SelectableUI
	{
		private List<UI> uiList = new List<UI>();
		private int cursorPos = -1;
		private readonly CursorMoveMode mode;
		private int row;
		private int col;

		public SelectableUI(List<UI> uiList_, CursorMoveMode mode_)
		{
			uiList = uiList_;
			mode = mode_;

			if (mode == CursorMoveMode.Square)
				mode = CursorMoveMode.Horizonal;

			HideKeyGuide();
			ShowKeyGuide();
		}

		public SelectableUI(List<UI> uiList_, CursorMoveMode mode_, int selectionRow, int selectionCol)
		{
            uiList = uiList_;
			mode = mode_;

			row = selectionRow;
			col = selectionCol;

			HideKeyGuide();
			ShowKeyGuide();
		}

        private void ShowKeyGuide()
        {
			string keyGuide = string.Empty;
            switch (mode)
			{
				case CursorMoveMode.Horizonal:
                    keyGuide = $"←,→:이동|스페이스바:선택|q:취소";
                    break;
                case CursorMoveMode.Vertical:
					keyGuide = $"↑,↓:이동|스페이스바:선택|q:취소";
                    break;
                case CursorMoveMode.Square:
                    keyGuide = $"←,↑,↓,→:이동|스페이스바:선택|q:취소";
                    break;
            }

			int posX = 0;
			int posY = Screen.HEIGHT + 1;
			for (int i = 0; i < keyGuide.Length; i++)
			{
                Console.SetCursorPosition(posX, posY);
				if (keyGuide[i] == '|')
				{
					posX = 0;
					++posY;
				}
				else
				{
					Console.Write(keyGuide[i]);
					posX += 2;
				}
            }
        }

		public static void HideKeyGuide()
		{
            Console.SetCursorPosition(0, Screen.HEIGHT + 1);
			Console.WriteLine("                                                          ");
			Console.WriteLine("                                                          ");
			Console.WriteLine("                                                          ");
        }

        public void ClearUI()
		{
			foreach (UI ui in uiList)
				Screen.Clear(ui);
			HideKeyGuide();
			uiList.Clear();
		}

		public int GetSelection()
		{
			if (mode == CursorMoveMode.Horizonal)
			{
				while (true)
				{
					switch (Console.ReadKey(true).Key)
					{
						case ConsoleKey.LeftArrow:
							--cursorPos;
							if (cursorPos < 0)
								cursorPos = uiList.Count - 1;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.RightArrow:
							++cursorPos;
							if (cursorPos >= uiList.Count)
								cursorPos = 0;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.Spacebar:
							if (cursorPos == -1)
								break;

							return cursorPos;
						case ConsoleKey.Q:
							return -1;
					}
				}
			}
			else if (mode == CursorMoveMode.Vertical)
			{
				while (true)
				{
					switch (Console.ReadKey(true).Key)
					{
						case ConsoleKey.UpArrow:
							--cursorPos;
							if (cursorPos < 0)
								cursorPos = uiList.Count - 1;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.DownArrow:
							++cursorPos;
							if (cursorPos >= uiList.Count)
								cursorPos = 0;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.Spacebar:
							if (cursorPos == -1)
								break;

							return cursorPos;
						case ConsoleKey.Q:
							return -1;
					}
				}
			}
			else
			{
				while (true)
				{
					switch (Console.ReadKey(true).Key)
					{
						case ConsoleKey.LeftArrow:
							if (cursorPos < 0)
								cursorPos = col - 1;
							else if (cursorPos % col <= 0)
								cursorPos += (col - 1);
							else
								--cursorPos;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.RightArrow:
                            if (cursorPos < 0)
                                cursorPos = 0;
                            else if (cursorPos % col >= col - 1)
								cursorPos -= (col - 1);
							else
								++cursorPos;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.UpArrow:
							if (cursorPos < 0)
								cursorPos = (row - 1) * col;
							else if (cursorPos / col == 0)
								cursorPos += (row - 1) * col;
							else
								cursorPos -= col;
                            Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.DownArrow:
							if (cursorPos < 0)
								cursorPos = 0;
							else if (cursorPos / col == row - 1)
								cursorPos -= (row - 1) * col;
							else
								cursorPos += col;
                            Animation.OnCursorOver(uiList[cursorPos]);
							break;

						case ConsoleKey.Spacebar:
							if (cursorPos == -1)
								break;
							return cursorPos;

						case ConsoleKey.Q:
							return -1;
					}
				}
			}
		}

		public void SetDefaultCursorPos()
		{
			cursorPos = -1;
		}
	}
}
