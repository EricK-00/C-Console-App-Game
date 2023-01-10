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
			return skillList.Count;
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
