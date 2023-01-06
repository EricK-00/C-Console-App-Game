using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class InputManager
	{
		Animation animation = new Animation();

		public InputManager()
		{
			//animation.Animate += new EventHandler(BlockUserInput);
		}

		public static void BlockUserInput(string message)
		{
			if (Console.KeyAvailable)
			{
				Console.ReadKey(true);
			}
		}
	}
}
