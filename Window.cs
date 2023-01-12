using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class Window : ImageArea
    {
        //const char DEFAULT_OUT_LINE_CHAR = '=';
        const char DEFAULT_OUT_LINE_CHAR = '…';
        public int BlankAreaXMin { get; }
        public int BlankAreaXMax { get; }
        public int BlankAreaYMin { get; }
        public int BlankAreaYMax { get; }
        public int BlankAreaWidth { get; }
        public int BlankAreaHeight { get; }
        public int BlankAreaSize { get; }

        private char paddingChar;
        private char outlineChar = DEFAULT_OUT_LINE_CHAR;

		public Window(int x_, int y_, int width_, int height_, char paddingChar_, bool isRendered) : base(x_, y_, width_, height_)
		{
			paddingChar = paddingChar_;
			BlankAreaXMin = X + 1;
			BlankAreaXMax = X + Width - 1;
			BlankAreaYMin = Y + 1;
			BlankAreaYMax = Y + Height - 1;
			BlankAreaWidth = Width - 2;
			BlankAreaHeight = Height - 2;
			BlankAreaSize = BlankAreaWidth * BlankAreaHeight;

			Contents = SetDefaultWindow(Width, Height, paddingChar);

            if (isRendered)
			    Screen.Render(this);
		}

		public Window(int x_, int y_, int width_, int height_, char paddingChar_, string[] windowContents, bool align, bool isRendered)
			: base(x_, y_, width_, height_)
		{
			paddingChar = paddingChar_;
			BlankAreaXMin = X + 1;
			BlankAreaXMax = X + Width - 1;
			BlankAreaYMin = Y + 1;
			BlankAreaYMax = Y + Height - 1;
			BlankAreaWidth = Width - 2;
			BlankAreaHeight = Height - 2;
			BlankAreaSize = BlankAreaWidth * BlankAreaHeight;

			SetDefaultWindow(Width, Height, paddingChar);
			Contents = SetWindowContents(windowContents, align);

			if (isRendered)
				Screen.Render(this);
		}

		public Window(int x_, int y_, int width_, int height_, char paddingChar_, char outlineChar_, bool isRendered) : base(x_, y_, width_, height_)
        {
            paddingChar = paddingChar_;
            outlineChar = outlineChar_;
            BlankAreaXMin = X + 1;
            BlankAreaXMax = X + Width - 1;
            BlankAreaYMin = Y + 1;
            BlankAreaYMax = Y + Height - 1;
            BlankAreaWidth = Width - 2;
            BlankAreaHeight = Height - 2;
            BlankAreaSize = BlankAreaWidth * BlankAreaHeight;

            Contents = SetDefaultWindow(Width, Height, paddingChar);

			if (isRendered)
				Screen.Render(this);
        }

        public Window(int x_, int y_, int width_, int height_, char paddingChar_, char outlineChar_, string[] windowContents, bool align, bool isRendered) 
            : base(x_, y_, width_, height_)
        {
            paddingChar = paddingChar_;
			outlineChar = outlineChar_;
			BlankAreaXMin = X + 1;
            BlankAreaXMax = X + Width - 1;
            BlankAreaYMin = Y + 1;
            BlankAreaYMax = Y + Height - 1;
            BlankAreaWidth = Width - 2;
            BlankAreaHeight = Height - 2;
            BlankAreaSize = BlankAreaWidth * BlankAreaHeight;

            SetDefaultWindow(Width, Height, paddingChar);
            Contents = SetWindowContents(windowContents, align);

			if (isRendered)
				Screen.Render(this);
        }

        private string[,] SetDefaultWindow(int width, int height, char paddingChar)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                        Image[i, j] = outlineChar.ToString();
                    else
                        Image[i, j] = paddingChar.ToString();
                }
            }

            return Image;
        }

        private string[,] SetWindowContents(string[] windowContents, bool align)
        {
            int maxHeight = Math.Min(windowContents.Length, BlankAreaHeight);
            int maxWidth;
            int startX;

            for (int i = 0; i < maxHeight; i++)
            {
                maxWidth = Math.Min(windowContents[i].Length, BlankAreaWidth);

                if (align)
                {
                    startX = Width / 2 - windowContents[i].Length / 2;
                    if (startX < 1)
                        startX = 1;
                }
                else
                {
                    startX = 1;
                }

                for (int j = 0; j < maxWidth; j++)
                {
                    Image[1 + i, startX + j] = windowContents[i][j].ToString();
                }
            }

            return Image;
        }

        private string[,] SetWindowContents(string lineContents, int lineIndex, bool align)
        {

            for (int i = 0; i < BlankAreaYMax; i++)
                Image[lineIndex, 1 + i] = " ";

            int maxWidth = Math.Min(lineContents.Length, BlankAreaWidth);
            int startX;

            if (align)
            {
                startX = Width / 2 - lineContents.Length / 2;
                if (startX < 1)
                    startX = 1;
            }
            else
            {
                startX = 1;
            }

            for (int i = 0; i < maxWidth; i++)
                Image[lineIndex, startX + i] = lineContents[i].ToString();

            return Image;
        }

        public void RewriteWindowContents(string[] newContents, bool align)
        {
            SetDefaultWindow(Width, Height, paddingChar);
            Contents = SetWindowContents(newContents, align);
            Screen.Render(this);
        }

        public void RewriteWindowContents(string newContents, int lineIndex, bool align)
        {
            Contents = SetWindowContents(newContents, lineIndex, align);
            Screen.Render(this);
        }

        public void ClearContents()
        {
            SetDefaultWindow(Width, Height, paddingChar);
            Screen.Render(this);
        }
    }
}