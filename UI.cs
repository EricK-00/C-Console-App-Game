using System;

namespace CSharpConsoleAppGame
{
	interface IAnimatable
	{
		public int X { get; }
		public int Y { get; }
		public int Width { get; }
		public int Height { get; }
		public string[,] Image { get; set; }
	}

	internal class UI : IAnimatable
	{
		public int X { get; protected set; }
		public int Y { get; protected set; }

		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public string[,] Image { get; set; }

		public UI(int x_, int y_, int width_, int height_)
		{
			X = x_;
			Y = y_;
			Width = width_;
			Height = height_;
			Image = new string[Height, Width];
			SetDefaultImage();
		}

		private void SetDefaultImage()
		{
			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (i == 0 || i == Height - 1 || j == 0 || j == Width - 1)
					{
						Image[i, j] = "▣";
					}
					else
						Image[i, j] = " ".PadLeft(2);
				}
			}
		}
	}
}