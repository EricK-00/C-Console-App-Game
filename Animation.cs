using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{

	internal class Animation
	{
        public static void Blink(IAnimatable animatableObject, int blinkCount)
		{
			Task task = Task.Run(() => { BlinkByChar(animatableObject, blinkCount); });
			InputManager.BlockInput(task);
        }

        public static void BlinkWithColor(IAnimatable animatableObject, int blinkCount, ConsoleColor color)
        {
            Task task = Task.Run(() => { BlinkByColor(animatableObject, blinkCount, color); });
            InputManager.BlockInput(task);
        }

        public static void OnCursorOver(IAnimatable animatableObject)
        {
            Task task = Task.Run(() => { BlinkByColor(animatableObject, 2, ConsoleColor.Black); });
            InputManager.BlockInput(task);
        }

		private static void BlinkArea()
		{

		}

        private static void BlinkByChar(IAnimatable animatableObject, int blinkCount)
		{
			string[,] blinkImage = new string[animatableObject.Contents.GetLength(0), animatableObject.Contents.GetLength(1)];
			for (int i = 0; i < blinkImage.GetLength(0); i++)
			{
				for (int j = 0; j < blinkImage.GetLength(1); j++)
				{
					blinkImage[i, j] = "xx";
				}
			}

			for (int i = 0; i < blinkCount; i++)
			{
				Screen.Render(animatableObject, blinkImage);
				Task.Delay(50).Wait();
				Screen.Render(animatableObject);
				Task.Delay(50).Wait();
			}
		}

		private static void BlinkByColor(IAnimatable animatableObject, int blinkCount, ConsoleColor color)
		{
            for (int i = 0; i < blinkCount; i++)
            {
                Screen.RenderWithColor(animatableObject, animatableObject.Contents, color);
                Task.Delay(50).Wait();
                Screen.Render(animatableObject);
                Task.Delay(50).Wait();
            }
        }
	}
}