using System;

namespace CSharpConsoleAppGame
{
	internal class Player
	{
		int winCount;
		const int MAX_CHARACTER_COUNT = 3;
		Character[] myCharacter = new Character[MAX_CHARACTER_COUNT];

		Player()
		{
			for (int i = 0; i < MAX_CHARACTER_COUNT; i++)
			{
				//myCharacter[i] = new Character();
			}
		}

		public void SwapCharacter(int index1, int index2)
		{
			Character temp = myCharacter[index1];
			myCharacter[index1] = myCharacter[index2];
			myCharacter[index2] = temp;
		}

		public int RandomCharacterGenerater()
		{
			Random random = new Random();
			return random.Next(0, 1);
		}
	}
}