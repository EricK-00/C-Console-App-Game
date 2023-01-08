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
        public string Text { get; private set; }

        public TextArea(int x_, int y_, int width_, int height_, string text_) : base(x_, y_, width_, height_)
        {
            Text = text_;
            SetContents();
        }

        private void SetContents()
        {
            int strIndex = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (strIndex >= Text.Length)
                        goto END;

                    Contents[i, j] = Text[strIndex].ToString();
                    ++strIndex;
                }
            }
END: { }
        }

        public void Rewrite(string newText)
        {
            Screen.ClearText(this);
            Text = newText;
            SetContents();
            Screen.RenderText(this);
        }
    }
}