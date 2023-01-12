using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class SkillEffectData
	{
		private static Dictionary<string, Action<InBattleCharacter, InBattleCharacter, int>> EffectDictionary = new Dictionary<string, Action<InBattleCharacter, InBattleCharacter, int>>();


		static SkillEffectData()
		{
			EffectDictionary.Add("화염방사", new Action<InBattleCharacter, InBattleCharacter, int>(MakeBurned));
			EffectDictionary.Add("불대문자", new Action<InBattleCharacter, InBattleCharacter, int>(MakeBurned));
			EffectDictionary.Add("에어슬래시", new Action<InBattleCharacter, InBattleCharacter, int>(MakeFlinched));
			EffectDictionary.Add("몸통박치기", new Action<InBattleCharacter, InBattleCharacter, int>(DoNothing));
			EffectDictionary.Add("기가드레인", new Action<InBattleCharacter, InBattleCharacter, int>(DrainHp));
			EffectDictionary.Add("파도타기", new Action<InBattleCharacter, InBattleCharacter, int>(DoNothing));
			EffectDictionary.Add("오물폭탄", new Action<InBattleCharacter, InBattleCharacter, int>(MakePoisoned));
			EffectDictionary.Add("냉동빔", new Action<InBattleCharacter, InBattleCharacter, int>(MakeFrozen));
			EffectDictionary.Add("메가드레인", new Action<InBattleCharacter, InBattleCharacter, int>(DrainHp));
			EffectDictionary.Add("폭포오르기", new Action<InBattleCharacter, InBattleCharacter, int>(MakeFlinched));
			EffectDictionary.Add("10만볼트", new Action<InBattleCharacter, InBattleCharacter, int>(MakeParalysed));
			EffectDictionary.Add("번개", new Action<InBattleCharacter, InBattleCharacter, int>(MakeParalysed));
			EffectDictionary.Add("전광석화", new Action<InBattleCharacter, InBattleCharacter, int>(DoNothing));
			EffectDictionary.Add("전기자석파", new Action<InBattleCharacter, InBattleCharacter, int>(TryParalysed));
			EffectDictionary.Add("용의춤", new Action<InBattleCharacter, InBattleCharacter, int>(ShowRankUpAnimation) + AddAttack + AddSpeed);
		}

		private static void DoNothing(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			//do nothing
		}

		private static void MakePoisoned(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			if (foe.Status != StatusCondition.없음)
				return;

			if (foe.BattleStats.Hp <= 0)
				return;

			if ((foe.FirstType != Type.독 && foe.SecondType != Type.독) && new Random().Next(0, 100) < rate)
			{
				foe.SetStatusCondition(StatusCondition.독);
			}
		}

		private static void MakeBurned(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			if (foe.Status != StatusCondition.없음)
				return;

			if (foe.BattleStats.Hp <= 0)
				return;

			if ((foe.FirstType != Type.불 && foe.SecondType != Type.불) && new Random().Next(0, 100) < rate)
			{
				foe.SetStatusCondition(StatusCondition.화상);
			}
		}

		private static void TryParalysed(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			if (foe.Status != StatusCondition.없음 || foe.BattleStats.Hp <= 0 ||
				foe.FirstType == Type.전기 || foe.SecondType == Type.전기)
			{
				UIPreset.CreateScriptTextArea("그러나 실패했다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
				return;
			}

			MakeParalysed(caster, foe, 100);
		}

		private static void MakeParalysed(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			if (foe.Status != StatusCondition.없음 || foe.BattleStats.Hp <= 0)
				return;

			if ((foe.FirstType != Type.전기 && foe.SecondType != Type.전기) && new Random().Next(0, 100) < rate)
			{
				foe.SetStatusCondition(StatusCondition.마비);
			}
		}

		private static void MakeFrozen(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			if (foe.Status != StatusCondition.없음)
				return;

			if (foe.BattleStats.Hp <= 0)
				return;

			if ((foe.FirstType != Type.얼음 && foe.SecondType != Type.얼음) && new Random().Next(0, 100) < rate)
			{
				foe.SetStatusCondition(StatusCondition.얼음);
			}
		}

		private static void MakeFlinched(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			if (foe.BattleStats.Hp <= 0)
				return;

			if (new Random().Next(0, 100) < rate)
			{
				foe.Flinch();
			}
		}

		private static void DrainHp(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			caster.DrainHp(foe.DamageDealt);
		}

		private static void ShowRankUpAnimation(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			Animation.BlinkWithColor(caster.viewWindow, 1, ConsoleColor.Red, 300);
		}

		private static void AddAttack(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			caster.AddAttackRank();
		}

		private static void AddSpeed(InBattleCharacter caster, InBattleCharacter foe, int rate)
		{
			caster.AddSpeedRank();
		}

		public static Action<InBattleCharacter, InBattleCharacter, int> GetEffects(string skillName)
		{
			if (EffectDictionary.TryGetValue(skillName, out var effects))
				return effects;
			else
				return new Action<InBattleCharacter, InBattleCharacter, int>(DoNothing);
		}
	}
}
