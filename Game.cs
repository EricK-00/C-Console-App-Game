using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
	internal class Game
	{
		bool isGameOver;
		Player player;
		readonly string[] battleOrderString = { "첫 번째", "두 번째", "세 번째", "네 번째", "다섯 번째", "여섯 번째", "일곱 번째", "마지막" };

		public Game()
		{
            DataLoader.LoadData();
            player = new Player();
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
            int[] foeIdArray = new int[3];

            ShowStartScreen();

			ShowGuideUI();
			//DebugBattle();

			while (!isGameOver)
			{


                SetCharacterOrder();
				if (Battle(out foeIdArray) && player.WinCount < 8)
				{
					SelectFoeCharacter(foeIdArray);
				}
				else
				{
					isGameOver = true;
				}
			}

			if (player.WinCount == 8)
			{
				WinGame();
			}
			else
			{
				DefeatGame();
			}
        }

        private void ShowStartScreen()
        {
			Screen.RenderScreenOutLine();
			ImageArea titleImage = new ImageArea(Screen.WIDTH / 2 - 21, 0, new string[,]
			{
				{"00", "00", "00", "00", "00" , "00", "00", "00", "00", "00", "00", "00" , "00", "00", "00", "00","00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "00", "00", "00", "00", "00", "00" , "00", "00", "00", "11","00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "00", "00", "00", "00", "00", "00" , "00", "00", "00", "11","00", "11", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "11", "11", "11", "11", "00", "11" , "11", "11", "00", "11","00", "11", "00", "00", "00", "00", "00", "11", "11", "11", "11", "11", "00", "00", "00", "00", "00", "00", "00", "00", "00" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "11", "00", "00", "11", "00", "11" , "00", "11", "00", "11","11", "00", "00", "11", "11", "11", "00", "11", "00", "11", "00", "11", "00", "11", "11", "11", "00", "11", "11", "11", "11" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "11", "00", "00", "11", "00", "11" , "00", "11", "00", "11","11", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "00", "11" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "11", "11", "11", "11", "00", "11" , "11", "11", "00", "11","00", "11", "00", "11", "11", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "11", "00", "00", "11" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "11", "00", "00", "00", "00", "00" , "00", "00", "00", "11","00", "11", "00", "11", "00", "00", "00", "11", "00", "11", "00", "11", "00", "11", "11", "11", "00", "11", "00", "00", "11" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "11", "00", "00", "00", "00", "00" , "00", "00", "00", "00","00", "00", "00", "11", "11", "11", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "11", "00", "00", "00", "00", "00" , "00", "00", "00", "00","00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" ,"00", "00", "00", "00", "00", "00", },
				{"00", "00", "00", "00", "00" , "00", "00", "00", "00", "00", "00", "00" , "00", "00", "00", "00","00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" ,"00", "00", "00", "00", "00", "00", },
			});

			Screen.RenderBitMap(titleImage, "11", "  ", "a");

			TextArea text = new TextArea(Screen.WIDTH / 2 - 9, Screen.HEIGHT - 3, "아무 키나 눌러 게임을 시작하세요.");
			Console.ReadKey(true);
			Animation.FadeView();
		}

		private void ShowGuideUI()
		{
			if (player.WinCount == 0)
			{
				UIPreset.CreateScriptWindow();
				UIPreset.CreateScriptTextArea("배틀팩토리에 오신 것을 환영합니다.", 1, true);
				Console.ReadKey(true);
				UIPreset.CreateScriptTextArea("배틀팩토리는 3마리의 포켓몬을 대여 받아 배틀을 하는 장소입니다.", 2, true);
				Console.ReadKey(true);

				UIPreset.ClearScript(2);
				UIPreset.CreateScriptTextArea("3마리의 포켓몬을 대여해 드리겠습니다.", 1, true);
				Console.ReadKey(true);
				UIPreset.ClearScript(1);
			}
        }

		private void SetCharacterInfoWindows(List<UI> characterWindows, Character[] characters)
		{
            for (int i = 0; i < BattleStage.MAX_BATTLE_CHARACTER; i++)
            {
                characterWindows.Add(
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
				}, true));
            }
        }

		private void SetCharacterOrder()
		{
			Character[] characterOrder = new Character[BattleStage.MAX_BATTLE_CHARACTER];
			List<UI> selectableWindows = new List<UI>();

			SetCharacterInfoWindows(selectableWindows, player.Characters);
			SelectableUI selectableUI = new SelectableUI(selectableWindows, CursorMoveMode.Horizonal);

			char[] word = new char[BattleStage.MAX_BATTLE_CHARACTER] { '첫', '두', '세' };
			bool[] isSelected = new bool[BattleStage.MAX_BATTLE_CHARACTER] { false, false, false };
			int[] prevSelection = new int[BattleStage.MAX_BATTLE_CHARACTER] { 0, 0, 0 };

			for (int i = 0; i < BattleStage.MAX_BATTLE_CHARACTER;)
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
								++i;
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
								++i;
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
								++i;
							}
							else
							{
								UIPreset.CreateScriptTextArea("이미 선택된 포켓몬입니다.", 2, true);
							}
							break;
						case -1:
							if (i > 0)
							{
								--i;
								isSelected[prevSelection[i]] = false;
								isValid = true;
							}
							break;
					}
				}
				UIPreset.ClearScript(1);
				UIPreset.ClearScript(2);
			}

            SelectableUI.HideKeyGuide();
            for (int i = 0; i < player.Characters.Length; i++)
			{
				player.Characters[i] = characterOrder[i];
			}

			UIPreset.CreateScriptTextArea($"그럼 {battleOrderString[player.WinCount]} 배틀을 시작합니다.", 1, true);
			Console.ReadKey(true);
			//Animation.BlinkViewWithColor(5, ConsoleColor.Red, 40);
			Animation.FadeView();
		}

        private bool Battle(out int[] foeId)
        {
			BattleStage battleStage = new BattleStage();
			return battleStage.PlayBattle(player, out foeId);
		}

		private void SelectFoeCharacter(int[] foeId)
		{
            UIPreset.CreateScriptTextArea($"현재 {player.WinCount}승 중입니다.", 1, true);
            Console.ReadKey(true);

            UIPreset.CreateScriptTextArea($"배틀에 승리하였으므로,", 1, true);
            Console.ReadKey(true);
            UIPreset.CreateScriptTextArea($"상대 포켓몬 중 하나와 교환할 수 있습니다.", 2, true);
            Console.ReadKey(true);

            UIPreset.ClearAllScript();

			string[] message = new string[2] { $"교환할 상대 포켓몬을 선택해주세요.", $"교환할 자신의 포켓몬을 선택해주세요." };
			int foeSwapTarget = -1;
			int allySwapTarget = -1;

			Character[] foeCharacters = new Character[BattleStage.MAX_BATTLE_CHARACTER] {
				CharacterData.GetCharacter(foeId[0]),
				CharacterData.GetCharacter(foeId[1]),
				CharacterData.GetCharacter(foeId[2])};

            List<UI> foeCharacterWindows = new List<UI>();
            List<UI> allyCharacterWindows = new List<UI>();
			SetCharacterInfoWindows(foeCharacterWindows, foeCharacters);
			SetCharacterInfoWindows(allyCharacterWindows, player.Characters);
			foeCharacterWindows.Add(new TextArea(Screen.WIDTH/ 2 - 2, UIPreset.WINDOW_Y - 1, "교체안함"));
			allyCharacterWindows.Add(new TextArea(Screen.WIDTH / 2 - 2, UIPreset.WINDOW_Y - 1, "교체안함"));

			SelectableUI selectableUI;

			for (int i = 0; i < 2;)
			{
				if (i == 0)
				{
					foreach(var window in foeCharacterWindows)
						Screen.Render(window);

					selectableUI = new SelectableUI(foeCharacterWindows, CursorMoveMode.Horizonal);
                }	
				else
				{
                    foreach (var window in allyCharacterWindows)
                        Screen.Render(window);

                    selectableUI = new SelectableUI(allyCharacterWindows, CursorMoveMode.Horizonal);
                }

				UIPreset.CreateScriptTextArea($"{message[i]}", 1, true);

				bool isValid = false;
				while (!isValid)
				{
					switch (selectableUI.GetSelection())
					{
						case 0:
							if (i == 0)
							{
								foeSwapTarget = 0;
								Animation.Selected(foeCharacterWindows[0]);
							}
							else
							{
								allySwapTarget = 0;
								Animation.Selected(allyCharacterWindows[0]);
							}

							isValid = true;

							++i;
							break;

						case 1:
							if (i == 0)
							{
								foeSwapTarget = 1;
								Animation.Selected(foeCharacterWindows[1]);
							}
							else
							{
								allySwapTarget = 1;
								Animation.Selected(allyCharacterWindows[1]);
							}

							isValid = true;

							++i;
							break;

						case 2:
							if (i == 0)
							{
								foeSwapTarget = 2;
								Animation.Selected(foeCharacterWindows[2]);
							}
							else
							{
								allySwapTarget = 2;
								Animation.Selected(allyCharacterWindows[2]);
							}

							isValid = true;

							++i;
							break;

						case 3:
							UIPreset.ClearScript(1);
							i = 2;
							isValid = true;
							break;

						case -1:
							if (i > 0)
							{
								--i;
								isValid = true;
							}
							break;
					}
				}
				UIPreset.ClearScript(1);
			}

			if (allySwapTarget != -1 && foeSwapTarget != -1)
			{
                player.Characters[allySwapTarget] = foeCharacters[foeSwapTarget];
			}
            SelectableUI.HideKeyGuide();
            Animation.FadeView();
        }

		private void WinGame()
		{
			UIPreset.CreateScriptTextArea("축하합니다!", 1, true);
			Console.ReadKey(true);
			UIPreset.CreateScriptTextArea("모든 배틀에 승리하여 배틀팩토리를 제패하였습니다!", 2, true);
			Console.ReadKey(true);
			Screen.ClearAll();
			Console.SetCursorPosition(0, 0);

		}

		private void DefeatGame()
		{
			UIPreset.CreateScriptTextArea("배틀에 패배하였습니다.", 1, true);
			Console.ReadKey(true);
			UIPreset.CreateScriptTextArea("다음에 다시 도전해주세요.", 2, true);
			Console.ReadKey(true);
            Screen.ClearAll();
            Console.SetCursorPosition(0, 0);
        }
	}
}