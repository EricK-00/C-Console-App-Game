using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class Skill
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Power { get; set; }
		public int HitRate { get; set; }

		public Type SkillType { get; set; }
		public delegate void SkillEffect (InBattleCharacter attacker, InBattleCharacter defenser, int rate);

		public Skill()
		{
            //SkillEffect effect = RankUp();
		}

		public void RankUp(InBattleCharacter attacker, InBattleCharacter defenser)
		{
			attacker.Poisoned();
		}
	}
}
