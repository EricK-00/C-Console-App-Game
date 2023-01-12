using System;
using System.Diagnostics.CodeAnalysis;

namespace CSharpConsoleAppGame
{
    enum OwnerType
    {
        MINE = 0,
        FOE
    }

    public struct RankSet
    {
        public RankSet()
        {
            Attack = 0;
            Defense = 0;
            SpAttack = 0;
            SpDefense = 0;
            Speed = 0;
        }

        public RankSet(int attack, int defense, int spAttack, int spDefense, int speed)
        {
            Attack = attack;
            Defense = defense;
			SpAttack = spAttack;
			SpDefense = spDefense;
			Speed = speed;
        }

        public int Attack { get; }
        public int Defense { get; }
		public int SpAttack { get; }
		public int SpDefense { get; }
		public int Speed { get; }

        //int HitRateRank;
        //int CriticalRateRank;

        public float GetRankValue(int rank)
        {
            if (rank >= 0)
            {
                return ((float)rank + 2) / 2;
            }
            else
            {
                return 2 / ((float)Math.Abs(rank) + 2);
            }
        }

        public string GetRankString(int rank)
        {
            if (rank == 0)
            {
                return "";
            }
            else if (rank > 0)
            {
                return $"[+{rank}]";
            }
            else
            {
                return $"[{rank}]";
            }
        }
    }


    internal class InBattleCharacter : Character
    {
        public CharacterStats BattleStats { get; private set; }
        public RankSet Rank { get; private set; }
        public StatusCondition Status { get; private set; } = StatusCondition.없음;
        public string BattleName { get; }
        public int TurnCount { get; private set; } = 0;

        public bool IsFlinch { get; private set; }
        public int DamageDealt { get; private set; }

        public Window viewWindow { get; }
        public OwnerType Owner { get; }

        private int attackReduced = 1;
        private int speedReduced = 1;

        //string heldItem;
        //string feature;

        public InBattleCharacter(int id, OwnerType owner, Window view)
        {
            Id = id;
            Owner = owner;
            viewWindow = view;

            DefaultStats = CharacterData.GetCharacter(Id).DefaultStats;
            DefaultName = CharacterData.GetCharacter(Id).DefaultName;
            FirstType = CharacterData.GetCharacter(Id).FirstType;
            SecondType = CharacterData.GetCharacter(Id).SecondType;
            Skills = CharacterData.GetCharacter(Id).Skills;

            BattleName = (Owner == OwnerType.FOE) ? $"상대 {DefaultName}" : DefaultName;
            BattleStats = new CharacterStats(DefaultStats.Hp, DefaultStats.Attack, DefaultStats.Defense,
                DefaultStats.SpAttack, DefaultStats.SpDefense, DefaultStats.Speed);
        }

        public InBattleCharacter(Character baseCharacter, OwnerType owner, Window view)
        {
            Id = baseCharacter.Id;
            Owner = owner;
            viewWindow = view;

            DefaultStats = CharacterData.GetCharacter(Id).DefaultStats;
            DefaultName = CharacterData.GetCharacter(Id).DefaultName;
            FirstType = CharacterData.GetCharacter(Id).FirstType;
            SecondType = CharacterData.GetCharacter(Id).SecondType;
            Skills = CharacterData.GetCharacter(Id).Skills;

            BattleName = (Owner == OwnerType.FOE) ? $"상대 {DefaultName}" : DefaultName;
            BattleStats = new CharacterStats(DefaultStats.Hp, DefaultStats.Attack, DefaultStats.Defense,
                DefaultStats.SpAttack, DefaultStats.SpDefense, DefaultStats.Speed);
        }

        public string GetSkillMessage(int skillIndex)
        {
            string message = string.Empty;
            if (skillIndex >= 0 && skillIndex < 4)
                message = $"{BattleName}의 {Skills[skillIndex].Name}!";

            return message;
        }

        private string GetStatusConditionString()
        {
			string message = string.Empty;
			if (Status != StatusCondition.없음)
				message = Status.ToString();

			return message;
		}

        public string GetStatusString()
        {
            string message = string.Empty;
            if (Status != StatusCondition.없음)
                message = Status.ToString();

            if (BattleStats.Hp <= 0)
                message = "기절";

            return message;
        }

        public string GetHpString()
        {
            return $"HP:{BattleStats.Hp}/{DefaultStats.Hp}";

        }

        public void PrintStatusAction()
        {
            string message = string.Empty;

            switch (Status)
            {
                case StatusCondition.마비:
                    message = $"{BattleName}은(는) 몸이 저려 움직이지 못했다.";
                    break;
                case StatusCondition.얼음:
                    message = $"{BattleName}은(는) 얼어붙어 있다.";
                    break;
                case StatusCondition.화상:
                    message = $"{BattleName}은(는) 화상에 의해 피해를 입었다.";
                    break;
                case StatusCondition.독:
                    message = $"{BattleName}은(는) 독에 의해 피해를 입었다.";
                    break;
                default:
                    return;
            }

            UIPreset.CreateScriptTextArea(message, 1, true);
            Console.ReadKey(true);
            UIPreset.ClearScript(1);
        }

        public void SetStatusCondition(StatusCondition newStatus)
        {
            string message = string.Empty;
            Status = newStatus;

            switch (newStatus)
            {
                case StatusCondition.화상:
                    message = $"{BattleName}은(는) 화상을 입었다.";
                    break;
                case StatusCondition.독:
                    message = $"{BattleName}은(는) 독에 걸렸다.";
                    break;
                case StatusCondition.얼음:
                    message = $"{BattleName}은(는) 얼어붙었다.";
                    break;
                case StatusCondition.마비:
                    message = $"{BattleName}은(는) 마비에 걸렸다.";
                    break;
                default:
                    return;
            }

            UIPreset.CreateScriptTextArea(message, 1, true);
            Console.ReadKey(true);
            UIPreset.ClearScript(1);
            RerenderWindow();
        }

        public void Damaged(int damage, int typeEffectiveness, bool isCritical)
        {
            if (typeEffectiveness != (int)TypeEffectiveness.없음)
            {
                if (damage < 1)
                    damage = 1;
                damage = (damage > BattleStats.Hp) ? BattleStats.Hp : damage;
                DamageDealt = damage;

                Animation.Blink(viewWindow, 5, 'x', 30);
                BattleStats = new CharacterStats(BattleStats.Hp - damage, BattleStats.Attack, BattleStats.Defense,
                    BattleStats.SpAttack, BattleStats.SpDefense, BattleStats.Speed);
                RerenderWindow();
            }

			switch (typeEffectiveness)
			{
				case (int)TypeEffectiveness.없음:
					UIPreset.CreateScriptTextArea("효과가 없는 것 같다...", 2, true);
					Console.ReadKey(true);
					UIPreset.ClearScript(1);
					UIPreset.ClearScript(2);
					break;
				case (int)TypeEffectiveness.별로 * (int)TypeEffectiveness.별로:
				case (int)TypeEffectiveness.별로 * (int)TypeEffectiveness.보통:
					UIPreset.CreateScriptTextArea("효과가 별로인 것 같다...", 2, true);
					Console.ReadKey(true);
					UIPreset.ClearScript(1);
					UIPreset.ClearScript(2);
					break;
				case (int)TypeEffectiveness.보통 * (int)TypeEffectiveness.보통:
                    UIPreset.ClearScript(1);
					break;
				default:
					UIPreset.CreateScriptTextArea("효과가 굉장했다!", 2, true);
					Console.ReadKey(true);
					UIPreset.ClearScript(1);
					UIPreset.ClearScript(2);
					break;
			}

			if (isCritical)
			{
				UIPreset.CreateScriptTextArea("급소에 맞았다!", 1, true);
				Console.ReadKey(true);
			}

			CheckStuned();
        }

        public void DamagedFromStatusCondition()
        {
			if (Status == StatusCondition.독 || Status == StatusCondition.화상)
			{
				int damage = DefaultStats.Hp / 8;
				ConsoleColor blinkColor = Status == StatusCondition.독 ? ConsoleColor.Magenta : ConsoleColor.Red;


				Animation.BlinkWithColor(viewWindow, 3, blinkColor, 50);
				BattleStats = new CharacterStats(BattleStats.Hp - damage, BattleStats.Attack, BattleStats.Defense,
	                BattleStats.SpAttack, BattleStats.SpDefense, BattleStats.Speed);
				RerenderWindow();

				PrintStatusAction();

				CheckStuned();
			}
		}

        public bool CheckSkipAction(int defenderHp, int skillIndex)
        {
			if (Status == StatusCondition.마비 && new Random().Next(0, 4) < 1)
			{
					Animation.BlinkWithColor(viewWindow, 1, ConsoleColor.DarkYellow, 150);
					PrintStatusAction();
                    return true;
			}
			else if (Status == StatusCondition.얼음)
			{
				if (new Random().Next(0, 5) < 1)
				{
					UIPreset.CreateScriptTextArea("얼음이 풀렸다!", 1, true);
					SetStatusCondition(StatusCondition.없음);
					Console.ReadKey(true);
					UIPreset.ClearScript(1);
                    RerenderWindow();
				}
				else
				{
					Animation.BlinkWithColor(viewWindow, 1, ConsoleColor.Cyan, 150);
					PrintStatusAction();
					return true;
				}
			}

			if (IsFlinch)
			{
				UIPreset.CreateScriptTextArea($"{BattleName}은(는) 풀이 죽어 움직일 수 없었다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
                return true;
			}

            if (Skills[skillIndex].HitRate == 102)
            {
                return false;
            }

			if (defenderHp <= 0 || new Random().Next(0, 100) >= Skills[skillIndex].HitRate)
			{
				UIPreset.CreateScriptTextArea($"{GetSkillMessage(skillIndex)}", 1, true);
				Console.ReadKey(true);
				UIPreset.CreateScriptTextArea("그러나 빗나갔다.", 1, true);
				Console.ReadKey(true);

                return true;
			}

			return false;

		}

        private void CheckStuned()
        {
            if (BattleStats.Hp <= 0)
            {
                UIPreset.CreateScriptTextArea($"{BattleName}은(는) 쓰러졌다.", 1, true);
                Console.ReadKey(true);
                viewWindow.ClearContents();
                UIPreset.ClearScript(1);
            }
        }

        public void AddTurnCount()
        {
            if (BattleStats.Hp > 0)
                ++TurnCount;
            IsFlinch = false;
        }

        public void ReturnToBall()
        {
            TurnCount = 1;
            Rank = new RankSet();
        }

        public void AddAttackRank()
        {
            if (Rank.Attack >= 6)
            {
                UIPreset.CreateScriptTextArea($"{BattleName}의 공격은(는) 더 올라가지 않는다!", 1, true);
                Console.ReadKey(true);
                UIPreset.ClearScript(1);
            }
            else
            {
                Rank = new RankSet(Rank.Attack + 1, Rank.Defense, Rank.SpAttack, Rank.SpDefense, Rank.Speed);
				UIPreset.CreateScriptTextArea($"{BattleName}의 공격이(가) 올라갔다.", 1, true);
                Console.ReadKey(true);
                UIPreset.ClearScript(1);
                RerenderWindow();
            }
        }

		public void AddSpAttackRank()
		{
			if (Rank.SpAttack >= 6)
			{
				UIPreset.CreateScriptTextArea($"{BattleName}의 특수공격은(는) 더 올라가지 않는다!", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
			}
			else
			{
				Rank = new RankSet(Rank.Attack, Rank.Defense, Rank.SpAttack + 1, Rank.SpDefense, Rank.Speed);
				UIPreset.CreateScriptTextArea($"{BattleName}의 특수공격(이)가 올라갔다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
				RerenderWindow();
			}
		}

		public void AddSpDefenseRank()
		{
			if (Rank.SpDefense >= 6)
			{
				UIPreset.CreateScriptTextArea($"{BattleName}의 특수방어은(는) 더 올라가지 않는다!", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
			}
			else
			{
				Rank = new RankSet(Rank.Attack, Rank.Defense, Rank.SpAttack, Rank.SpDefense + 1, Rank.Speed);
				UIPreset.CreateScriptTextArea($"{BattleName}의 특수방어(이)가 올라갔다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
				RerenderWindow();
			}
		}

		public void AddSpeedRank()
        {
            if (Rank.Speed >= 6)
            {
                UIPreset.CreateScriptTextArea($"{BattleName}의 스피드은(는) 더 올라가지 않는다!", 1, true);
                Console.ReadKey(true);
                UIPreset.ClearScript(1);
            }
            else
            {
                Rank = new RankSet(Rank.Attack, Rank.Defense, Rank.SpAttack, Rank.SpDefense, Rank.Speed + 1);
				UIPreset.CreateScriptTextArea($"{BattleName}의 스피드(이)가 올라갔다.", 1, true);
				Console.ReadKey(true);
                UIPreset.ClearScript(1);
				RerenderWindow();
			}
        }

		public void ReduceSpDefenseRank()
		{
			if (Rank.SpDefense <= -6)
			{
				UIPreset.CreateScriptTextArea($"{BattleName}의 특수방어은(는) 더 내려가지 않는다!", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
			}
			else
			{
				Rank = new RankSet(Rank.Attack, Rank.Defense, Rank.SpAttack, Rank.SpDefense - 1, Rank.Speed);
				UIPreset.CreateScriptTextArea($"{BattleName}의 특수방어(이)가 내려갔다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
				RerenderWindow();
			}
		}

		public void Flinch()
        {
            IsFlinch = true;
        }

        public void DrainHp(string foeName, int givenDamage)
        {
            int gainHp = givenDamage / 2;
            if (gainHp <= 0)
                gainHp = 1;

            gainHp = BattleStats.Hp + gainHp > DefaultStats.Hp ? DefaultStats.Hp - BattleStats.Hp : gainHp;

            BattleStats = new CharacterStats(BattleStats.Hp + gainHp, BattleStats.Attack, BattleStats.Defense,
                BattleStats.SpAttack, BattleStats.SpDefense, BattleStats.Speed);

            Animation.BlinkWithColor(viewWindow, 3, ConsoleColor.Green, 100);
            RerenderWindow();
            UIPreset.CreateScriptTextArea($"{foeName}(으)로부터 체력을 흡수했다!", 1, true);
            Console.ReadKey(true);
            UIPreset.ClearScript(1);
        }

        public void RerenderWindow()
        {
            attackReduced = Status == StatusCondition.화상 ? 2 : 1;
            speedReduced = Status == StatusCondition.마비 ? 2 : 1;

            BattleStats = new CharacterStats(BattleStats.Hp, (int)(DefaultStats.Attack * (float)Rank.GetRankValue(Rank.Attack) / attackReduced),
                (int)(DefaultStats.Defense * (float)Rank.GetRankValue(Rank.Defense)), (int)(DefaultStats.SpAttack * (float)Rank.GetRankValue(Rank.SpAttack)),
                (int)(DefaultStats.SpDefense * (float)Rank.GetRankValue(Rank.SpDefense)), (int)(DefaultStats.Speed * (float)Rank.GetRankValue(Rank.Speed) / speedReduced));


			if (Owner == OwnerType.MINE)
            {
                viewWindow.RewriteWindowContents(
                new string[]
                {
                    $"",
                    $"{BattleName}",
                    $"{GetTypeString()}",
                    $"{GetHpString()}",
                    $"",
                    $"물리공격:{BattleStats.Attack} {Rank.GetRankString(Rank.Attack)}",
                    $"물리방어:{BattleStats.Defense} {Rank.GetRankString(Rank.Defense)}",
                    $"특수공격:{BattleStats.SpAttack} {Rank.GetRankString(Rank.SpAttack)}",
                    $"특수방어:{BattleStats.SpDefense} {Rank.GetRankString(Rank.SpDefense)}",
                    $"스피드:{BattleStats.Speed} {Rank.GetRankString(Rank.Speed)}",
                    $"",
                    $"",
                    $"{GetStatusConditionString()}"
                }, true);
            }
            else
            {
                viewWindow.RewriteWindowContents(
                new string[]
                {
                    $"",
                    $"{BattleName}",
                    $"{GetTypeString()}",
                    $"{GetHpString()}",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"{GetStatusConditionString()}"
                }, true);
            }
        }
    }
}