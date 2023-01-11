using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class DataLoader
	{
		public static void LoadData()
		{
			SkillData.Initialize(new List<Skill>
			{
				new Skill(0, "-----", 0, 0, Type.없음, SkillCategory.변화),
				new Skill(1, "화염방사", 95, 100, Type.불꽃, SkillCategory.특수),
				new Skill(2, "불대문자", 120, 85, Type.불꽃, SkillCategory.특수),
				new Skill(3, "에어슬래시", 80, 100, Type.비행, SkillCategory.특수),
				new Skill(4, "몸통박치기", 40, 100, Type.노말, SkillCategory.물리),
				new Skill(5, "기가드레인", 40, 100, Type.풀, SkillCategory.특수),
				new Skill(6, "파도타기", 95, 100, Type.물, SkillCategory.특수),
				new Skill(7, "오물폭탄", 80, 100, Type.독, SkillCategory.특수)
			});

			CharacterData.Initialize(new List<Character>
			{
				//+75, +20
				new Character(0, "default", Type.없음, Type.없음, new CharacterStats(1,1,1,1,1,1), new Skill[1]{new Skill(0)}),
				new Character(1, "리자몽", Type.불꽃, Type.비행, new CharacterStats(100,100,100,100,100,100), new Skill[4]{new Skill(1), new Skill(2), new Skill(3), new Skill(4)}),
				new Character(2, "이상해꽃", Type.풀, Type.독, new CharacterStats(75,75,125,75,100,125), new Skill[4]{new Skill(4), new Skill(5), new Skill(7), new Skill(6)}),
				new Character(3, "거북왕", Type.물, Type.없음, new CharacterStats(111,99,88,77,66,55), new Skill[4]{new Skill(1), new Skill(4), new Skill(6), new Skill(7)}),
			});
		}
	}
}