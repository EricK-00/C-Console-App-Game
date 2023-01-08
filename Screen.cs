using System;
using System.Data;

namespace CSharpConsoleAppGame
{
	internal class Screen
	{
		const int screenSizeX = 50;
		const int screenSizeY = 25;
		public static string[,] screen = new string[screenSizeY, screenSizeX];

		//public Screen()
		public static void Initialize()
		{
			for (int i = 0; i < screenSizeY; i++)
			{
				for (int j = 0; j < screenSizeX; j++)
				{
					screen[i, j] = "00";
				}
			}
		}

		public static void RenderScreen(object? o)
		{
			Console.SetCursorPosition(0, 0);
			for (int i = 0; i < screenSizeY; i++)
			{
				for (int j = 0; j < screenSizeX; j++)
				{
					Console.Write($"{screen[i, j].PadRight(2)}");
				}
				Console.WriteLine();
			}
		}

		public static void PlaceOnScreen(int x, int y, int width, int height, string[,] image)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					screen[y + i, x + j] = image[i, j];
				}
			}
		}

		public static void PlaceOnScreen(UI ui)
		{
			for (int i = 0; i < ui.Height; i++)
			{
				for (int j = 0; j < ui.Width; j++)
				{
					screen[ui.Y + i, ui.X + j] = ui.Image[i, j];
				}
			}
		}

		public static void PlaceOnScreen(IAnimatable animatable)
		{
			for (int i = 0; i < animatable.Height; i++)
			{
				for (int j = 0; j < animatable.Width; j++)
				{
					screen[animatable.Y + i, animatable.X + j] = animatable.Image[i, j];
				}
			}
		}

		public static void PlaceOnScreen(IAnimatable animatable, string[,] image)
		{
			for (int i = 0; i < animatable.Height; i++)
			{
				for (int j = 0; j < animatable.Width; j++)
				{
					screen[animatable.Y + i, animatable.X + j] = image[i, j];
				}
			}
		}
	}
}
