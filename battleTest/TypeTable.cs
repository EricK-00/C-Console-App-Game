using System;
using System.Collections.Generic;
using System.Data;
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
		독,
		노말,
		없음
	}

	class TypeTable
	{
		public enum TypeEffectiveness
		{
			없음 = 0,
			별로 = 1,
			보통 = 2,
			굉장 = 4
		}

		//[attacker, defender]
		public static readonly int[,] table = new int[(int)Type.없음 + 1, (int)Type.없음 + 1];

		static TypeTable()
		{
			table[(int)Type.불꽃, (int)Type.불꽃] = (int)TypeEffectiveness.별로;
			table[(int)Type.불꽃, (int)Type.물] = (int)TypeEffectiveness.별로;
			table[(int)Type.불꽃, (int)Type.풀] = (int)TypeEffectiveness.굉장;
			table[(int)Type.불꽃, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.없음] = (int)TypeEffectiveness.보통;

			table[(int)Type.물, (int)Type.불꽃] = (int)TypeEffectiveness.별로;
			table[(int)Type.물, (int)Type.물] = (int)TypeEffectiveness.별로;
			table[(int)Type.물, (int)Type.풀] = (int)TypeEffectiveness.별로;
			table[(int)Type.물, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.없음] = (int)TypeEffectiveness.보통;

			table[(int)Type.풀, (int)Type.불꽃] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.물] = (int)TypeEffectiveness.굉장;
			table[(int)Type.풀, (int)Type.풀] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.비행] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.독] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.풀, (int)Type.없음] = (int)TypeEffectiveness.보통;

			table[(int)Type.비행, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.풀] = (int)TypeEffectiveness.굉장;
			table[(int)Type.비행, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.없음] = (int)TypeEffectiveness.보통;

			table[(int)Type.독, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.풀] = (int)TypeEffectiveness.굉장;
			table[(int)Type.독, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.노말] = (int)TypeEffectiveness.별로;
			table[(int)Type.독, (int)Type.없음] = (int)TypeEffectiveness.보통;

			table[(int)Type.노말, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.풀] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.없음] = (int)TypeEffectiveness.보통;
		}
	}
}