using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

		public Skill(int id_)
		{
			Id = id_;
			Name = SkillData.GetSkill(id_).Name;
			Power = SkillData.GetSkill(id_).Power;
			HitRate = SkillData.GetSkill(id_).HitRate;
			SkillType = SkillData.GetSkill(id_).SkillType;
            Category = SkillData.GetSkill(id_).Category;
        }

		public Skill(int id_, string name_, int power_, int hitRate_, Type skillType_, SkillCategory category_)
		{
			Id = id_;
			Name = name_;
			Power = power_;
			HitRate = hitRate_;
			SkillType = skillType_;
			Category = category_;
		}
	}
}
