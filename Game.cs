using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class Game
	{
		bool isGameover;
		bool winTheBattle;
		string gameState;
		Player player;
		const int PRINT_DELAY = 25;

		public Game()
		{
			player = new Player();

			PrintKeyGuide();
        }

		private void PrintKeyGuide()
		{
            Console.SetCursorPosition(0, Screen.SCREEN_HEIGHT + 1);
            Console.Write("키 입력: 다음\n←, →: 커서이동\n스페이스바: 선택");
        }

		private void LoadData()
		{

		}

		public void PlayGame()
		{
            Console.CursorVisible = false;

			InitializeMainUI();

            InitializeFirstLobbyUI();

			InitializeBattleUI();
        }

        private void InitializeMainUI()
        {
			Screen.Initialize();
			Window mainWindow = new Window(0, 0, Screen.SCREEN_WIDTH, Screen.SCREEN_HEIGHT, ' ');
            TextArea text = new TextArea(Screen.SCREEN_WIDTH / 2 - 7, Screen.SCREEN_HEIGHT - 3, Screen.SCREEN_WIDTH - 2, 1, "아무 키나 눌러 게임 시작");
			Console.ReadKey(true);
		}

		private void InitializeFirstLobbyUI()
		{
			Screen.ClearAll();

            Window viewWindow = new Window(0, 15, 45, 10, ' ');
            TextArea text1 = new TextArea(1, 16, 43, 1, "배틀팩토리에 오신 것을 환영합니다.", PRINT_DELAY);
			Console.ReadKey(true);
			TextArea text2 = new TextArea(1, 17, 43, 1, "배틀팩토리는 3마리의 포켓몬을 대여 받아 배틀을 하는 장소입니다.", PRINT_DELAY);
			Console.ReadKey(true);
			Screen.Clear(text1);
			Screen.Clear(text2);

			//랜덤 생성
			Character[] characters = new Character[3]; 
			characters[0] = new Character("리자몽", new CharacterStats(105, 110, 80, 108), Type.불꽃, Type.비행);
			characters[1] = new Character("리자몽", new CharacterStats(100, 110, 80, 108));
			characters[2] = new Character("리자몽", new CharacterStats(99, 110, 80, 1));

			List<UI> selectableUIList = new List<UI>() {
				new Window(0, 0, 15, 14, '-', new string[] {
					$"[{characters[0].Name}]",
					$"{characters[0].GetTypeString()}",
					$"HP:{characters[0].Stats.Hp}",
					$"공격:{characters[0].Stats.Attack}",
                    $"방어:{characters[0].Stats.Defense}",
                    $"특수공격:{characters[0].Stats.Attack}",
                    $"특수방어:{characters[0].Stats.Defense}",
                    $"스피드:{characters[0].Stats.Speed}",
                }, true),
				new Window(Screen.SCREEN_WIDTH / 2 - 7, 0, 15, 14, '-', new string[] {
                    $"[{characters[1].Name}]",
                    $"타입:{characters[1].GetTypeString()}",
                    $"HP:{characters[1].Stats.Hp}",
                    $"공격:{characters[1].Stats.Attack}",
                    $"방어:{characters[1].Stats.Defense}",
                    $"스피드:{characters[1].Stats.Speed}",
                }, true),
				new Window(Screen.SCREEN_WIDTH - 15, 0, 15, 14, '-', new string[] {
                    $"[{characters[2].Name}]",
                    $"타입:{characters[2].GetTypeString()}",
                    $"HP:{characters[2].Stats.Hp}",
                    $"공격:{characters[2].Stats.Attack}",
                    $"방어:{characters[2].Stats.Defense}",
                    $"스피드:{characters[2].Stats.Speed}",
                }, true)
			};

            text1 = new TextArea(1, 16, 43, 1, "당신이 대여 받을 포켓몬입니다.", PRINT_DELAY);
			Console.ReadKey(true);
			Screen.Clear(text1);

			InputManager.SetCursorDefault();
			for (int i = 0; i < Player.MAX_CHARACTER_COUNT; i++)
			{
				char[] word = new char[3] {'첫', '두', '세'};

				text1 = new TextArea(1, 16, 43, 1, $"{word[i]} 번째로 출전할 포켓몬을 선택해주세요.", PRINT_DELAY);

				bool isValid = false;
				while (!isValid)
				{
					switch (InputManager.GetSelection(selectableUIList))
					{
						case 0:
							if (player.Characters[0] != characters[0] && player.Characters[1] != characters[0])
							{
								player.Characters[i] = characters[0];
								isValid = true;
								break;
							}
							else
							{
								Screen.Clear(text2);
								text2 = new TextArea(1, 17, 43, 1, "이미 선택된 포켓몬입니다.", PRINT_DELAY);
								continue;
							}

						case 1:
							if (player.Characters[0] != characters[1] && player.Characters[1] != characters[1])
							{
								player.Characters[i] = characters[1];
								isValid = true;
								break;
							}
							else
							{
								Screen.Clear(text2);
								text2 = new TextArea(1, 17, 43, 1, "이미 선택된 포켓몬입니다.", PRINT_DELAY);
								continue;
							}

						case 2:
							if (player.Characters[0] != characters[2] && player.Characters[1] != characters[2])
							{
								player.Characters[i] = characters[2];
								isValid = true;
								break;
							}
							else
							{
								Screen.Clear(text2);
								text2 = new TextArea(1, 17, 43, 1, "이미 선택된 포켓몬입니다.", PRINT_DELAY);
								continue;
							}
					}

				}
				Screen.Clear(text1);
				Screen.Clear(text2);
			}

			foreach (UI ui in selectableUIList)
				Screen.Clear(ui);
			Screen.Clear(text1);

            text1 = new TextArea(1, 16, 43, 1, "그럼 첫 번째 배틀을 시작합니다.", 50);
            Console.ReadKey(true);
        }

		private void InitializeSelectionUI()
        {
			Screen.ClearAll();

			while (true)
			{
				Window viewWindow = new Window(0, 15, 45, 10, ' ');
				TextArea textt = new TextArea(1, 16, 30, 10, "교체할 포켓몬을 선택해 주세요.", 25);

				List<UI> selectableUIList = new List<UI>() {
				new Window(1, 2, 10, 10, '+'),
				new Window(Screen.SCREEN_WIDTH / 2 - 3, 2, 10, 10, '='),
				new Window(Screen.SCREEN_WIDTH - 1 - 7, 2, 10, 10, '-'),
				new TextArea(Screen.SCREEN_WIDTH / 2 - 2, 13, 10, 1, "교체안함")
			};

				switch (InputManager.GetSelection(selectableUIList))
				{
					case 0:
						TextArea text = new TextArea(1, 16, 10, 10, "1번 선택", 100);
						break;

					case 1:
						TextArea text1 = new TextArea(1, 17, 10, 10, "2번 선택", 100);
						break;

					case 2:
						TextArea text2 = new TextArea(1, 18, 10, 10, "3번 선택");
						Console.ReadKey(true);
						break;
				}
			}
		}

        private void InitializeBattleUI()
        {
            //Animation.BlinkView(7, 'x');
            Animation.BlinkViewWithColor(7, ConsoleColor.Red);
			Screen.ClearAll();

            Window viewWindow = new Window(0, 15, 45, 10, ' ');

            Window allyWindow = new Window(5 - 1, 5, 15, 15, ' ');
			Window foeWindow = new Window(Screen.SCREEN_WIDTH - 5 - 1 - 15, 5, 15, 15, ' ');

			InBattleCharacter allyCharacter = new InBattleCharacter(player.Characters[0]);

			//랜덤 생성
			Character[] foeCharacters = new Character[Player.MAX_CHARACTER_COUNT];
			foeCharacters[0] = new Character("리자몽", new CharacterStats(105, 110, 80, 108));
			foeCharacters[1] = new Character("리자몽", new CharacterStats(100, 110, 80, 108));
			foeCharacters[2] = new Character("리자몽", new CharacterStats(99, 110, 80, 1));

			InBattleCharacter foeCharacter = new InBattleCharacter(foeCharacters[0]);

			Console.ReadKey(true);
		}
	}
}