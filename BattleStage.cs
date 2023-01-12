using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CSharpConsoleAppGame
{
    internal class BattleStage
    {
        public static bool BATTLE_DEBUG_MODE = false;

        public const int MAX_BATTLE_CHARACTER = 3;

        int battleTurn = 0;

        InBattleCharacter[] allyCharacters;
        InBattleCharacter[] foeCharacters;

        Window allyWindow;
        Window foeWindow;

        bool winTheBattle = false;
        bool battleEnd = false;
        bool setNewAlly = true;
        bool setNewFoe = true;

        public BattleStage()
        {
            allyWindow = new Window(5, 0, 17, 15, ' ', true);
            foeWindow = new Window(Screen.WIDTH - 7 - 15, 0, 17, 15, ' ', true);
        }

        public bool PlayBattle(Player player, out int[] foeId)
        {
            SetUpBattleCharacter(player);
            foeId = new int[MAX_BATTLE_CHARACTER] { foeCharacters[0].Id, foeCharacters[1].Id, foeCharacters[2].Id };

            while (!battleEnd)
            {
                ++battleTurn;

                if (setNewFoe)
                {
                    FoeThrowBall();
                    setNewFoe = false;
                }
                if (setNewAlly)
                {
                    ThrowBall();
                    setNewAlly = false;
                }

                allyCharacters[0].AddTurnCount();
                foeCharacters[0].AddTurnCount();

                int mySkillIndex = SelectAction();
                int foeSkillIndex = FoeSelectAction();
                SelectableUI.HideKeyGuide();

                bool isAllyFirst = GetAttackerOrder(allyCharacters[0], foeCharacters[0], mySkillIndex, foeSkillIndex);

                (InBattleCharacter firstAttacker, InBattleCharacter secondAttacker) = isAllyFirst ?
                    (allyCharacters[0], foeCharacters[0]) : (foeCharacters[0], allyCharacters[0]);
                (int firstASkillIndex, int secondASkillIndex) = isAllyFirst ? (mySkillIndex, foeSkillIndex) : (foeSkillIndex, mySkillIndex);


                if (mySkillIndex == -1)
                {
                    PlayTurnAction(foeCharacters[0], allyCharacters[0], foeSkillIndex);

                    if (firstAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(firstAttacker);
                    if (secondAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(secondAttacker);
                }
                else
                {
                    if (firstAttacker.BattleStats.Hp > 0)
                        PlayTurnAction(firstAttacker, secondAttacker, firstASkillIndex);
                    if (secondAttacker.BattleStats.Hp > 0)
                        PlayTurnAction(secondAttacker, firstAttacker, secondASkillIndex);

                    if (firstAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(firstAttacker);
                    if (secondAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(secondAttacker);
                }

                if (foeCharacters[0].BattleStats.Hp <= 0)
                {
                    if (!SwapBattleOrder(ref foeCharacters[0], ref foeCharacters[1]) && !SwapBattleOrder(ref foeCharacters[0], ref foeCharacters[2]))
                    {
                        winTheBattle = true;
                        battleEnd = true;
                        break;

                    }
                    else
                    {
                        setNewFoe = true;
                    }
                }

				if (allyCharacters[0].BattleStats.Hp <= 0)
				{
					if (allyCharacters[1].BattleStats.Hp <= 0 &&
						allyCharacters[2].BattleStats.Hp <= 0)
					{
						winTheBattle = false;
						battleEnd = true;
                        break;
					}
					else
					{
						Replace(false);
						setNewAlly = true;
						SelectableUI.HideKeyGuide();
					}
				}
			}

            BattleResult(winTheBattle, player);

            Animation.FadeView();
            return winTheBattle;
        }

        private void BattleResult(bool win, Player player)
        {
            if (win)
            {
                UIPreset.CreateScriptTextArea("배틀에 승리했다!", 1, true);
                Console.ReadKey(true);
                player.WinTheBattle();
            }
            else
            {
                UIPreset.CreateScriptTextArea("눈앞이 깜깜해졌다...", 1, true);
                Console.ReadKey(true);
            }
        }

        private void SetUpBattleCharacter(Player player)
        {
            allyCharacters = new InBattleCharacter[MAX_BATTLE_CHARACTER];
            Character[] foeArray = new Character[MAX_BATTLE_CHARACTER];
            foeCharacters = new InBattleCharacter[MAX_BATTLE_CHARACTER];

            for (int i = 0; i < MAX_BATTLE_CHARACTER; i++)
            {
                allyCharacters[i] = new InBattleCharacter(player.Characters[i], OwnerType.MINE, allyWindow);
                foeArray[i] = CharacterData.GetRandomCharacter();
                foeCharacters[i] = new InBattleCharacter(foeArray[i], OwnerType.FOE, foeWindow);
            }
        }

        private void ReturnToBall(int prevCharacterIndex)
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea($"돌아와, {allyCharacters[prevCharacterIndex].BattleName}", 1, true);
            Console.ReadKey(true);

            allyWindow.ClearContents();
        }

        private void ThrowBall()
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea($"가랏, {allyCharacters[0].BattleName}!", 1, true);
            Console.ReadKey(true);

            allyCharacters[0].RerenderWindow();

		}

        private void FoeThrowBall()
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea($"상대는 {foeCharacters[0].DefaultName}을(를) 내보냈다.", 1, true);
            Console.ReadKey(true);

			foeCharacters[0].RerenderWindow();
		}

        private int SelectAction()
        {
            int mySelecion = -1;
            bool isValid = false;

            UIPreset.ClearAllScript();

            List<UI> fightOrReplace = new List<UI>()
            {
                new Window(Screen.WIDTH / 2 - 13, UIPreset.WINDOW_Y + 3, 7, 4, ' ', '=', new string[1] {"싸운다"}, true, true),
                new Window(Screen.WIDTH / 2 + 6, UIPreset.WINDOW_Y + 3, 8, 4, ' ', '=', new string[1] {"교체한다"}, true, true)
        };

            SelectableUI selectableUI = new SelectableUI(fightOrReplace, CursorMoveMode.Horizonal);

            while (!isValid)
            {
                switch (selectableUI.GetSelection())
                {
                    case 0:
                        mySelecion = SelectSkill();
                        if (mySelecion == -1)
                        {
                            UIPreset.ClearAllScript();
                            Screen.Render(fightOrReplace[0]);
                            Screen.Render(fightOrReplace[1]);
                            selectableUI = new SelectableUI(fightOrReplace, CursorMoveMode.Horizonal);
                            continue;
                        }
                        else
                            isValid = true;
                        break;

                    case 1:
                        bool end = Replace(true);
                        if (!end)
                        {
                            UIPreset.ClearAllScript();
                            Screen.Render(fightOrReplace[0]);
                            Screen.Render(fightOrReplace[1]);
                            selectableUI = new SelectableUI(fightOrReplace, CursorMoveMode.Horizonal);
                            continue;
                        }
                        else
                            isValid = true;
                        break;
                }
            }

            UIPreset.ClearAllScript();
            return mySelecion;
        }

        private int SelectSkill()
        {
            UIPreset.ClearAllScript();

            List<UI> skills = new List<UI>();

            for (int i = 0; i < allyCharacters[0].Skills.Length; i++)
            {
                skills.Add(
                new Window(Screen.WIDTH / 2 - 17 + 18 * (i % 2), UIPreset.WINDOW_Y + 1 + 4 * (i / 2), 17, 4, ' ', '=',
                new string[2] {
                    $"{allyCharacters[0].Skills[i].Name}「{allyCharacters[0].Skills[i].SkillType}」",
                    $"위력{allyCharacters[0].Skills[i].PowerString()} 명중{allyCharacters[0].Skills[i].HitRateString()}〈{allyCharacters[0].Skills[i].Category}〉"}, true, true));
            }

            SelectableUI selectableUI = new SelectableUI(skills, CursorMoveMode.Square, 2, 2);

            while (true)
            {
                switch (selectableUI.GetSelection())
                {
                    case 0:
                        if (allyCharacters[0].Skills[0].Id == 0)
                            continue;

                        UIPreset.ClearAllScript();
                        return 0;

                    case 1:
                        if (allyCharacters[0].Skills[1].Id == 0)
                            continue;

                        UIPreset.ClearAllScript();
                        return 1;

                    case 2:
                        if (allyCharacters[0].Skills[2].Id == 0)
                            continue;

                        UIPreset.ClearAllScript();
                        return 2;

                    case 3:
                        if (allyCharacters[0].Skills[3].Id == 0)
                            continue;

                        UIPreset.ClearAllScript();
                        return 3;

                    case -1:
                        return -1;
                }
            }
        }

        private bool Replace(bool isAlive)
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea("내보낼 포켓몬을 선택하세요.", 1, false);

            List<UI> characters = new List<UI>();
            for (int i = 0; i < MAX_BATTLE_CHARACTER; i++)
            {
                characters.Add(
                new Window(1 + 15 * i, UIPreset.WINDOW_Y + 2, 13, 7, ' ', '=', new string[] {
                    $"『{allyCharacters[i].DefaultName}』",
                    $"{allyCharacters[i].GetTypeString()}",
                    $"{allyCharacters[i].GetHpString()}",
                    $"",
                    $"{allyCharacters[i].GetStatusString()}"
                }, true, true));
            }

            SelectableUI selectableUI = new SelectableUI(characters, CursorMoveMode.Horizonal);

            while (true)
            {
                switch (selectableUI.GetSelection())
                {
                    case 0:
                        if (isAlive)
                            UIPreset.CreateScriptTextArea("전투 중인 포켓몬입니다.", 1, true);
                        else
                            UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 1, true);

                        Console.ReadKey(true);
                        UIPreset.ClearScript(1);
                        UIPreset.CreateScriptTextArea("내보낼 포켓몬을 선택하세요.", 1, false);
                        continue;
                    case 1:
                        if (!SwapBattleOrder(ref allyCharacters[0], ref allyCharacters[1]))
                        {
                            UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 1, true);
                            Console.ReadKey(true);
                            UIPreset.ClearScript(1);
                            UIPreset.CreateScriptTextArea("내보낼 포켓몬을 선택하세요.", 1, false);
                            continue;
                        }

                        if (isAlive)
                        {
                            ReturnToBall(1);
                            ThrowBall();
                        }
                        return true;

                    case 2:
                        if (!SwapBattleOrder(ref allyCharacters[0], ref allyCharacters[2]))
                        {
                            UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 1, true);
                            Console.ReadKey(true);
                            UIPreset.ClearScript(1);
                            UIPreset.CreateScriptTextArea("내보낼 포켓몬을 선택하세요.", 1, false);
                            continue;
                        }

                        if (isAlive)
                        {
                            ReturnToBall(2);
                            ThrowBall();
                        }
                        return true;

                    case -1:
                        if (isAlive)
                        {
                            return false;
                        }
                        else
                            continue;
                }
            }
        }

        private int FoeSelectAction()
        {
            return new Random().Next(0, 3 + 1);
        }

        private bool GetAttackerOrder(InBattleCharacter character1, InBattleCharacter character2, int skillIndex1, int skillIndex2)
        {
            if (skillIndex1 != -1 && skillIndex2 != -1)
            {
                if (character1.Skills[skillIndex1].Priority > character2.Skills[skillIndex2].Priority)
                {
                    return true;
                }
                else if (character1.Skills[skillIndex1].Priority < character2.Skills[skillIndex2].Priority)
                {
                    return false;
                }
            }

			if (character1.BattleStats.Speed == character2.BattleStats.Speed)
            {
                if (new Random().Next(0, 100) < 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (character1.BattleStats.Speed > character2.BattleStats.Speed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SwapBattleOrder(ref InBattleCharacter mainCharacter, ref InBattleCharacter targetCharacter)
        {
            if (targetCharacter.BattleStats.Hp <= 0)
                return false;

            InBattleCharacter temp = mainCharacter;
            mainCharacter = targetCharacter;
            targetCharacter = temp;
            return true;
        }

        private void PlayTurnAction(InBattleCharacter attacker, InBattleCharacter defender, int skillIndex)
        {
            bool skipAction = false;
            bool isCritical = false;
            Skill skill = attacker.Skills[skillIndex];

            skipAction = attacker.CheckSkipAction(defender.BattleStats.Hp, skillIndex);

            if (!skipAction)
            {
                UIPreset.CreateScriptTextArea($"{attacker.GetSkillMessage(skillIndex)}", 1, true);
                Console.ReadKey(true);

                if (skill.Category != SkillCategory.변화)
                {
                    float mainTypeBonus = 1f;
                    if (attacker.FirstType == skill.SkillType ||
                    attacker.SecondType == skill.SkillType)
                    {
                        mainTypeBonus = 1.5f;
                    }

                    int typeEffectiveness = TypeTable.table[(int)skill.SkillType, (int)defender.FirstType] *
                        TypeTable.table[(int)skill.SkillType, (int)defender.SecondType];

                    float criticalDamage = 1f;
                    if (new Random().Next(0, 16) == 0)
                    {
                        criticalDamage = 1.5f;
                        isCritical = true;
                    }

                    int randomValue = new Random().Next(217, 255 + 1);


                    int damage = (skill.Category == SkillCategory.물리) ?
                        (int)(((22 * skill.Power * (float)attacker.BattleStats.Attack / 50) / defender.BattleStats.Defense + 2)
                         * criticalDamage * ((float)randomValue / 255) * mainTypeBonus * ((float)typeEffectiveness / 4)) :
                        (int)(((22 * skill.Power * (float)attacker.BattleStats.SpAttack / 50) / defender.BattleStats.SpDefense + 2)
                        * criticalDamage * ((float)randomValue / 255) * mainTypeBonus * ((float)typeEffectiveness / 4));

                    defender.Damaged(damage, typeEffectiveness, isCritical);

                    if (defender.Status == StatusCondition.얼음 && skill.SkillType == Type.불꽃 && defender.BattleStats.Hp > 0)
                    {
                        UIPreset.CreateScriptTextArea("얼음이 풀렸다!", 1, true);
                        defender.SetStatusCondition(StatusCondition.없음);
                        Console.ReadKey(true);
                        UIPreset.ClearScript(1);
                    }
                }

                if (attacker.BattleStats.Hp > 0)
                {
                    if (BATTLE_DEBUG_MODE)
                        skill.Effect.Invoke(attacker, defender);
                    else
                    {
                        if (new Random().Next(0, 100) < skill.EffectRate)
                            skill.Effect.Invoke(attacker, defender);
                    }
                }
            }
        }

        private void PlayTurnEndAction(InBattleCharacter character)
        {
            character.DamagedFromStatusCondition();
        }
    }
}
