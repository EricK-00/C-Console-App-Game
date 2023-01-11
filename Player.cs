using System;

namespace CSharpConsoleAppGame
{
	internal class Player
	{
		public int WinCount { get; private set; }
		public Character[] Characters { get; } = new Character[CharacterData.BATTLE_CHARACTER_COUNT];

		public Player()
		{
			Characters = new Character[CharacterData.BATTLE_CHARACTER_COUNT];
			for (int i = 0; i < Characters.Length; i++)
				Characters[i] = CharacterData.GetRandomCharacter();
		}

		public void WinTheBattle()
		{
			++WinCount;
		}

		//public void SwapCharacter(int index1, int index2)
		//{
		//	Character temp = Characters[index1];
		//	Characters[index1] = Characters[index2];
		//	Characters[index2] = temp;
		//}
	}
}