using System;

namespace battleTest
{
	internal class Player
	{
		int winCount;
		public const int MAX_CHARACTER_COUNT = 3;
		public Character[] Characters { get; } = new Character[MAX_CHARACTER_COUNT];

		public Player()
		{
			Characters = new Character[MAX_CHARACTER_COUNT];
			for (int i = 0; i < Characters.Length; i++)
				Characters[i] = new Character(0);
		}

		public void SwapCharacter(int index1, int index2)
		{
			Character temp = Characters[index1];
			Characters[index1] = Characters[index2];
			Characters[index2] = temp;
		}

		public int RandomCharacterGenerater()
		{
			Random random = new Random();
			return random.Next(0, 1);
		}
	}
}