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
	    public const int WIDTH = 45;
		public const int HEIGHT = 25;
		public static string[,] View { get; } = new string[HEIGHT, WIDTH];

		static Screen()
		{
			for (int i = 0; i < HEIGHT; i++)
			{
				for (int j = 0; j < WIDTH; j++)
				{
					View[i, j] = " ";
				}
			}
		}
		public static void RenderScreenOutLine()
		{
			new Window(0, 0, WIDTH, HEIGHT, ' ', true);
        }

		public static void RenderScreenView()
		{
			Console.SetCursorPosition(0, 0);
			for (int i = 0; i < HEIGHT; i++)
			{
				for (int j = 0; j < WIDTH; j++)
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

		public static void RenderBitMap(ImageArea image, string oneValue, string zeroValueView, string oneValueView)
		{
			int viewPosX, viewPosY;

			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					viewPosX = image.X + j;
					viewPosY = image.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);

					if (image.Contents[i, j] == oneValue)
					{
						if (Encoding.Default.GetBytes(oneValueView).Length == 1)
						{
							Console.Write(oneValueView.PadRight(2, ' '));
						}
						else
						{
							Console.Write(oneValueView);
						}
						View[viewPosY, viewPosX] = oneValueView;
					}
					else
					{
						if (Encoding.Default.GetBytes(zeroValueView).Length == 1)
						{
							Console.Write(zeroValueView.PadRight(2, ' '));
						}
						else
						{
							Console.Write(zeroValueView);
						}
						View[viewPosY, viewPosX] = zeroValueView;
					}
				}
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
            });

			task.Wait();
            InputBufferCleaner.Clear();
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
		}

		public static void Render(IAnimatable animatable)
		{
			int viewPosX, viewPosY;

			for (int i = 0; i < animatable.Height; i++)
			{
				for (int j = 0; j < animatable.Width; j++)
				{
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
		}

		public static void Render(IAnimatable animatable, string[,] image)
		{
			int viewPosX, viewPosY;

			if (image.GetLength(0) * image.GetLength(1) != animatable.ContentsSize)
				return;

			for (int i = 0; i < image.GetLength(0); i++)
			{
				for (int j = 0; j < image.GetLength(1); j++)
				{
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
		}

        public static void RenderWithColor(IAnimatable animatable, ConsoleColor color)
        {
			int viewPosX, viewPosY;

			for (int i = 0; i < animatable.Height; i++)
            {
                for (int j = 0; j < animatable.Width; j++)
                {
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
			Console.ResetColor();
        }

        public static void RenderWithColor(IAnimatable animatable, string[,] image, ConsoleColor color)
        {
			int viewPosX, viewPosY;

			if (image.GetLength(0) * image.GetLength(1) != animatable.ContentsSize)
				return;

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {

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
					viewPosX = ui.X + j;
					viewPosY = ui.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);
                    Console.Write("  ");

					View[viewPosY, viewPosX] = "  ";
				}
            }
		}

        public static void Clear(IAnimatable animatable)
        {
			int viewPosX, viewPosY;

			for (int i = 0; i < animatable.Height; i++)
            {
                for (int j = 0; j < animatable.Width; j++)
                {
					viewPosX = animatable.X + j;
					viewPosY = animatable.Y + i;
					Console.SetCursorPosition(2 * viewPosX, viewPosY);
                    Console.Write("  ");

					View[viewPosY, viewPosX] = "  ";
				}
            }
		}

		public static void ClearAll()
		{
			int viewPosX, viewPosY;

			for (int i = 0; i < HEIGHT; i++)
			{
				for (int j = 0; j < WIDTH; j++)
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
