using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class Window : ImageArea
    {
        const char outlineChar = '▣';
        public Window(int x_, int y_, int width_, int height_, char paddingChar) : base(x_, y_, width_, height_)
        {
            SetWindowImage(Width, Height, paddingChar);
            Contents = Image;

            Screen.Render(this);
        }

        private void SetWindowImage(int width, int height, char paddingChar)
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
        }
    }
}