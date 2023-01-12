using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class SkillEffectData
	{
		private static Dictionary<string, Action<InBattleCharacter, InBattleCharacter>> EffectDictionary = new Dictionary<string, Action<InBattleCharacter, InBattleCharacter>>();


		static SkillEffectData()
		{
			EffectDictionary.Add("드래곤다이브", new Action<InBattleCharacter, InBattleCharacter>(MakeFlinched));
			EffectDictionary.Add("불대문자", new Action<InBattleCharacter, InBattleCharacter>(MakeBurned));
			EffectDictionary.Add("에어슬래시", new Action<InBattleCharacter, InBattleCharacter>(MakeFlinched));
			EffectDictionary.Add("몸통박치기", new Action<InBattleCharacter, InBattleCharacter>(DoNothing));
			EffectDictionary.Add("기가드레인", new Action<InBattleCharacter, InBattleCharacter>(DrainHp));
			EffectDictionary.Add("파도타기", new Action<InBattleCharacter, InBattleCharacter>(DoNothing));
			EffectDictionary.Add("오물폭탄", new Action<InBattleCharacter, InBattleCharacter>(MakePoisoned));
			EffectDictionary.Add("냉동빔", new Action<InBattleCharacter, InBattleCharacter>(MakeFrozen));
			EffectDictionary.Add("메가드레인", new Action<InBattleCharacter, InBattleCharacter>(DrainHp));
			EffectDictionary.Add("폭포오르기", new Action<InBattleCharacter, InBattleCharacter>(MakeFlinched));
			EffectDictionary.Add("10만볼트", new Action<InBattleCharacter, InBattleCharacter>(MakeParalysed));
			EffectDictionary.Add("번개", new Action<InBattleCharacter, InBattleCharacter>(MakeParalysed));
			EffectDictionary.Add("전광석화", new Action<InBattleCharacter, InBattleCharacter>(DoNothing));
			EffectDictionary.Add("전기자석파", new Action<InBattleCharacter, InBattleCharacter>(TryParalysed));
			EffectDictionary.Add("용의춤", new Action<InBattleCharacter, InBattleCharacter>(ShowRankUpAnimation) + AddAttack + AddSpeed);
			EffectDictionary.Add("신속", new Action<InBattleCharacter, InBattleCharacter>(DoNothing));
			EffectDictionary.Add("화염방사", new Action<InBattleCharacter, InBattleCharacter>(MakeBurned));
			EffectDictionary.Add("불꽃엄니", new Action<InBattleCharacter, InBattleCharacter>(MakeBurnAndFlinched));
			EffectDictionary.Add("드래곤크루", new Action<InBattleCharacter, InBattleCharacter>(DoNothing));
			EffectDictionary.Add("번개엄니", new Action<InBattleCharacter, InBattleCharacter>(MakeParalysedAndFlinched));
			EffectDictionary.Add("독찌르기", new Action<InBattleCharacter, InBattleCharacter>(MakePoisoned));
			EffectDictionary.Add("눈보라", new Action<InBattleCharacter, InBattleCharacter>(MakeFrozen));
			EffectDictionary.Add("사이코키네시스", new Action<InBattleCharacter, InBattleCharacter>(ShowFoeRankDownAnimation) + ReduceFoeSpDefense);
			EffectDictionary.Add("명상", new Action<InBattleCharacter, InBattleCharacter>(ShowRankUpAnimation) + AddSpAttack + AddSpDefense);
		}

		private static void DoNothing(InBattleCharacter caster, InBattleCharacter foe)
		{
			//do nothing
		}

		private static void MakePoisoned(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.Status != StatusCondition.없음 || foe.BattleStats.Hp <= 0)
				return;

			if (foe.FirstType != Type.독 && foe.SecondType != Type.독)
			{
				foe.SetStatusCondition(StatusCondition.독);
			}
		}

		private static void MakeBurned(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.Status != StatusCondition.없음 || foe.BattleStats.Hp <= 0)
				return;

			if (foe.FirstType != Type.불꽃 && foe.SecondType != Type.불꽃)
			{
				foe.SetStatusCondition(StatusCondition.화상);
			}
		}

		private static void TryParalysed(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.Status != StatusCondition.없음 || foe.BattleStats.Hp <= 0 || foe.FirstType == Type.전기 || foe.SecondType == Type.전기)
			{
				UIPreset.CreateScriptTextArea("그러나 실패했다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
				return;
			}

			MakeParalysed(caster, foe);
		}

		private static void MakeParalysed(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.Status != StatusCondition.없음 || foe.BattleStats.Hp <= 0)
				return;

			if (foe.FirstType != Type.전기 && foe.SecondType != Type.전기)
			{
				foe.SetStatusCondition(StatusCondition.마비);
			}
		}

		private static void MakeFrozen(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.Status != StatusCondition.없음 || foe.BattleStats.Hp <= 0)
				return;

			if (foe.FirstType != Type.얼음 && foe.SecondType != Type.얼음)
			{
				foe.SetStatusCondition(StatusCondition.얼음);
			}
		}

		private static void MakeFlinched(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.BattleStats.Hp <= 0)
				return;

			foe.Flinch();
		}

		private static void MakeBurnAndFlinched(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.BattleStats.Hp <= 0)
				return;

			if (foe.FirstType != Type.불꽃 && foe.SecondType != Type.불꽃 && foe.Status == StatusCondition.없음)
			{
				foe.SetStatusCondition(StatusCondition.화상);
			}

			int rate = BattleStage.BATTLE_DEBUG_MODE ? 100 : 10;
			if (new Random().Next(0, 100) < rate)
				foe.Flinch();
		}
		private static void MakeParalysedAndFlinched(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.BattleStats.Hp <= 0)
				return;

			if (foe.FirstType != Type.전기 && foe.SecondType != Type.전기 && foe.Status == StatusCondition.없음)
			{
				foe.SetStatusCondition(StatusCondition.마비);
			}

			int rate = BattleStage.BATTLE_DEBUG_MODE ? 100 : 10;
			if (new Random().Next(0, 100) < rate)
				foe.Flinch();
		}

		private static void DrainHp(InBattleCharacter caster, InBattleCharacter foe)
		{
			caster.DrainHp(foe.BattleName, foe.DamageDealt);
		}

		private static void ShowRankUpAnimation(InBattleCharacter caster, InBattleCharacter foe)
		{
			Animation.BlinkWithColor(caster.viewWindow, 1, ConsoleColor.Red, 300);
		}

		private static void ShowFoeRankDownAnimation(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.BattleStats.Hp <= 0)
				return;

			Animation.BlinkWithColor(foe.viewWindow, 1, ConsoleColor.Blue, 300);
		}

		private static void AddAttack(InBattleCharacter caster, InBattleCharacter foe)
		{
			caster.AddAttackRank();
		}

		private static void AddSpeed(InBattleCharacter caster, InBattleCharacter foe)
		{
			caster.AddSpeedRank();
		}

		private static void AddSpAttack(InBattleCharacter caster, InBattleCharacter foe)
		{
			caster.AddSpAttackRank();
		}

		private static void AddSpDefense(InBattleCharacter caster, InBattleCharacter foe)
		{
			caster.AddSpDefenseRank();
		}

		private static void ReduceFoeSpDefense(InBattleCharacter caster, InBattleCharacter foe)
		{
			if (foe.BattleStats.Hp <= 0)
				return;

			foe.ReduceSpDefenseRank();
		}

		public static Action<InBattleCharacter, InBattleCharacter> GetEffects(string skillName)
		{
			if (EffectDictionary.TryGetValue(skillName, out var effects))
				return effects;
			else
				return new Action<InBattleCharacter, InBattleCharacter>(DoNothing);
		}
	}
}
