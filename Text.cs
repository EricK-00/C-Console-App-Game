using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class Text
    {
        public string TextString { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        private int xMiddle;

        public Text(int relativeX, int relativeY, string text_)
        {
            TextString = text_;
            xMiddle = relativeX;
            X = xMiddle - TextString.Length / 2;
            Y = relativeY;
        }

        public void Rewrite(string text_)
        {
            TextString = text_;
            X = xMiddle - TextString.Length / 2;
        }
    }
}