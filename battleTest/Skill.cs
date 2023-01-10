using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace battleTest
{
	internal class Skill
	{

		public int Id { get; }
		public string Name { get; set; }
		public int Power { get; set; }
		public int HitRate { get; set; }

		public Type SkillType { get; set; }

		//public string Description { get; set; }

		public Skill(int id_)
		{
			Id = id_;
			Name = SkillData.GetSkill(id_).Name;
			Power = SkillData.GetSkill(id_).Power;
			HitRate = SkillData.GetSkill(id_).HitRate;
			SkillType = SkillData.GetSkill(id_).SkillType;
		}

		public Skill(int id_, string name, int power, int hitRate, Type skillType)// , string description)
		{
			Id = id_;
			Name = name;
			Power = power;
			HitRate = hitRate;
			SkillType = skillType;
			//Description = description;
		}
	}
}
