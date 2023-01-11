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

        public TextArea(int x_, int y_, string text_) : base(x_, y_, text_)
        {
            TextString = text_;
            SetContents();

            Screen.Render(this);
        }

		public TextArea(int x_, int y_, string text_, int delayTerm) : base(x_, y_, text_)
		{
			TextString = text_;
			SetContents();

            Screen.RenderTextWithDelay(this, delayTerm);
		}

		private void SetContents()
        {
            ContentsSize = Width = TextString.Length;
            for (int i = 0; i < Width; i++)
            {
                Contents[0, i] = TextString[i].ToString();
            }
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