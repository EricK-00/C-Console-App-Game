using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	enum Object
	{
		NONE = 0,
		WALL
	}

	internal class MapGenerator
	{
		int[,] map;
		int mapRow;
		int mapCol;

		public MapGenerator(int row, int col)
		{
			mapRow= row;
			mapCol= col;
			map = new int[mapRow, mapCol];
			InitializeWall();
		}

		private void InitializeWall()
		{
			Random random = new Random();

			for (int i = 0; i < mapRow; i++)
			{
				for (int j = 0; j < mapCol; j++)
				{
					if (random.Next(0, 100) < 45)
					{
						map[i, j] = (int)Object.WALL;
					}
					else
					{
						map[i, j] = (int)Object.NONE;
					}
				}
			}
		}

		public void GenerateMap()
		{
			for (int i = 0; i < 4; i++)
				ChangeObject();
		}

		private void ChangeObject()
		{
			for (int i = 0; i < mapRow; i++)
			{
				for (int j = 0; j < mapCol; j++)
				{
					CheckNearObject(i, j);
				}
			}
		}

		private void CheckNearObject(int i, int j)
		{
			int nearObjectCount = 8;
			int wallCount = 0;

			for (int k = -1; k <= 1; k++)
			{
				for (int l = -1; l <= 1; l++)
				{
					int objectValue = GetObjectValue(i + k, j + l);
					if (objectValue == -1)
						--nearObjectCount;
					else
						wallCount += objectValue;
				}
			}

			if (wallCount > nearObjectCount / 2)
				map[i, j] = (int)Object.WALL;
			else
				map[i, j] = (int)Object.NONE;
		}

		private int GetObjectValue(int i, int j)
		{
			if (i < 0 || i > mapRow - 1 || j < 0 || j > mapCol - 1)
				return -1;

			return map[i, j];
		}

		public void PrintMap()
		{
			for (int i = 0; i < mapRow; i++)
			{
				for (int j = 0; j < mapCol; j++)
				{
					switch (map[i, j])
					{
						case (int)Object.NONE:
							Console.Write("□");
							break;
						case (int)Object.WALL:
							Console.Write("■");
							break;
					}
				}
				Console.WriteLine();
			}
		}
	}
}
