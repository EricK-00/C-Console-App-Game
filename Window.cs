using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class Window : UI
	{
		public Window(int x_, int y_, int width_, int height_) : base(x_, y_, width_, height_)
		{
			X = x_;
			Y = y_;
			Width = width_;
			Height = height_;
		}
	}
}