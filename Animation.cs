using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{

	internal class Animation
	{
        public static void Blink(IAnimatable animatableObject, int blinkCount, char paddingChar)
		{
			Task task = Task.Run(() => { BlinkByChar(animatableObject, blinkCount, paddingChar); });
			task.Wait();
			InputManager.ClearInputBuffer();
        }

        public static void BlinkWithColor(IAnimatable animatableObject, int blinkCount, ConsoleColor color)
        {
            Task task = Task.Run(() => { BlinkByColor(animatableObject, blinkCount, color); });
			task.Wait();
			InputManager.ClearInputBuffer();
		}

		public static void BlinkView(int blinkCount, char paddingChar)
		{
			Task task = Task.Run(() => { BlinkByChar(blinkCount, paddingChar); });
			task.Wait();
			InputManager.ClearInputBuffer();
		}

		public static void BlinkViewWithColor(int blinkCount, ConsoleColor color)
		{
			Task task = Task.Run(() => { BlinkByColor(blinkCount, color); });
			task.Wait();
			InputManager.ClearInputBuffer();
		}

        public static void OnCursorOver(IAnimatable animatableObject)
        {
			Task task = Task.Run(() => { BlinkByColor(animatableObject, 2, ConsoleColor.Black); });
			task.Wait();
			InputManager.ClearInputBuffer();
		}

        private static void BlinkByChar(IAnimatable animatableObject, int blinkCount, char paddingChar)
		{
			string[,] blinkImage = new string[animatableObject.Contents.GetLength(0), animatableObject.Contents.GetLength(1)];
			for (int i = 0; i < blinkImage.GetLength(0); i++)
			{
				for (int j = 0; j < blinkImage.GetLength(1); j++)
				{
					if (animatableObject.ContentsSize <= i * j)
						goto LOOP_END;

					blinkImage[i, j] = paddingChar.ToString();
				}
			}
		LOOP_END: { }

			for (int i = 0; i < blinkCount; i++)
			{
				Screen.Render(animatableObject, blinkImage);
				Task.Delay(50).Wait();
				Screen.Render(animatableObject);
				Task.Delay(50).Wait();
			}
		}

		private static void BlinkByColor(IAnimatable animatableObject, int blinkCount, ConsoleColor color)
		{
			for (int i = 0; i < blinkCount; i++)
			{
				Screen.RenderWithColor(animatableObject, color);
				Task.Delay(50).Wait();
				Screen.Render(animatableObject);
				Task.Delay(50).Wait();
			}
		}

		private static void BlinkByChar(int blinkCount, char paddingChar)
		{
			string[,] blinkImage = new string[Screen.View.GetLength(0), Screen.View.GetLength(1)];
			string[,] viewImage = new string[Screen.View.GetLength(0), Screen.View.GetLength(1)];

			for (int i = 0; i < blinkImage.GetLength(0); i++)
			{
				for (int j = 0; j < blinkImage.GetLength(1); j++)
				{
					blinkImage[i, j] = paddingChar.ToString();
					viewImage[i, j] = Screen.View[i, j];
				}
			}

			for (int i = 0; i < blinkCount; i++)
			{
				Screen.Render(0, 0, Screen.SCREEN_WIDTH, Screen.SCREEN_HEIGHT, blinkImage);
				Task.Delay(50).Wait();
				Screen.Render(0, 0, Screen.SCREEN_WIDTH, Screen.SCREEN_HEIGHT, viewImage);
				Task.Delay(50).Wait();
			}
		}

		private static void BlinkByColor(int blinkCount, ConsoleColor color)
		{
			for (int i = 0; i < blinkCount; i++)
			{
				Console.ForegroundColor = color;
				Screen.RenderScreenView();
				Task.Delay(50).Wait();
				Console.ResetColor();
				Screen.RenderScreenView();
				Task.Delay(50).Wait();
			}
		}
	}
}