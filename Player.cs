using System;

namespace CSharpConsoleAppGame
{
	internal class Player
	{
		int winCount;
		public Character[] Characters { get; } = new Character[CharacterData.BATTLE_CHARACTER_COUNT];

		public Player()
		{
			Characters = new Character[CharacterData.BATTLE_CHARACTER_COUNT];
			for (int i = 0; i < Characters.Length; i++)
				Characters[i] = new Character(0);
		}

		//public void SwapCharacter(int index1, int index2)
		//{
		//	Character temp = Characters[index1];
		//	Characters[index1] = Characters[index2];
		//	Characters[index2] = temp;
		//}
	}
}