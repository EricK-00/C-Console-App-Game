using System;
using System.Diagnostics.CodeAnalysis;

namespace CSharpConsoleAppGame
{
    public struct Rank
    {
        public Rank()
        {
            AttackRank = 0;
            DefenseRank = 0;
            SpeedRank = 0;
        }

        int AttackRank { get; set; }
        int DefenseRank { get; set; }
        int SpeedRank { get; set; }
        //int HitRateRank;
        //int CriticalRateRank;
    }


    internal class InBattleCharacter : Character
    {
        public CharacterStats BattleStats { get; }
        public Rank CurrentRank { get; private set; }
        public StatusCondition Conditon { get; private set; } = StatusCondition.없음;
        public string BattleName { get; }
        public int TurnCount { get; private set; } = 0;
		//string heldItem;
		//string feature;

        public InBattleCharacter(int id, string name)
        {
            Id = id;
			DefaultStats = CharacterData.GetCharacter(Id).DefaultStats;
			DefaultName = CharacterData.GetCharacter(Id).DefaultName;
			FirstType = CharacterData.GetCharacter(Id).FirstType;
			SecondType = CharacterData.GetCharacter(Id).SecondType;
			Skills = CharacterData.GetCharacter(Id).Skills;

			BattleName = name;
            BattleStats = new CharacterStats(DefaultStats.Hp, DefaultStats.Attack, DefaultStats.Defense,
                DefaultStats.SpAttack, DefaultStats.SpDefense, DefaultStats.Speed);
        }

		public InBattleCharacter(Character baseCharacter, string name)
        {
            Id = baseCharacter.Id;
            DefaultStats = CharacterData.GetCharacter(Id).DefaultStats;
            DefaultName = CharacterData.GetCharacter(Id).DefaultName;
            FirstType = CharacterData.GetCharacter(Id).FirstType;
            SecondType = CharacterData.GetCharacter(Id).SecondType;
            Skills = CharacterData.GetCharacter(Id).Skills;

            BattleName = name;
			BattleStats = new CharacterStats(DefaultStats.Hp, DefaultStats.Attack, DefaultStats.Defense,
                DefaultStats.SpAttack, DefaultStats.SpDefense, DefaultStats.Speed);
        }

        public void Poisoned()
        {
            Conditon = StatusCondition.독;
        }

        public string GetSkillMessage(int skillIndex)
        {
			string message = string.Empty;
            if (skillIndex >= 0 && skillIndex < 4)
					message = $"{BattleName}의 {Skills[skillIndex].Name}!";

			return message;
		}

		public string GetConditionString()
		{
            string message = string.Empty;
            if (Conditon != StatusCondition.없음)
                message = Conditon.ToString();

            if (BattleStats.Hp <= 0)
                message = "기절";

            return message;
		}

		public string GetConditionActionMessage()
        {
            string message = string.Empty;

            switch (Conditon)
            {
                case StatusCondition.마비:
                    //
                    message = $"{BattleName}은(는) 몸이 저려 움직이지 못했다.";
                    break;
                case StatusCondition.얼음:
                    //
                    message = $"{BattleName}은(는) 얼어붙어 있다.";
                    break;
                case StatusCondition.화상:
                    if (FirstType == Type.불꽃 || SecondType == Type.불꽃)
                        message = $"{BattleName}은(는) 화상에 의해 피해를 입었다.";
                    break;
                case StatusCondition.독:
                    if (FirstType == Type.독 || SecondType == Type.독)
                        message = $"{BattleName}은(는) 독에 의해 피해를 입었다.";
                    break;
                default:
                    break;
            }

            return message;
        }

        public void SetStatusCondition(StatusCondition condition)
        {
            switch (condition)
            {
                case StatusCondition.화상:
                    Console.WriteLine($"{BattleName}은(는) 화상을 입었다.");
                    break;
				case StatusCondition.독:
					Console.WriteLine($"{BattleName}은(는) 독에 걸렸다.");
					break;
				case StatusCondition.얼음:
					Console.WriteLine($"{BattleName}은(는) 얼어붙었다.");
					break;
				case StatusCondition.마비:
					Console.WriteLine($"{BattleName}은(는) 마비에 걸렸다.");
					break;
			}
        }

        public void Damaged(int damage, Window window, int hpTextLine)
        {
            BattleStats.Damaged(damage);
            Animation.Blink(window, 3, 'x', 30);
            window.RewriteWindowContents($"HP:{BattleStats.Hp}/{DefaultStats.Hp}", hpTextLine, true);
        }

        public void CheckStuned(Window window)
        {
            if (BattleStats.Hp <= 0)
            {
                UIPreset.CreateScriptTextArea($"{BattleName}은(는) 쓰러졌다.", 1, true);
                Console.ReadKey();
                window.ClearContents();
            }
        }

        public void AddTurnCount()
        {
            if (BattleStats.Hp > 0)
                ++TurnCount;
        }

        public void ReturnToBall()
        {
            TurnCount = 1;
            CurrentRank = new Rank();
        }
    }
}
