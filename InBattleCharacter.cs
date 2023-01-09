using System;

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
        private CharacterStats DefaultStats;
        private CharacterStats BattleStats;
        public string StatusConditon = string.Empty;

        //string heldItem;
        //string feature;
        Skill[] skills = new Skill[4];
        Type characterType;

        public int X { get; protected set; }
        public int Y { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public string[,] Image { get; set; }

        public InBattleCharacter(string name, CharacterStats stats) : base (name, stats)
        {
            DefaultStats = stats;
            BattleStats = DefaultStats;
        }

        public void Poisoned()
        {
            StatusConditon = "독";
        }

        public void Damaged(int damage)
        {
            BattleStats.Hp -= damage;
            if (BattleStats.Hp <= 0)
            {
                BattleStats.Hp = 0;
                Stuned();
            }
        }

        private void Stuned()
        {

        }
    }
}
