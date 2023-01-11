using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	class CharacterData
	{
        private static List<Character> characterList;

		public static void Initialize(List<Character> characters)
		{
			if (characterList == null)
				characterList = characters;
		}

		public static Character GetCharacter(int id)
		{
			if (characterList.Count <= id || id <= 0)
				return characterList[0];

			return characterList[id];
		}

		public static Character GetRandomCharacter()
		{
			return characterList[new Random().Next(1, characterList.Count)];
		}

		public static int GetCount()
		{
			return characterList.Count;
		}
	}
}
