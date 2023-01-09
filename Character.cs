﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	public enum Type
	{
		불꽃 = 0,
		물,
		풀,
		비행,
		없음
	}

	public struct CharacterStats
	{
		public CharacterStats(int hp_, int attack_, int defense_, int speed_)
		{
			Hp = hp_;
			Attack = attack_;
			Defense = defense_;
			Speed = speed_;
		}

		public int Hp { get; set; }
		public int Attack { get; }
		public int Defense { get; }
		//public int SpecialAttack { get; }
		//public int SpecialDefense { get; }
		public int Speed { get; }
	}

	internal class Character
	{
		int id;

		public string Name { get; }
		public CharacterStats Stats { get; }

		//string heldItem;
		//string feature;
		Skill[] skills = new Skill[4];
		public Type FirstType { get; }
		public Type SecondType { get; }

		public Character(string name, CharacterStats stats)
		{
			Name = name;
			Stats = stats;
		}

        public Character(string name, CharacterStats stats, Type first, Type second)
        {
            Name = name;
            Stats = stats;
			FirstType = first;
			SecondType = second;
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