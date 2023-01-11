using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class BattleStage
    {
        //캐릭터 내보내기

        //연속기 체크
        //키 선택
        //기술 선택(플레이어는 직접, 적은 랜덤 선택/가중치 선택)
        //스피드 체크

        //[선 공격자]
        //혼란 체크
        //선 상태이상 체크
        //스킬
        //명중률체크
        //데미지계산(랭크/상태이상반영 실능력치 + 자속체크 + 타입상성체크 + 급소 체크) + 상대 체력 체크
        //스킬부가효과, 풀죽음 체크

        //[후 공격자]
        //위와 동일

        //날씨

        //선 공격자의 후 상태이상 체크
        //후 공격자의 후 상태이상 체크

        //++배틀 캐릭터 턴 카운트
        //++배틀 턴 카운트

        //[]
        //교체
        //배틀 캐릭터 [0]번과 [x]번이 교체
        //교체 전 배틀 캐릭터 턴 카운트 초기화(=1), 교체 전 배틀 캐릭터 랭크 초기화

        public const int MAX_BATTLE_CHARACTER = 3;
        const int HP_TEXT_LINE = 4;
        const int CONDITION_TEXT_LINE = 13;

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
			allyWindow = new Window(5, 0, 17, 15, ' ');
			foeWindow = new Window(Screen.WIDTH - 7 - 15, 0, 17, 15, ' ');
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

                bool isAllyFirst = GetAttackerOrder(allyCharacters[0], foeCharacters[0]);

                (InBattleCharacter firstAttacker, InBattleCharacter secondAttacker) = isAllyFirst ?
                    (allyCharacters[0], foeCharacters[0]) : (foeCharacters[0], allyCharacters[0]);
                (Window firstAWindow, Window secondAWindow) = isAllyFirst ? (allyWindow, foeWindow) : (foeWindow, allyWindow);
                (int firstASkillIndex, int secondASkillIndex) = isAllyFirst ? (mySkillIndex, foeSkillIndex) : (foeSkillIndex, mySkillIndex);


                if (mySkillIndex == -1)
                {
                    PlayTurnAction(foeCharacters[0], allyCharacters[0], foeWindow, allyWindow, foeSkillIndex);

                    if (firstAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(firstAttacker, firstAWindow);
                    if (secondAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(secondAttacker, secondAWindow);
                }
                else
                {
                    if (firstAttacker.BattleStats.Hp > 0)
                        PlayTurnAction(firstAttacker, secondAttacker, firstAWindow, secondAWindow, firstASkillIndex);
                    if (secondAttacker.BattleStats.Hp > 0)
                        PlayTurnAction(secondAttacker, firstAttacker, secondAWindow, firstAWindow, secondASkillIndex);

                    if (firstAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(firstAttacker, firstAWindow);
                    if (secondAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(secondAttacker, secondAWindow);
                }

                if (allyCharacters[0].BattleStats.Hp <= 0)
                {
                    if (allyCharacters[1].BattleStats.Hp <= 0 &&
                        allyCharacters[2].BattleStats.Hp <= 0)
                    {
                        winTheBattle = false;
                        battleEnd = true;
                    }
                    else
                    {
                        Replace(false);
                        setNewAlly = true;
                        SelectableUI.HideKeyGuide();
                    }
                }

                if (foeCharacters[0].BattleStats.Hp <= 0)
                {
                    if (!SwapBattleOrder(ref foeCharacters[0], ref foeCharacters[1]) && !SwapBattleOrder(ref foeCharacters[0], ref foeCharacters[2]))
                    {
                        winTheBattle = true;
                        battleEnd = true;

                    }
                    else
                    {
                        setNewFoe = true;
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
                allyCharacters[i] = new InBattleCharacter(player.Characters[i], player.Characters[i].DefaultName);
                foeArray[i] = CharacterData.GetRandomCharacter();
                foeCharacters[i] = new InBattleCharacter(foeArray[i], $"상대 {foeArray[i].DefaultName}");
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
            allyWindow.RewriteWindowContents(
                new string[]
                {
                    $"",
					$"{allyCharacters[0].BattleName}",
					$"{allyCharacters[0].GetTypeString()}",
                    $"HP:{allyCharacters[0].BattleStats.Hp}/{allyCharacters[0].DefaultStats.Hp}",
                    $"",
					$"물리공격:{allyCharacters[0].DefaultStats.Attack}",
					$"물리방어:{allyCharacters[0].DefaultStats.Defense}",
					$"특수공격:{allyCharacters[0].DefaultStats.SpAttack}",
					$"특수방어:{allyCharacters[0].DefaultStats.SpDefense}",
					$"스피드:{allyCharacters[0].DefaultStats.Speed}",
                    $"",
					$"",
					$"{allyCharacters[0].GetConditionString()}"
                }, true);
        }

        private void FoeThrowBall()
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea($"상대는 {foeCharacters[0].DefaultName}을(를) 내보냈다.", 1, true);
            Console.ReadKey(true);
            foeWindow.RewriteWindowContents(
                new string[]
                {
                    $"",
                    $"{foeCharacters[0].BattleName}",
                    $"{foeCharacters[0].GetTypeString()}",
                    $"HP:{foeCharacters[0].BattleStats.Hp}/{foeCharacters[0].DefaultStats.Hp}",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"",
                    $"{foeCharacters[0].GetConditionString()}"
                }, true);
        }

        private int SelectAction()
        {
            int mySelecion = -1;
            bool isValid = false;

            UIPreset.ClearAllScript();

            List<UI> fightOrReplace = new List<UI>()
            {
                new Window(Screen.WIDTH / 2 - 13, UIPreset.WINDOW_Y + 3, 7, 4, ' ', '=', new string[1] {"싸운다"}, true),
                new Window(Screen.WIDTH / 2 + 6, UIPreset.WINDOW_Y + 3, 8, 4, ' ', '=', new string[1] {"교체한다"}, true)
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
                new string[2] {$"{allyCharacters[0].Skills[i].Name} {allyCharacters[0].Skills[i].SkillType} 〈{allyCharacters[0].Skills[i].Category}〉", 
                    $"위력{allyCharacters[0].Skills[i].Power} 명중{allyCharacters[0].Skills[i].HitRate}"}, true));
			}

            SelectableUI selectableUI = new SelectableUI( skills, CursorMoveMode.Square, 2, 2);

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
                    $"HP:{allyCharacters[i].BattleStats.Hp}/{allyCharacters[i].DefaultStats.Hp}",
                    $"",
                    $"{allyCharacters[i].GetConditionString()}"
				}, true));
			}

			SelectableUI selectableUI = new SelectableUI( characters, CursorMoveMode.Horizonal);

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

        private bool GetAttackerOrder(InBattleCharacter character1, InBattleCharacter character2)
        {
            if (character1.BattleStats.Speed == character2.BattleStats.Speed)
            {
                if (new Random().Next(0, 99 + 1) < 50)
                    return true;
                else
                    return false;
            }
            else if (character1.BattleStats.Speed > character2.BattleStats.Speed)
                return true;
            else
                return false;
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

        private void PlayTurnAction(InBattleCharacter attacker, InBattleCharacter defender, 
            Window attackerWindow, Window defenderWindow, int skillIndex)
        {
            bool skipAction = false;
            bool miss = false;
            bool isCritical = false;

            if (attacker.Conditon == StatusCondition.마비)
            {
                if (new Random().Next(0, 99 + 1) < 50)
                {
                    skipAction = true;
                    attacker.GetConditionActionMessage();
                }
            }
            else if (attacker.Conditon == StatusCondition.얼음)
            {
                if (new Random().Next(0, 99 + 1) < 90)
                {
                    skipAction = true;
                    attacker.GetConditionActionMessage();
                }
                else
                {
                    //얼음 해제
                    UIPreset.CreateScriptTextArea("얼음이 풀렸다!", 1, true);
                    Console.ReadKey(true);
                }
            }

            if (defender.BattleStats.Hp <= 0 || new Random().Next(0, 99 + 1) >= attacker.Skills[skillIndex].HitRate)
            {
                skipAction = true;
                UIPreset.CreateScriptTextArea($"{attacker.GetSkillMessage(skillIndex)}", 1, true);
                Console.ReadKey(true);
                UIPreset.CreateScriptTextArea("그러나 빗나갔다.", 1, true);
                Console.ReadKey(true);
            }

            if (!skipAction)
            {
                UIPreset.CreateScriptTextArea($"{attacker.GetSkillMessage(skillIndex)}", 1, true);
                Console.ReadKey(true);

                if (attacker.Skills[skillIndex].Category != SkillCategory.변화)
                {
                    float mainTypeBonus = 1f;
                    if (attacker.FirstType == attacker.Skills[skillIndex].SkillType ||
                    attacker.SecondType == attacker.Skills[skillIndex].SkillType)
                    {
                        mainTypeBonus = 1.5f;
                    }
                    int typeEffectiveness = TypeTable.table[(int)attacker.Skills[skillIndex].SkillType, (int)defender.FirstType] *
                    TypeTable.table[(int)attacker.Skills[skillIndex].SkillType, (int)defender.SecondType];

                    float criticalDamage = 1f;
                    if (new Random().Next(0, 16) == 0)
                    {
                        criticalDamage = 1.5f;
                        isCritical = true;
                    }

                    int randomValue = new Random().Next(217, 255 + 1) * 100;

                    int damage = (attacker.Skills[skillIndex].Category == SkillCategory.물리) ?
                    (int)(((12 * attacker.Skills[skillIndex].Power * (float)attacker.BattleStats.Attack / 50) / defender.BattleStats.Defense + 2)
                     * criticalDamage * ((float)randomValue / 255 / 100) * mainTypeBonus * ((float)typeEffectiveness / 4)) :
                    (int)(((12 * attacker.Skills[skillIndex].Power * (float)attacker.BattleStats.SpAttack / 50) / defender.BattleStats.SpDefense + 2)
                    * criticalDamage * ((float)randomValue / 255 / 100) * mainTypeBonus * ((float)typeEffectiveness / 4));

                    if (damage < 1 && typeEffectiveness != (int)TypeTable.TypeEffectiveness.없음)
                        damage = 1;
                    defender.Damaged(damage, defenderWindow, HP_TEXT_LINE);


                    //UIPreset.CreateScriptTextArea(($"[{attacker.BattleName}] <{attacker.Skills[skillIndex].Name}> " +
                    //    $"{{{attacker.Skills[skillIndex].SkillType}}} 데미지: {damage}, 자속: {mainTypeBonus}, 크리: ({criticalDamage}, {isCritical}), " +
                    //    $"위력: {attacker.Skills[skillIndex].Power}, 명중률: {attacker.Skills[skillIndex].HitRate}, 상성: {typeEffectiveness}, " +
                    //    $"랜덤값: {randomValue}"), 5, false);

                    switch (typeEffectiveness)
                    {
                        case (int)TypeTable.TypeEffectiveness.없음:
                            UIPreset.CreateScriptTextArea("효과가 없는 것 같다...", 1, true);
                            break;
                        case (int)TypeTable.TypeEffectiveness.별로 * (int)TypeTable.TypeEffectiveness.별로:
                        case (int)TypeTable.TypeEffectiveness.별로 * (int)TypeTable.TypeEffectiveness.보통:
                            UIPreset.CreateScriptTextArea("효과가 별로인 것 같다...", 1, true);
                            break;
                        case (int)TypeTable.TypeEffectiveness.보통 * (int)TypeTable.TypeEffectiveness.보통:
                            //do nothing
                            break;
                        default:
                            UIPreset.CreateScriptTextArea("효과가 굉장했다!", 1, true);
                            break;
                    }
                    Console.ReadKey(true);

                    if (isCritical)
                    {
                        UIPreset.CreateScriptTextArea("급소에 맞았다!", 1, true);
                        Console.ReadKey(true);
                    }

                    defender.CheckStuned(defenderWindow);
                }

                SkillData.GetSkillEffect(attacker.Skills[skillIndex].Id);
            }
        }

        private void PlayTurnEndAction(InBattleCharacter character, Window characterWindow)
        {
			if (character.Conditon == StatusCondition.독)
			{
                int maxHp = character.DefaultStats.Hp;
                int damage = maxHp / 8;
                if (damage < 1)
                    damage = 1;
                character.Damaged(damage, characterWindow, HP_TEXT_LINE);
                character.GetConditionActionMessage();
                Console.ReadKey(true);
            }
			else if (character.Conditon == StatusCondition.화상)
			{
				int maxHp = character.DefaultStats.Hp;
				int damage = maxHp / 8;
				if (damage < 1)
					damage = 1;
				character.Damaged(damage, characterWindow, HP_TEXT_LINE);
				character.GetConditionActionMessage();
                Console.ReadKey(true);
            }

            character.CheckStuned(characterWindow);
        }
    }
}
