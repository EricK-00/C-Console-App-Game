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

		//public string Description { get; }

		public Skill(int id_)
		{
			Id = id_;
			Name = SkillData.GetSkill(id_).Name;
			Power = SkillData.GetSkill(id_).Power;
			HitRate = SkillData.GetSkill(id_).HitRate;
			SkillType = SkillData.GetSkill(id_).SkillType;
		}

		public Skill(int id_, string name_, int power_, int hitRate_, Type skillType_, SkillCategory category_)// , string description)
		{
			Id = id_;
			Name = name_;
			Power = power_;
			HitRate = hitRate_;
			SkillType = skillType_;
			Category = category_;
			//Description = description;
		}
	}
}
