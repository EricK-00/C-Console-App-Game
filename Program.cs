namespace CSharpConsoleAppGame
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			//Timer timer = new Timer(Screen.RenderScreen, null, 0, 50);

			List<string> strings = new List<string>();
			int row, col;
			string str = "../../../files/csvfile.csv";
			if (File.Exists(str) == false)
			{
				Console.WriteLine("!");
				int.TryParse(Console.ReadLine(), out row);
				int.TryParse(Console.ReadLine(), out col);
			}
			else
			{
				string[] parsedStr;
				StreamReader streamReader = new StreamReader(str);

				while (!streamReader.EndOfStream)
				{
					parsedStr = streamReader.ReadToEnd().Split(',');
					for (int j = 0; j < parsedStr.Length; j++)
					{
						//if (parsedStr[j] != " " && parsedStr[j] != "\n" && parsedStr[j] != "\t" && parsedStr[j] != null)
						{
							Console.Write(parsedStr[j] + "^");
							//strings.Add(parsedStr[j]);
						}
					}
				}
				streamReader.Close();

				for (int i = 0; i < strings.Count; i++)
					Console.Write($"{strings[i]} ");
				Console.WriteLine($"str C{strings.Count}");
			}

			Screen.Initialize();
			Screen.RenderScreen(null);

			Window selectionWindow = new Window(2, 2, 10, 10);
			Window viewWindow = new Window(10, 1, 20, 10);
			//Screen.PlaceOnScreen(selectionWindow);
			Screen.PlaceOnScreen(viewWindow);
			Console.ReadLine();
			Screen.RenderScreen(null);

			Console.ReadLine();

			Animation animation = new Animation();
			animation.BlinkImage(viewWindow, 5);

			//Screen.RenderScreen(null);

			Console.ReadLine();
		}
	}
}