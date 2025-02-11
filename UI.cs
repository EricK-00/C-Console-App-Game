﻿using System;

namespace CSharpConsoleAppGame
{
	interface IAnimatable
	{
		public int X { get; }
		public int Y { get; }
		public int Width { get; }
		public int Height { get; }
		public string[,] Contents { get; }

		public int ContentsSize { get; }
	}

	internal abstract class UI : IAnimatable
	{
		public int X { get; protected set; }
		public int Y { get; protected set; }

		public int Width { get; protected set; }
		public int Height { get; protected set; }

        public string[,] Contents { get; protected set; }

		public int ContentsSize { get; protected set; }

        public UI(int x_, int y_, string text)
        {
            X = x_;
            Y = y_;
            Width = text.Length;
            Height = 1;
            Contents = new string[1, Width];
            ContentsSize = text.Length;
        }

        public UI(int x_, int y_, string[,] image_)
		{
			X = x_;
			Y = y_;
			Width = image_.GetLength(1);
			Height = image_.GetLength(0);
			Contents = new string[Height, Width];
			ContentsSize = Width * Height;
		}

		public UI(int x_, int y_, int width_, int height_)
		{
			X = x_;
			Y = y_;
			Width = width_;
			Height = height_;
            Contents = new string[Height, Width];
			ContentsSize = Width * Height;
		}
	}
}