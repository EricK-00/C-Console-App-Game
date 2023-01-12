using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
				//102 = self target skill
				new Skill(0, "-----", 0, 0, Type.없음, SkillCategory.변화, 0, 0),
				new Skill(1, "드래곤다이브", 100, 75, Type.드래곤, SkillCategory.물리, 20, 0),
				new Skill(2, "불대문자", 120, 85, Type.불꽃, SkillCategory.특수, 30, 0),
				new Skill(3, "에어슬래시", 75, 95, Type.비행, SkillCategory.특수, 30, 0),
				new Skill(4, "몸통박치기", 50, 100, Type.노말, SkillCategory.물리, 0, 0),
				new Skill(5, "기가드레인", 75, 100, Type.풀, SkillCategory.특수, 100, 0),
				new Skill(6, "파도타기", 95, 100, Type.물, SkillCategory.특수, 0, 0),
				new Skill(7, "오물폭탄", 90, 100, Type.독, SkillCategory.특수, 30, 0),
				new Skill(8, "냉동빔", 95, 100, Type.얼음, SkillCategory.특수, 10, 0),
				new Skill(9, "메가드레인", 40, 100, Type.풀, SkillCategory.특수, 100, 0),
				new Skill(10, "폭포오르기", 80, 100, Type.물, SkillCategory.물리, 20, 0),
				new Skill(11, "10만볼트", 95, 100, Type.전기, SkillCategory.특수, 10, 0),
				new Skill(12, "번개", 120, 70, Type.전기, SkillCategory.특수, 30, 0),
				new Skill(13, "전광석화", 40, 100, Type.노말, SkillCategory.물리, 0, 1),
				new Skill(14, "전기자석파", -1, 90, Type.전기, SkillCategory.변화, 100, 0),
				new Skill(15, "용의춤", -1, 102, Type.드래곤, SkillCategory.변화, 100, 0),
				new Skill(16, "신속", 80, 100, Type.노말, SkillCategory.물리, 0, 2),
				new Skill(17, "화염방사", 95, 100, Type.불꽃, SkillCategory.특수, 10, 0),
				new Skill(18, "불꽃엄니", 65, 95, Type.불꽃, SkillCategory.물리, 10, 0),
				new Skill(19, "드래곤크루", 85, 100, Type.드래곤, SkillCategory.물리, 0, 0),
				new Skill(20, "번개엄니", 65, 95, Type.전기, SkillCategory.물리, 10, 0),
				new Skill(21, "독찌르기", 80, 100, Type.독, SkillCategory.물리, 30, 0),
				new Skill(22, "눈보라", 120, 70, Type.얼음, SkillCategory.특수, 10, 0),
				new Skill(23, "사이코키네시스", 90, 100, Type.에스퍼, SkillCategory.특수, 10, 0),
				new Skill(24, "명상", -1, 102, Type.에스퍼, SkillCategory.변화, 100, 0),
			});

			CharacterData.Initialize(new List<Character>
			{
				new Character(0, "default", Type.없음, Type.없음, new CharacterStats(1,1,1,1,1,1), new Skill[1]{ new Skill(0) }),
				new Character(1, "리자몽", Type.불꽃, Type.비행, new CharacterStats(78 + 75, 84 + 20, 78 + 20, 109 + 20, 85 + 20, 100 + 20), new Skill[4] { new Skill(1), new Skill(2), new Skill(3), new Skill(15) }),
				new Character(2, "이상해꽃", Type.풀, Type.독, new CharacterStats(80 + 75, 82 + 20, 83 + 20, 100 + 20, 100 + 20 ,80 + 20), new Skill[4] { new Skill(4), new Skill(5), new Skill(7), new Skill(9) }),
				new Character(3, "거북왕", Type.물, Type.없음, new CharacterStats(79 + 75, 83 + 20, 100 + 20, 85 + 20, 105 + 20, 78 + 20), new Skill[4] { new Skill(8), new Skill(4), new Skill(6), new Skill(10) }),
				new Character(4, "피카츄", Type.전기, Type.없음, new CharacterStats(35 + 75, 55 + 20, 40 + 20, 50 + 20, 50 + 20, 90 + 20), new Skill[4] { new Skill(11), new Skill(12), new Skill(13), new Skill(14) }),
				new Character(5, "망나뇽", Type.드래곤, Type.비행, new CharacterStats(91 + 75, 134 + 20, 95 + 20, 100 + 20, 100 + 20, 80 + 20), new Skill[4] { new Skill(1), new Skill(19), new Skill(15), new Skill(16) }),
				new Character(6, "윈디", Type.불꽃, Type.없음, new CharacterStats(90 + 75, 110 + 20, 80 + 20, 100 + 20, 80 + 20, 95 + 20), new Skill[4] { new Skill(18), new Skill(20), new Skill(16), new Skill(13) }),
				new Character(7, "질뻐기", Type.독, Type.없음, new CharacterStats(105 + 75, 105 + 20, 75 + 20, 65 + 20, 100 + 20, 50 + 20), new Skill[4] { new Skill(11), new Skill(18), new Skill(21), new Skill(7) }),
				new Character(8, "라이츄", Type.전기, Type.없음, new CharacterStats(60 + 75, 90 + 20, 55 + 20, 90 + 20, 80 + 20, 110 + 20), new Skill[4] { new Skill(11), new Skill(12), new Skill(13), new Skill(14) }),
				new Character(9, "썬더", Type.전기, Type.비행, new CharacterStats(90 + 75, 90 + 20, 85 + 20, 125 + 20, 90 + 20, 100 + 20), new Skill[4] { new Skill(11), new Skill(12), new Skill(14), new Skill(3) }),
				new Character(10, "파이어", Type.불꽃, Type.비행, new CharacterStats(90 + 75, 100 + 20, 90 + 20, 125 + 20, 85 + 20, 90 + 20), new Skill[4] { new Skill(17), new Skill(2), new Skill(13), new Skill(3) }),
				new Character(11, "프리져", Type.얼음, Type.비행, new CharacterStats(90 + 75, 85 + 20, 100 + 20, 95 + 20, 125 + 20, 85 + 20), new Skill[4] { new Skill(8), new Skill(22), new Skill(13), new Skill(3) }),
				new Character(12, "뮤츠", Type.에스퍼, Type.없음, new CharacterStats(106 + 75, 110 + 20, 90 + 20, 154 + 20, 90 + 20, 130 + 20), new Skill[4] { new Skill(23), new Skill(24), new Skill(8), new Skill(11) }),
			});
		}
	}
}