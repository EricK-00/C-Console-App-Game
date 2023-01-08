using System;
using System.Data;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class Screen
	{
		const int screenSizeX = 45;
		const int screenSizeY = 25;
		public static void Initialize()
		{
			for (int i = 0; i < screenSizeY; i++)
			{
				for (int j = 0; j < screenSizeX; j++)
				{
                    Console.SetCursorPosition(2 * j, i);
                    Console.Write($"0".PadRight(2, ' '));
				}
				Console.WriteLine();
			}
		}

        public static void RenderTextWithDelay(TextArea text, int delayTime)
        {
            int strIndex = 0;
            Task task = Task.Run(() =>
            {
                for (int i = 0; i < text.Height; i++)
                {
                    for (int j = 0; j < text.Width; j++)
                    {
                        if (strIndex >= text.Text.Length)
                            goto END;

                        Console.SetCursorPosition(2 * (text.X + j), text.Y + i);
                        Console.Write(text.Text[strIndex].ToString().PadRight(2, ' '));
                        ++strIndex;
                        Task.Delay(delayTime).Wait();
                    }
                    Console.WriteLine();
                }
END: { }
            });

            InputManager.BlockInput(task);
        }

        public static void RenderText(TextArea text)
        {
            int strIndex = 0;
            for (int i = 0; i < text.Height; i++)
            {
                for (int j = 0; j < text.Width; j++)
                {
                    if (strIndex >= text.Text.Length)
                        goto END;

                    Console.SetCursorPosition(2 * (text.X + j), text.Y + i);
                    Console.Write(text.Text[strIndex].ToString().PadRight(2, ' '));
                    ++strIndex;
                }
                Console.WriteLine();
            }
END: { }
        }

		public static void Render(int x, int y, int width, int height, string[,] image)
		{
			for (int i = 0; i < height; i++)
			{
                for (int j = 0; j < width; j++)
				{
                    Console.SetCursorPosition(2 * (x + j), y + i);
					Console.Write(image[i, j].PadRight(2, ' '));
				}
			}
		}

		public static void Render(UI ui)
		{
			for (int i = 0; i < ui.Height; i++)
			{
				for (int j = 0; j < ui.Width; j++)
				{
                    Console.SetCursorPosition(2 * (ui.X + j), ui.Y + i);
                    Console.Write(ui.Contents[i, j].PadRight(2, ' '));
				}
			}
		}

		public static void Render(IAnimatable animatable)
		{
			for (int i = 0; i < animatable.Height; i++)
			{
				for (int j = 0; j < animatable.Width; j++)
				{
                    Console.SetCursorPosition(2 * (animatable.X + j), animatable.Y + i);
                    Console.Write(animatable.Contents[i, j].PadRight(2, ' '));
                }
			}
		}

		public static void Render(IAnimatable animatable, string[,] image)
		{
			for (int i = 0; i < animatable.Height; i++)
			{
				for (int j = 0; j < animatable.Width; j++)
				{
                    Console.SetCursorPosition(2 * (animatable.X + j), animatable.Y + i);
                    Console.Write(image[i, j].PadRight(2, ' '));
                }
			}
		}

        public static void RenderWithColor(IAnimatable animatable, ConsoleColor color)
        {
            for (int i = 0; i < animatable.Height; i++)
            {
                for (int j = 0; j < animatable.Width; j++)
                {
                    Console.ForegroundColor = color;
                    Console.SetCursorPosition(2 * (animatable.X + j), animatable.Y + i);
                    Console.Write(animatable.Contents[i, j].PadRight(2, ' '));
                }
            }
            Console.ResetColor();
        }

        public static void RenderWithColor(IAnimatable animatable, string[,] image, ConsoleColor color)
        {
            for (int i = 0; i < animatable.Height; i++)
            {
                for (int j = 0; j < animatable.Width; j++)
                {
					Console.ForegroundColor = color;
                    Console.SetCursorPosition(2 * (animatable.X + j), animatable.Y + i);
                    Console.Write(image[i, j].PadRight(2, ' '));
                }
            }
			Console.ResetColor();
        }

        public static void Clear(int x, int y, int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.SetCursorPosition(2 * (x + j), y + i);
                    Console.Write("  ");
                }
            }
        }

        public static void ClearText(TextArea text)
        {
            int strIndex = 0;
            for (int i = 0; i < text.Height; i++)
            {
                for (int j = 0; j < text.Width; j++)
                {
                    if (strIndex >= text.Text.Length)
                        goto END;

                    Console.SetCursorPosition(2 * (text.X + j), text.Y + i);
                    Console.Write("  ");
                    ++strIndex;
                }
                Console.WriteLine();
            }
END: { }
        }

        public static void Clear(UI ui)
        {
            for (int i = 0; i < ui.Height; i++)
            {
                for (int j = 0; j < ui.Width; j++)
                {
                    Console.SetCursorPosition(2 * (ui.X + j), ui.Y + i);
                    Console.Write("  ");
                }
            }
        }

        public static void Clear(IAnimatable animatable)
        {
            for (int i = 0; i < animatable.Height; i++)
            {
                for (int j = 0; j < animatable.Width; j++)
                {
                    Console.SetCursorPosition(2 * (animatable.X + j), animatable.Y + i);
                    Console.Write("  ");
                }
            }
        }
    }
}
