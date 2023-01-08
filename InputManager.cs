using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class InputManager
	{
		public InputManager()
		{
			Animation.AnimatePlaying += BlockUserInput;
		}

		public static void BlockUserInput(object sender, Task task)
		{
			while (!task.IsCompleted)
			{
				if (Console.KeyAvailable)
				{
					Console.ReadKey(true);
				}
			}
		}
	}
}