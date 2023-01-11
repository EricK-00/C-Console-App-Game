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
		public const int DEFAULT_DELAY_TERM = 50;

        public static void Blink(IAnimatable animatableObject, int blinkCount, char paddingChar, int delayTerm)
		{
			Task task = Task.Run(() => { BlinkByChar(animatableObject, blinkCount, paddingChar, delayTerm); });
			task.Wait();
			InputBufferCleaner.Clear();
        }

        public static void BlinkWithColor(IAnimatable animatableObject, int blinkCount, ConsoleColor color, int delayTerm)
        {
            Task task = Task.Run(() => { BlinkByColor(animatableObject, blinkCount, color, delayTerm); });
			task.Wait();
			InputBufferCleaner.Clear();
		}

		public static void BlinkView(int blinkCount, char paddingChar, int delayTerm)
		{
			Task task = Task.Run(() => { BlinkByChar(blinkCount, paddingChar, delayTerm); });
			task.Wait();
			InputBufferCleaner.Clear();
		}

		public static void BlinkViewWithColor(int blinkCount, ConsoleColor color, int delayTerm)
		{
			Task task = Task.Run(() => { BlinkByColor(blinkCount, color, delayTerm); });
			task.Wait();
			InputBufferCleaner.Clear();
		}

        public static void OnCursorOver(IAnimatable animatableObject)
        {
			Task task = Task.Run(() => { BlinkByColor(animatableObject, 3, ConsoleColor.Black, 30); });
			task.Wait();
			InputBufferCleaner.Clear();
		}

		public static void Selected(IAnimatable animatableObject)
		{
			Task task = Task.Run(() => { BlinkByColor(animatableObject, 5, ConsoleColor.DarkYellow, 70); });
			task.Wait();
			InputBufferCleaner.Clear();
		}

		public static void Fade(IAnimatable animatableObject, int delayTerm)
		{
			Task task = Task.Run(() => { FadeObject(animatableObject, delayTerm); }) ;
			task.Wait();
			InputBufferCleaner.Clear();
		}

		public static void FadeView(int delayTerm)
		{
			Task task = Task.Run(() => { FadeObject(delayTerm); });
			task.Wait();
			InputBufferCleaner.Clear();
		}

		private static void BlinkByChar(IAnimatable animatableObject, int blinkCount, char paddingChar, int delayTerm)
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
				Task.Delay(delayTerm).Wait();
				Screen.Render(animatableObject);
				Task.Delay(delayTerm).Wait();
			}
		}

		private static void BlinkByColor(IAnimatable animatableObject, int blinkCount, ConsoleColor color, int delayTerm)
		{
			for (int i = 0; i < blinkCount; i++)
			{
				Screen.RenderWithColor(animatableObject, color);
				Task.Delay(delayTerm).Wait();
				Screen.Render(animatableObject);
				Task.Delay(delayTerm).Wait();
			}
		}

		private static void BlinkByChar(int blinkCount, char paddingChar, int delayTerm)
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
				Screen.Render(0, 0, Screen.WIDTH, Screen.HEIGHT, blinkImage);
				Task.Delay(delayTerm).Wait();
				Screen.Render(0, 0, Screen.WIDTH, Screen.HEIGHT, viewImage);
				Task.Delay(delayTerm).Wait();
			}
		}

		private static void BlinkByColor(int blinkCount, ConsoleColor color, int delayTerm)
		{
			for (int i = 0; i < blinkCount; i++)
			{
				Console.ForegroundColor = color;
				Screen.RenderScreenView();
				Task.Delay(delayTerm).Wait();
				Console.ResetColor();
				Screen.RenderScreenView();
				Task.Delay(delayTerm).Wait();
			}
		}

		private static void FadeObject(IAnimatable animatableObject, int delayTerm)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Screen.Render(animatableObject);
			Task.Delay(delayTerm).Wait();
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Screen.Render(animatableObject);
			Task.Delay(delayTerm).Wait();
			Console.ForegroundColor = ConsoleColor.Black;
			Screen.Render(animatableObject);
			Task.Delay(delayTerm).Wait();
			Screen.Clear(animatableObject);
		}

		private static void FadeObject(int delayTerm)
		{
			string[,] viewImage = new string[Screen.View.GetLength(0), Screen.View.GetLength(1)];

			for (int i = 0; i < viewImage.GetLength(0); i++)
			{
				for (int j = 0; j < viewImage.GetLength(1); j++)
				{
					viewImage[i, j] = Screen.View[i, j];
				}
			}

			Console.ForegroundColor = ConsoleColor.Gray;
			Screen.Render(0, 0, Screen.WIDTH, Screen.HEIGHT, viewImage);
			Task.Delay(delayTerm).Wait();
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Screen.Render(0, 0, Screen.WIDTH, Screen.HEIGHT, viewImage);
			Task.Delay(delayTerm).Wait();
			Console.ForegroundColor = ConsoleColor.Black;
			Screen.Render(0, 0, Screen.WIDTH, Screen.HEIGHT, viewImage);
			Task.Delay(delayTerm).Wait();
			Screen.ClearAll();
		}
	}
}