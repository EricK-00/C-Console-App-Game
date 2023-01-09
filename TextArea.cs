using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace CSharpConsoleAppGame
{
    internal class TextArea : UI
    {
        public string TextString { get; private set; }

        public TextArea(int x_, int y_, int width_, int height_, string text_) : base(x_, y_, width_, height_)
        {
            TextString = text_;
            SetContents();

            Screen.Render(this);
        }

		public TextArea(int x_, int y_, int width_, int height_, string text_, int delayTerm) : base(x_, y_, width_, height_)
		{
			TextString = text_;
			SetContents();

            Screen.RenderTextWithDelay(this, delayTerm);
		}

		private void SetContents()
        {
            int strIndex = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (strIndex >= TextString.Length)
                        goto END;

                    Contents[i, j] = TextString[strIndex].ToString();
                    ++strIndex;
                }
            }
END: { ContentsSize = strIndex; }
        }

        public void Rewrite(string newText)
        {
            Screen.Clear(this);
            TextString = newText;
            SetContents();
            Screen.Render(this);
        }
    }
}