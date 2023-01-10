using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class CharacterStats
    {
        public CharacterStats(int hp_, int attack_, int defense_, int specialAttack_, int specialDefense_, int speed_)
        {
            Hp = hp_;
            Attack = attack_;
            Defense = defense_;
            SpAttack = specialAttack_;
            SpDefense= specialDefense_;
            Speed = speed_;
        }

        public int Hp { get; private set; }
        public int Attack { get; }
        public int Defense { get; }

        public int SpAttack { get; }
        public int SpDefense { get; }
        public int Speed { get; }

        public void Damaged(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
                Hp = 0;
        }
    }
}
