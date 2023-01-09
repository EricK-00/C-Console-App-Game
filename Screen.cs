using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CSharpConsoleAppGame
{
	//utf-8
	internal class Screen
	{
	    public const int SCREEN_WIDTH = 45;
		public const int SCREEN_HEIGHT = 25;
		private const string DEFAULT_WORD = "+";
		public static string[,] View { get; } = new string[SCREEN_HEIGHT, SCREEN_WIDTH];
		public static void Initialize()
		{
			for (int i = 0; i < SCREEN_HEIGHT; i++)
			{
				for (int j = 0; j < SCREEN_WIDTH; j++)
				{
                    Console.SetCursorPosition(2 * j, i);
                    Console.Write(DEFAULT_WORD.PadRight(2, ' '));
					View[i, j] = DEFAULT_WORD;
				}
				Console.WriteLine();
			}
		}

		public static void RenderScreenView()
		{
			Console.SetCursorPosition(0, 0);
			for (int i = 0; i < SCREEN_HEIGHT; i++)
			{
				for (int j = 0; j < SCREEN_WIDTH; j++)
				{
					if (Encoding.Default.GetBytes(View[i, j]).Length == 1)
					{
						Console.Write(View[i, j].PadRight(2, ' '));
					}
					else
					{
						Console.Write(View[i, j]);
					}
				}
				Console.WriteLine();
			}
		}

        public static void RenderTextWithDelay(TextArea text, int delayTime)
        {
			int viewPosX, viewPosY;

            Task task = Task.Run(() =>
            {
                for (int i = 0; i < text.Height; i++)
                {
                    for (int j = 0; j < text.Width; j++)
                    {
                        if (text.ContentsSize <= i * text.Width + j)
                            goto END;

						viewPosX = text.X + j;
						viewPosY = text.Y + i;
						Console.SetCursorPosition(2 * viewPosX, viewPosY);

						if (Encoding.Default.GetBytes(text.Contents[i, j]).Length == 1)
						{
							Console.Write(text.Contents[i, j].PadRight(2, ' '));
						}
						else
						{
							Console.Write(text.Contents[i, j]);
						}
						View[viewPosY, viewPosX] = text.Contents[i, j];

						Task.Delay(delayTime).Wait();
                    }
                    Console.WriteLine();
                }
END: { }
            });

			task.Wait();
            InputManager.ClearInputBuffer();
        }

		public static void Render(int x, int y, int width, int height, string[,] image)
		{
			int viewPosX, viewPosY;

			for (int i = 0; i < height; i++)
			{
                for (int j = 0; j < width; j++)
				{
					viewPosX = x + j;
					viewPosY = y + i;
                    Console.SetCursorPosition(2 * viewPosX, viewPosY);

					if (Encoding.Default.GetBytes(image[i, j]).Length == 1)
						Console.Write(image[i, j].PadRight(2, ' '));
					else
						Console.Write(image[i, j]);

					View[viewPosY, viewPosX] = image[i, j];
				}
			}
		}

		public static void Render(UI ui)
		{
			int viewPosX, viewPosY;

			for (int i = 0; i < ui.Height; i++)
			{
				for (int j = 0; j < ui.Width; j++)
				{
					if (ui.ContentsSize <= i * ui.Width + j)
						goto END;

					viewPosX = ui.X + j;
					viewPosY = ui.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);

					if (Encoding.Default.GetBytes(ui.Contents[i, j]).Length == 1)
						Console.Write(ui.Contents[i, j].PadRight(2, ' '));
					else
						Console.Write(ui.Contents[i, j]);

					View[viewPosY, viewPosX] = ui.Contents[i, j];
				}
			}

		END: { }
		}

		public static void Render(IAnimatable animatable)
		{
			int viewPosX, viewPosY;

			for (int i = 0; i < animatable.Height; i++)
			{
				for (int j = 0; j < animatable.Width; j++)
				{
					if (animatable.ContentsSize <= i * animatable.Width + j)
						goto END;


					viewPosX = animatable.X + j;
					viewPosY = animatable.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);

					if (Encoding.Default.GetBytes(animatable.Contents[i, j]).Length == 1)
						Console.Write(animatable.Contents[i, j].PadRight(2, ' '));
					else
						Console.Write(animatable.Contents[i, j]);

					View[viewPosY, viewPosX] = animatable.Contents[i, j];
				}
			}

		END: { }
		}

		public static void Render(IAnimatable animatable, string[,] image)
		{
			int viewPosX, viewPosY;

			if (image.GetLength(0) != animatable.Height || image.GetLength(1) != animatable.Width)
				return;

			for (int i = 0; i < image.GetLength(0); i++)
			{
				for (int j = 0; j < image.GetLength(1); j++)
				{
					if (animatable.ContentsSize <= i * image.GetLength(1) + j)
						goto END;

					viewPosX = animatable.X + j;
					viewPosY = animatable.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);

					if (Encoding.Default.GetBytes(image[i, j]).Length == 1)
						Console.Write(image[i, j].PadRight(2, ' '));
					else
						Console.Write(image[i, j]);

					View[viewPosY, viewPosX] = image[i, j];
				}
			}

		END: { }
		}

        public static void RenderWithColor(IAnimatable animatable, ConsoleColor color)
        {
			int viewPosX, viewPosY;

			for (int i = 0; i < animatable.Height; i++)
            {
                for (int j = 0; j < animatable.Width; j++)
                {
					if (animatable.ContentsSize <= i * animatable.Width + j)
						goto END;

					viewPosX = animatable.X + j;
					viewPosY = animatable.Y + i;
					Console.ForegroundColor = color;
                    Console.SetCursorPosition(2 * viewPosX, viewPosY);

					if (Encoding.Default.GetBytes(animatable.Contents[i, j]).Length == 1)
						Console.Write(animatable.Contents[i, j].PadRight(2, ' '));
					else
						Console.Write(animatable.Contents[i, j]);

					View[viewPosY, viewPosX] = animatable.Contents[i, j];
				}
            }
		END: { }
			Console.ResetColor();
        }

        public static void RenderWithColor(IAnimatable animatable, string[,] image, ConsoleColor color)
        {
			int viewPosX, viewPosY;

			if (image.GetLength(0) != animatable.Height || image.GetLength(1) != animatable.Width)
				return;

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    if (animatable.ContentsSize <= i * image.GetLength(1) + j)
                        goto END;

					Console.ForegroundColor = color;

					viewPosX = animatable.X + j;
					viewPosY = animatable.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);

					if (Encoding.Default.GetBytes(image[i, j]).Length == 1)
						Console.Write(image[i, j].PadRight(2, ' '));
					else
						Console.Write(image[i, j]);

					View[viewPosY, viewPosX] = image[i, j];
				}
            }
		END: { }
			Console.ResetColor();
        }

        public static void Clear(int x, int y, int width, int height)
        {
			int viewPosX, viewPosY;

			for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
					viewPosX = x + j;
					viewPosY = y + i;
                    Console.SetCursorPosition(2 * viewPosX,  viewPosY);
                    Console.Write("  ");

					View[viewPosY, viewPosX] = "  ";
                }
            }
        }

        public static void Clear(UI ui)
        {
			int viewPosX, viewPosY;

			for (int i = 0; i < ui.Height; i++)
            {
                for (int j = 0; j < ui.Width; j++)
                {
					if (ui.ContentsSize <= i * ui.Width + j)
						goto END;

					viewPosX = ui.X + j;
					viewPosY = ui.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);
                    Console.Write("  ");

					View[viewPosY, viewPosX] = "  ";
				}
            }
		END: { }
		}

        public static void Clear(IAnimatable animatable)
        {
			int viewPosX, viewPosY;

			for (int i = 0; i < animatable.Height; i++)
            {
                for (int j = 0; j < animatable.Width; j++)
                {
					if (animatable.ContentsSize <= i * animatable.Width + j)
						goto END;

					viewPosX = animatable.X + j;
					viewPosY = animatable.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);
                    Console.Write("  ");

					View[viewPosY, viewPosX] = "  ";
				}
            }
		END: { }
		}

		public static void ClearAll()
		{
			int viewPosX, viewPosY;

			for (int i = 0; i < SCREEN_HEIGHT; i++)
			{
				for (int j = 0; j < SCREEN_WIDTH; j++)
				{
					viewPosX = 0 + j;
					viewPosY = 0 + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);
					Console.Write("  ");

					View[viewPosY, viewPosX] = "  ";
				}
			}
		}
    }
}
