using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class ImageArea : UI
    {
        public string[,] Image { get; }

		public ImageArea(int x_, int y_, string[,] image_, bool isRendered) : base(x_, y_, image_)
        {
            Image = new string[Height, Width];
            for (int i = 0; i < Image.GetLength(0); i++)
            {
                for (int j = 0; j < Image.GetLength(1); j++)
                {
                    Image[i, j] = image_[i, j];
                }
            }
            Contents = Image;

            if (isRendered)
                Screen.Render(this);
        }

		public ImageArea(int x_, int y_, int width_, int height_, char paddingChar, bool isRendered) : base(x_, y_, width_, height_)
        {
            Image = new string[Height, Width];
            for (int i = 0; i < height_; i++)
            {
                for (int j = 0; j < width_; j++)
                {
                    Image[i, j] = paddingChar.ToString();
                }
            }
            Contents = Image;

            if (isRendered)
                Screen.Render(this);
        }

		protected ImageArea(int x_, int y_, int width_, int height_) : base(x_, y_, width_, height_)
		{
			Image = new string[Height, Width];
		}
	}
}
