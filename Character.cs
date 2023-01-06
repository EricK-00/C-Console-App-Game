using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	public enum Type
	{

	}

	internal class Character : IAnimatable
	{
		int maxHp;
		int attack;
		int defense;
		int specialAttack;
		int specialDefense;
		int speed;

		int currentHp;

		//string bringObject;
		//string feature;
		Skill[] skills = new Skill[4];
		Type characterType;

		public int X { get; protected set; }
		public int Y { get; protected set; }

		public int Width { get; protected set; }
		public int Height { get; protected set; }
		public string[,] Image { get; set; }
	}
}