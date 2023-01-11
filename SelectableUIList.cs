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

		public SelectableUI()
		{

		}

		public SelectableUI(List<UI> uiList_, CursorMoveMode mode_)
		{
			if (uiList != null)
				ClearUI();

			uiList = uiList_;
			mode = mode_;

			if (mode == CursorMoveMode.Square)
				mode = CursorMoveMode.Horizonal;
		}

		public SelectableUI(List<UI> uiList_, CursorMoveMode mode_, int selectionRow, int selectionCol)
		{
			uiList = uiList_;
			mode = mode_;

			row = selectionRow;
			col = selectionCol;
		}

		public void ClearUI()
		{
			foreach (UI ui in uiList)
				Screen.Clear(ui);

			//UIList.Clear();
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
							if (cursorPos % col <= 0)
								cursorPos += (col - 1);
							else
								--cursorPos;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;
						case ConsoleKey.RightArrow:
							if (cursorPos % col >= col - 1)
								cursorPos -= (col - 1);
							else
								++cursorPos;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;
						case ConsoleKey.UpArrow:
							cursorPos -= row;
							if (cursorPos < 0)
								cursorPos += uiList.Count;
							Animation.OnCursorOver(uiList[cursorPos]);
							break;
						case ConsoleKey.DownArrow:
							cursorPos += row;
							if (cursorPos >= uiList.Count)
								cursorPos -= uiList.Count;
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
