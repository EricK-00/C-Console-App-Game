using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	public enum Type
	{
		없음 = 0,
		불꽃,
		물,
		풀,
		비행,
		독,
		노말,
		얼음,
		전기,
		드래곤,
		에스퍼
	}

	public enum TypeEffectiveness
	{
		없음 = 0,
		별로 = 1,
		보통 = 2,
		굉장 = 4
	}

	class TypeTable
	{
		readonly int MAX_TYPE_COUNT = Enum.GetValues(typeof(Type)).Length;

		public static readonly int[,] table = new int[Enum.GetValues(typeof(Type)).Length, Enum.GetValues(typeof(Type)).Length];

		static TypeTable()
		{
			table[(int)Type.불꽃, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.불꽃] = (int)TypeEffectiveness.별로;
			table[(int)Type.불꽃, (int)Type.물] = (int)TypeEffectiveness.별로;
			table[(int)Type.불꽃, (int)Type.풀] = (int)TypeEffectiveness.굉장;
			table[(int)Type.불꽃, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.얼음] = (int)TypeEffectiveness.굉장;
			table[(int)Type.불꽃, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.불꽃, (int)Type.드래곤] = (int)TypeEffectiveness.별로;
			table[(int)Type.불꽃, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.물, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.불꽃] = (int)TypeEffectiveness.굉장;
			table[(int)Type.물, (int)Type.물] = (int)TypeEffectiveness.별로;
			table[(int)Type.물, (int)Type.풀] = (int)TypeEffectiveness.별로;
			table[(int)Type.물, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.물, (int)Type.드래곤] = (int)TypeEffectiveness.별로;
			table[(int)Type.물, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.풀, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.풀, (int)Type.불꽃] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.물] = (int)TypeEffectiveness.굉장;
			table[(int)Type.풀, (int)Type.풀] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.비행] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.독] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.풀, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.풀, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.풀, (int)Type.드래곤] = (int)TypeEffectiveness.별로;
			table[(int)Type.풀, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.비행, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.풀] = (int)TypeEffectiveness.굉장;
			table[(int)Type.비행, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.전기] = (int)TypeEffectiveness.별로;
			table[(int)Type.비행, (int)Type.드래곤] = (int)TypeEffectiveness.보통;
			table[(int)Type.비행, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.독, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.풀] = (int)TypeEffectiveness.굉장;
			table[(int)Type.독, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.독] = (int)TypeEffectiveness.별로;
			table[(int)Type.독, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.드래곤] = (int)TypeEffectiveness.보통;
			table[(int)Type.독, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.노말, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.풀] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.드래곤] = (int)TypeEffectiveness.보통;
			table[(int)Type.노말, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.얼음, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.얼음, (int)Type.불꽃] = (int)TypeEffectiveness.별로;
			table[(int)Type.얼음, (int)Type.물] = (int)TypeEffectiveness.별로;
			table[(int)Type.얼음, (int)Type.풀] = (int)TypeEffectiveness.굉장;
			table[(int)Type.얼음, (int)Type.비행] = (int)TypeEffectiveness.굉장;
			table[(int)Type.얼음, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.얼음, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.얼음, (int)Type.얼음] = (int)TypeEffectiveness.별로;
			table[(int)Type.얼음, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.얼음, (int)Type.드래곤] = (int)TypeEffectiveness.굉장;
			table[(int)Type.얼음, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.전기, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.전기, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.전기, (int)Type.물] = (int)TypeEffectiveness.굉장;
			table[(int)Type.전기, (int)Type.풀] = (int)TypeEffectiveness.별로;
			table[(int)Type.전기, (int)Type.비행] = (int)TypeEffectiveness.굉장;
			table[(int)Type.전기, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.전기, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.전기, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.전기, (int)Type.전기] = (int)TypeEffectiveness.별로;
			table[(int)Type.전기, (int)Type.드래곤] = (int)TypeEffectiveness.별로;
			table[(int)Type.전기, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.드래곤, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.풀] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.독] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.드래곤, (int)Type.드래곤] = (int)TypeEffectiveness.굉장;
			table[(int)Type.드래곤, (int)Type.에스퍼] = (int)TypeEffectiveness.보통;

			table[(int)Type.에스퍼, (int)Type.없음] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.불꽃] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.물] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.풀] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.비행] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.독] = (int)TypeEffectiveness.굉장;
			table[(int)Type.에스퍼, (int)Type.노말] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.얼음] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.전기] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.드래곤] = (int)TypeEffectiveness.보통;
			table[(int)Type.에스퍼, (int)Type.에스퍼] = (int)TypeEffectiveness.별로;
		}
	}
}