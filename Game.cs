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
		bool isGameOver;
		Player player;
		Window scriptWindow;
		readonly string[] battleOrderString = { "첫", "두", "세", "네", "다섯", "여섯", "일곱", "마지막" };

		public Game()
		{
            DataLoader.LoadData();
            PrintKeyGuide();
            player = new Player();
        }

		private void PrintKeyGuide()
		{
			Console.SetCursorPosition(0, Screen.HEIGHT + 1);
			Console.Write($"키 입력: 다음\n방향키: 커서이동\n스페이스바: 선택\n선택 취소: q");
		}

		private void DebugBattle()
		{
			player.Characters[0] = CharacterData.GetRandomCharacter();
			player.Characters[1] = CharacterData.GetRandomCharacter();
			player.Characters[2] = CharacterData.GetRandomCharacter();
		}

		public void PlayGame()
		{
            Console.CursorVisible = false;

			ShowStartScreenUI();

			while (!isGameOver)
			{
				//DebugBattle();
				ShowLobbyUI();
				ShowBattleUI();
			}

			if (player.WinCount == 10)
			{
				WinGame();
			}
			else
			{
				LoseGame();
			}
        }

        private void ShowStartScreenUI()
        {
			Screen.Initialize();
			Window startScreenWindow = new Window(0, 0, Screen.WIDTH, Screen.HEIGHT, ' ');
            TextArea text = new TextArea(Screen.WIDTH / 2 - 7, Screen.HEIGHT - 3, Screen.WIDTH - 2, 1, "아무 키나 눌러 게임 시작");
            Console.ReadKey(true);
			Screen.Clear(text);
        }

		private void ShowLobbyUI()
		{
			if (player.WinCount == 0)
			{
				scriptWindow = new Window(0, 15, 45, 10, ' ');
				UIPreset.CreateScriptTextArea("배틀팩토리에 오신 것을 환영합니다.", 1, true);
				Console.ReadKey(true);
				UIPreset.CreateScriptTextArea("배틀팩토리는 3마리의 포켓몬을 대여 받아 배틀을 하는 장소입니다.", 2, true);
				Console.ReadKey(true);

				UIPreset.ClearScript(2);
				UIPreset.CreateScriptTextArea("3마리의 포켓몬을 대여해 드리겠습니다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
			}

			SetCharacterOrder();
        }

		private void SetCharacterOrder()
		{
			Character[] characterOrder = new Character[CharacterData.BATTLE_CHARACTER_COUNT];
			List<UI> selectableWindows = new List<UI>();

			for (int i = 0; i < CharacterData.BATTLE_CHARACTER_COUNT; i++)
			{
				selectableWindows.Add(
				new Window(0 + 15 * i, 0, 15, 14, ' ', new string[] {
					$"『{player.Characters[i].DefaultName}』",
					$"{player.Characters[i].GetTypeString()}",
					$"HP:{player.Characters[i].DefaultStats.Hp}",
					$"물리공격:{player.Characters[i].DefaultStats.Attack}",
					$"물리방어:{player.Characters[i].DefaultStats.Defense}",
					$"특수공격:{player.Characters[i].DefaultStats.SpAttack}",
					$"특수방어:{player.Characters[i].DefaultStats.SpDefense}",
					$"스피드:{player.Characters[i].DefaultStats.Speed}",
					$"",
					$"{player.Characters[i].Skills[0].Name} {player.Characters[i].Skills[1].Name}",
					$"{player.Characters[i].Skills[2].Name} {player.Characters[i].Skills[3].Name}",
					//$"",                                                                                                                                       $"기술4:{characters[i].Skills[0]}",
				}, true));
			}

			SelectableUI selectableUI = new SelectableUI(selectableWindows, CursorMoveMode.Horizonal);
			char[] word = new char[CharacterData.BATTLE_CHARACTER_COUNT] { '첫', '두', '세' };
			bool[] isSelected = new bool[CharacterData.BATTLE_CHARACTER_COUNT] { false, false, false };
			int[] prevSelection = new int[CharacterData.BATTLE_CHARACTER_COUNT] { 0, 0, 0 };
			for (int i = 0; i < CharacterData.BATTLE_CHARACTER_COUNT; i++)
			{
				UIPreset.CreateScriptTextArea($"{word[i]} 번째로 출전할 포켓몬을 선택해주세요.", 1, true);

				bool isValid = false;
				while (!isValid)
				{
					switch (selectableUI.GetSelection())
					{
						case 0:
							if (!isSelected[0])
							{
								characterOrder[i] = player.Characters[0];
								isSelected[0] = true;
								prevSelection[i] = 0;
								isValid = true;
								Animation.Selected(selectableWindows[0]);
							}
							else
							{
								UIPreset.CreateScriptTextArea("이미 선택된 포켓몬입니다.", 2, true);
							}
							break;
						case 1:
							if (!isSelected[1])
							{
								characterOrder[i] = player.Characters[1];
								isSelected[1] = true;
								prevSelection[i] = 1;
								isValid = true;
								Animation.Selected(selectableWindows[1]);
							}
							else
							{
								UIPreset.CreateScriptTextArea("이미 선택된 포켓몬입니다.", 2, true);
							}
							break;
						case 2:
							if (!isSelected[2])
							{
								characterOrder[i] = player.Characters[2];
								isSelected[2] = true;
								prevSelection[i] = 2;
								isValid = true;
								Animation.Selected(selectableWindows[2]);
							}
							else
							{
								UIPreset.CreateScriptTextArea("이미 선택된 포켓몬입니다.", 2, true);
							}
							break;
						case -1:
							if (i != 0)
							{
								i -= 2;
								isSelected[prevSelection[i + 1]] = false;
								isValid = true;
							}
							break;
					}
				}
				UIPreset.ClearScript(1);
				UIPreset.ClearScript(2);
			}

			for (int i = 0; i < player.Characters.Length; i++)
			{
				player.Characters[i] = characterOrder[i];
			}

			UIPreset.CreateScriptTextArea($"그럼 {battleOrderString[player.WinCount]} 번째 배틀을 시작합니다.", 1, true);
			Console.ReadKey(true);
			//Animation.BlinkViewWithColor(7, ConsoleColor.Red, 35);
			Animation.FadeView(30);
		}

        private void ShowBattleUI()
        {
            //Animation.BlinkView(7, 'x');
            //Animation.BlinkViewWithColor(7, ConsoleColor.Red);
			Screen.ClearAll();

			UIPreset.CreateScriptWindow();

			int[] foeId;
			Battle battle = new Battle();
			if (battle.PlayBattle(player, out foeId))
			{
				//Animation
				UIPreset.CreateScriptTextArea($"현재 {player.WinCount}승 중입니다.", 1, true);
				Console.ReadKey(true);

				//교체 선택
				UIPreset.CreateScriptTextArea($"배틀에 승리하였으므로,", 1, true);
				Console.ReadKey(true);
				UIPreset.CreateScriptTextArea($"상대 포켓몬 중 하나와 교환할 수 있습니다.", 2, true);
				Console.ReadKey(true);
				UIPreset.ClearAllScript();


				UIPreset.CreateScriptTextArea($"교환할 상대 포켓몬을 선택해주세요.", 1, true);
				Console.ReadKey(true);
				UIPreset.CreateScriptTextArea($"교환할 자신의 포켓몬을 선택해주세요.", 1, true);
				Console.ReadKey(true);

				//순서 선택
				SetCharacterOrder();
			}
			else
			{
				isGameOver = true;
			}
		}

		private void WinGame()
		{
			UIPreset.CreateScriptTextArea("축하합니다!", 1, true);
			Console.ReadKey(true);
			UIPreset.CreateScriptTextArea("모든 배틀에 승리하여 배틀펙토리를 제패하였습니다!", 2, true);
			Console.ReadKey(true);
			Console.SetCursorPosition(100, 100);
		}

		private void LoseGame()
		{
			UIPreset.CreateScriptTextArea("배틀에 패배하였습니다.", 1, true);
			Console.ReadKey(true);
			UIPreset.CreateScriptTextArea("다음에 다시 도전해주세요.", 2, true);
			Console.ReadKey(true);
			Console.SetCursorPosition(100, 100);
		}
	}
}