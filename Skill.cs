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
	}
}
