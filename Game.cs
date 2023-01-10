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

		public Game()
		{
            LoadData();
            PrintKeyGuide();
            player = new Player();
        }

		private void PrintKeyGuide()
		{
			Console.SetCursorPosition(0, Screen.HEIGHT + 1);
			Console.Write("키 입력: 다음\n←, →: 커서이동\n스페이스바: 선택");
		}
		private void LoadData()
		{
			SkillData.Initialize(new List<Skill>
			{
				new Skill(0, "default", 0, 100, Type.없음, SkillCategory.변화),
				new Skill(1, "화염방사", 95, 100, Type.불꽃, SkillCategory.특수),
				new Skill(2, "불대문자", 120, 85, Type.불꽃, SkillCategory.특수),
				new Skill(3, "에어슬래시", 80, 100, Type.비행, SkillCategory.특수),
				new Skill(4, "몸통박치기", 40, 100, Type.노말, SkillCategory.물리),
				new Skill(5, "기가드레인", 40, 100, Type.풀, SkillCategory.특수),
				new Skill(6, "파도타기", 95, 100, Type.물, SkillCategory.특수),
				new Skill(7, "오물폭탄", 80, 100, Type.독, SkillCategory.특수)
			});
			CharacterData.Initialize(new List<Character>
			{
				//+75, +20
				new Character(0, "default", Type.없음, Type.없음, new CharacterStats(1,1,1,1,1,1), new Skill[1]{new Skill(0)}),
				new Character(1, "리자몽", Type.불꽃, Type.비행, new CharacterStats(100,100,100,100,100,100), new Skill[4]{new Skill(1), new Skill(2), new Skill(3), new Skill(4)}),
				new Character(2, "이상해꽃", Type.풀, Type.독, new CharacterStats(75,75,125,75,100,125), new Skill[4]{new Skill(4), new Skill(5), new Skill(7), new Skill(6)}),
				new Character(3, "거북왕", Type.물, Type.없음, new CharacterStats(111,99,88,77,66,55), new Skill[4]{new Skill(1), new Skill(4), new Skill(6), new Skill(7)}),
			});
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
			Window mainWindow = new Window(0, 0, Screen.WIDTH, Screen.HEIGHT, ' ');
            TextArea text = new TextArea(Screen.WIDTH / 2 - 7, Screen.HEIGHT - 3, Screen.WIDTH - 2, 1, "아무 키나 눌러 게임 시작");
            Console.ReadKey(true);
        }

		private void InitializeFirstLobbyUI()
		{
			Screen.ClearAll();

            Window viewWindow = new Window(0, 15, 45, 10, ' ');
            TextArea text1 = new TextArea(1, 16, 43, 1, "배틀팩토리에 오신 것을 환영합니다.", UIPreset.PRINT_DELAY);
			Console.ReadKey(true);
			TextArea text2 = new TextArea(1, 17, 43, 1, "배틀팩토리는 3마리의 포켓몬을 대여 받아 배틀을 하는 장소입니다.", UIPreset.PRINT_DELAY);
			Console.ReadKey(true);
			Screen.Clear(text1);
			Screen.Clear(text2);

            //랜덤 생성

            Character[] characters = new Character[CharacterData.BATTLE_CHARACTER_COUNT];
            characters[0] = CharacterData.GetRandomCharacter();
            characters[1] = CharacterData.GetRandomCharacter();
            characters[2] = CharacterData.GetRandomCharacter();

			Window[] selectableWindows = new Window[CharacterData.BATTLE_CHARACTER_COUNT];
			for (int i = 0; i < CharacterData.BATTLE_CHARACTER_COUNT; i++)
			{
				selectableWindows[i] =
				new Window(0 + 15 * i, 0, 15, 14, ' ', new string[] {
					$"『{characters[i].DefaultName}』",
					$"{characters[i].GetTypeString()}",
					$"HP:{characters[i].DefaultStats.Hp}",
					$"물리공격:{characters[i].DefaultStats.Attack}",
					$"물리방어:{characters[i].DefaultStats.Defense}",
					$"특수공격:{characters[i].DefaultStats.SpAttack}",
					$"특수방어:{characters[i].DefaultStats.SpDefense}",
					$"스피드:{characters[i].DefaultStats.Speed}",
					$"",
					$"{characters[i].Skills[0].Name} {characters[i].Skills[1].Name}",
					$"{characters[i].Skills[2].Name} {characters[i].Skills[3].Name}",
					//$"",                                                                                                                                       $"기술4:{characters[i].Skills[0]}",
				}, true);
			}

			List<UI> selectableUIList = new List<UI>() {
				selectableWindows[0],
				selectableWindows[1],
				selectableWindows[2]
			};

            text1 = new TextArea(1, 16, 43, 1, "당신이 대여 받을 포켓몬입니다.", UIPreset.PRINT_DELAY);
			Console.ReadKey(true);
			Screen.Clear(text1);

			InputManager.SetCursorDefault();

            char[] word = new char[CharacterData.BATTLE_CHARACTER_COUNT] { '첫', '두', '세' };
			bool[] isSelected = new bool[CharacterData.BATTLE_CHARACTER_COUNT] { false, false, false };
            for (int i = 0; i < CharacterData.BATTLE_CHARACTER_COUNT; i++)
			{
				text1 = new TextArea(1, 16, 43, 1, $"{word[i]} 번째로 출전할 포켓몬을 선택해주세요.", UIPreset.PRINT_DELAY);

				bool isValid = false;
				while (!isValid)
				{
					switch (InputManager.GetSelection(selectableUIList))
					{
						case 0:
							if (!isSelected[0])
							{
								player.Characters[i] = characters[0];
								isSelected[0] = true;
								isValid = true;
								break;
							}
							else
							{
								Screen.Clear(text2);
								text2 = new TextArea(1, 17, 43, 1, "이미 선택된 포켓몬입니다.", UIPreset.PRINT_DELAY);
								continue;
							}

						case 1:
							if (!isSelected[1])
							{
								player.Characters[i] = characters[1];
                                isSelected[1] = true;
                                isValid = true;
								break;
							}
							else
							{
								Screen.Clear(text2);
								text2 = new TextArea(1, 17, 43, 1, "이미 선택된 포켓몬입니다.", UIPreset.PRINT_DELAY);
								continue;
							}

						case 2:
							if (!isSelected[2])
							{
								player.Characters[i] = characters[2];
                                isSelected[2] = true;
                                isValid = true;
								break;
							}
							else
							{
								Screen.Clear(text2);
								text2 = new TextArea(1, 17, 43, 1, "이미 선택된 포켓몬입니다.", UIPreset.PRINT_DELAY);
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
			viewWindow.RewriteWindowContents(new string[1] { text1.TextString }, false);
			Animation.BlinkWithColor(viewWindow, 7, ConsoleColor.Red);
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
				new Window(Screen.WIDTH / 2 - 3, 2, 10, 10, '='),
				new Window(Screen.WIDTH - 1 - 7, 2, 10, 10, '-'),
				new TextArea(Screen.WIDTH / 2 - 2, 13, 10, 1, "교체안함")
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
            //Animation.BlinkViewWithColor(7, ConsoleColor.Red);
			Screen.ClearAll();

			UIPreset.CreateScriptWindow();

            Window allyWindow = new Window(5 - 1, 0, 15, 15, ' ');
			Window foeWindow = new Window(Screen.WIDTH - 5 - 1 - 15, 0, 15, 15, ' ');

			Battle battle = new Battle();
			battle.PlayBattle(player);
		}
	}
}