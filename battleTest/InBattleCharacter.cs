using System;
using System.Diagnostics.CodeAnalysis;

namespace battleTest
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
        public int TurnCount { get; private set; }
		//string heldItem;
		//string feature;

        public InBattleCharacter(int id, string name)
        {
            Id = id;
			DefaultStats = CharacterData.GetCharacter(id).DefaultStats;
			DefaultName = CharacterData.GetCharacter(id).DefaultName;
			FirstType = CharacterData.GetCharacter(id).FirstType;
			SecondType = CharacterData.GetCharacter(id).SecondType;
			Skills = CharacterData.GetCharacter(id).Skills;

			BattleName = name;
            BattleStats = DefaultStats;
        }

		public InBattleCharacter(Character baseCharacter, string name)
        {
            Id = baseCharacter.Id;
			DefaultStats = baseCharacter.DefaultStats;
            DefaultName = baseCharacter.DefaultName;
            FirstType = baseCharacter.FirstType;
            SecondType = baseCharacter.SecondType;
            Skills = baseCharacter.Skills;

			BattleName = name;
			BattleStats = DefaultStats;
        }

        public void Poisoned()
        {
            Conditon = StatusCondition.독;
        }

        public string GetSkillMessage(int skillIndex)
        {
			string message = string.Empty;
            if (skillIndex > 0 && skillIndex < 5)
					message = $"{BattleName}의 {Skills[skillIndex].Name}!";

			return message;
		}

        public string GetStatusConditionMessage()
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

        public void Damaged(int damage)
        {
            BattleStats.Damaged(damage);
            if (BattleStats.Hp <= 0)
                Stuned();
        }

        private void Stuned()
        {
            Console.WriteLine($"{BattleName}은(는) 쓰러졌다.");
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
