using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    enum SkillCategory
    {
        물리 = 0,
        특수,
        변화
    }

    class SkillData
	{
		public const int MAX_SKILL_NAME_LENGTH = 6;
		private static List<Skill> skillList;
		public static void Initialize(List<Skill> skills)
		{
			if (skillList == null)
				skillList = skills;
		}

		public static Skill GetSkill(int id)
		{
			if (skillList.Count <= id || id <= 0)
				return skillList[0];

			return skillList[id];
		}

		public static int GetCount()
		{
			//default 제외
			return skillList.Count - 1;
		}

		public static void GetSkillEffect(int id)
		{
			switch (id)
			{
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
			}
		}
	}
}
