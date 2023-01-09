using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleTest
{
    public enum Type
    {
        불꽃 = 0,
        물,
        풀,
        비행,
        없음
    }

    internal class Character
    {
        int id;

        public string Name { get; }
        public CharacterStats Stats { get; }

        Skill[] skills;
        public Type FirstType;
        public Type SecondType;

        public Character(string name, CharacterStats stats)
        {
            Name = name;
            Stats = stats;
        }

        public Character(string name, Type first, Type second, CharacterStats stats, Skill[] skillArray)
        {
            Name = name;
            Stats = stats;
            FirstType = first;
            SecondType = second;

            skills = new Skill[4];
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i] = skillArray[i];
            }
        }

        public string GetTypeString()
        {
            if (SecondType == Type.없음)
                return $"{FirstType}";
            else
                return $"{FirstType} {SecondType}";
        }
    }
}