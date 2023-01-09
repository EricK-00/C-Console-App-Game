using System;

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


    internal class InBattleCharacter
    {
        private Character baseCharacter;
        public CharacterStats DefaultStats { get; }
        public CharacterStats BattleStats { get; }
        public string StatusConditon = string.Empty;

        //string heldItem;
        //string feature;
        Skill[] skills = new Skill[4];
        Type characterType;

        public InBattleCharacter(Character baseCharacter_)
        {
            baseCharacter = baseCharacter_;
            DefaultStats = baseCharacter_.Stats;
            BattleStats = DefaultStats;
        }

        public void Poisoned()
        {
            StatusConditon = "독";
        }

        public void Damaged(int damage)
        {

        }

        private void Stuned()
        {

        }
    }
}
