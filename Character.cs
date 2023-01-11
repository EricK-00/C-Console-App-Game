using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpConsoleAppGame
{
    internal class Character
    {
		public int Id { get; protected set; }

        public string DefaultName { get; protected set; }
        public CharacterStats DefaultStats { get; protected set; }

        public Skill[] Skills { get; protected set; }
        public Type FirstType { get; protected set; }
        public Type SecondType { get; protected set; }

        public Character()
        {

        }

        public Character(int id)
        {
            Id = id;
            DefaultName = CharacterData.GetCharacter(id).DefaultName;
			DefaultStats = CharacterData.GetCharacter(id).DefaultStats;
			FirstType = CharacterData.GetCharacter(id).FirstType;
			SecondType = CharacterData.GetCharacter(id).SecondType;
            Skills = CharacterData.GetCharacter(id).Skills;
		}

        public Character(int id_, string name_, Type first, Type second, CharacterStats stats, Skill[] skill_)
        {
            Id = id_;
            DefaultName = name_;
            DefaultStats = stats;
            FirstType = first;
            SecondType = second;

            SetUpSkills();
            for (int i = 0; i < skill_.Length; i++)
                Skills[i] = new Skill(skill_[i].Id);
        }

        private void SetUpSkills()
        {
            Skills = new Skill[4];
            for (int i = 0; i < Skills.Length; i++)
                Skills[i] = new Skill(0);
        }

        public string GetTypeString()
        {
            if (SecondType == Type.없음)
                return $"{FirstType}";
            else
                return $"{FirstType},{SecondType}";
        }
    }
}