using System;

namespace CSharpConsoleAppGame
{
	internal class Player
	{
		public int WinCount { get; private set; }
		public Character[] Characters { get; } = new Character[BattleStage.MAX_BATTLE_CHARACTER];

		public Player()
		{
			Characters = new Character[BattleStage.MAX_BATTLE_CHARACTER];
			for (int i = 0; i < Characters.Length; i++)
				Characters[i] = CharacterData.GetRandomCharacter();
		}

		public void WinTheBattle()
		{
			++WinCount;
		}
	}
}