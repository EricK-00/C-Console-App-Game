using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class Skill
	{

		public int Id { get; }
		public string Name { get; }
		public int Power { get; }
		public int HitRate { get; }

		public Type SkillType { get; }
		public SkillCategory Category { get; }
		public Action<InBattleCharacter, InBattleCharacter> Effect { get; }
		public int EffectRate { get; }
		public int Priority { get; }

		public Skill(int id_)
		{
			Id = id_;
			Name = SkillData.GetSkill(id_).Name;
			Power = SkillData.GetSkill(id_).Power;
			HitRate = SkillData.GetSkill(id_).HitRate;
			SkillType = SkillData.GetSkill(id_).SkillType;
            Category = SkillData.GetSkill(id_).Category;
			Effect = SkillEffectData.GetEffects(Name);
			EffectRate = SkillData.GetSkill(id_).EffectRate;
			Priority = SkillData.GetSkill(id_).Priority;
        }

		public Skill(int id_, string name_, int power_, int hitRate_, Type skillType_, SkillCategory category_, int effectRate_, int priority_)
		{
			Id = id_;
			Name = name_;
			Power = power_;
			HitRate = hitRate_;
			SkillType = skillType_;
			Category = category_;
			Effect = SkillEffectData.GetEffects(Name);
			EffectRate = effectRate_;
			Priority = priority_;
		}

		public string PowerString()
		{
			if (Power <= 0)
				return "--";
			return Power.ToString();
		}

		public string HitRateString()
		{
			if (HitRate <= 0 || HitRate > 100)
				return "--";
			return HitRate.ToString();
		}
	}
}
