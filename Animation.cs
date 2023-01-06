using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{

	internal class Animation
	{
		public event EventHandler Animate;

		public void BlinkImage(IAnimatable animatableObject, int blinkCount)
		{
			Task task = Task.Run(() => { Blink(animatableObject, blinkCount); });
			task.Wait();
			if (Animate != null)
			{
				Animate(task, EventArgs.Empty);
			}
			//while (!task.IsCompleted)
			{
				//BlockUserInput();
			}
		}

		private Task<bool> Blink(IAnimatable animatableObject, int blinkCount)
		{
			string[,] blinkImage = new string[animatableObject.Image.GetLength(0), animatableObject.Image.GetLength(1)];
			for (int i = 0; i < blinkImage.GetLength(0); i++)
			{
				for (int j = 0; j < blinkImage.GetLength(1); j++)
				{
					blinkImage[i, j] = "00";
				}
			}

			for (int i = 0; i < blinkCount; i++)
			{
				Screen.PlaceOnScreen(animatableObject, blinkImage);
				Task.Delay(50).Wait();
				Screen.PlaceOnScreen(animatableObject);
				Task.Delay(50).Wait();
			}

			return Task.FromResult(true);
		}
	}
}