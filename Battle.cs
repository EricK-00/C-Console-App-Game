using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsoleAppGame
{
    internal class Battle
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

        int battleTurn = 0;

        InBattleCharacter[] allyCharacters;
        InBattleCharacter[] foeCharacters;

        bool win = false;
        bool battleEnd = false;
        bool setNewAlly = true;
        bool setNewFoe = true;

        public bool PlayBattle(Player player)
        {
            SetUpBattleCharacter(player);

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

                TextArea hpText = new TextArea(10, 0, Screen.WIDTH, 1, $"{allyCharacters[0].BattleName}:HP({allyCharacters[0].BattleStats.Hp}) / {foeCharacters[0].BattleName}:HP({foeCharacters[0].BattleStats.Hp})");

                int mySkillIndex = SelectAction();
                int foeSkillIndex = FoeSelectAction();

                hpText = new TextArea(10, 0, Screen.WIDTH, 1, $"{allyCharacters[0].BattleName}:HP({allyCharacters[0].BattleStats.Hp}) / {foeCharacters[0].BattleName}:HP({foeCharacters[0].BattleStats.Hp})");

                (InBattleCharacter firstAttacker, InBattleCharacter secondAttacker, int firstSkillIndex, int secondSkillIndex) = 
                    GetAttackerOrder(allyCharacters[0], foeCharacters[0], mySkillIndex, foeSkillIndex);

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
						PlayTurnAction(firstAttacker, secondAttacker, firstSkillIndex);
					if (secondAttacker.BattleStats.Hp > 0)
						PlayTurnAction(secondAttacker, firstAttacker, secondSkillIndex);

                    if (firstAttacker.BattleStats.Hp > 0)
                        PlayTurnEndAction(firstAttacker);
					if (secondAttacker.BattleStats.Hp > 0)
						PlayTurnEndAction(secondAttacker);
                }

				if (allyCharacters[0].BattleStats.Hp <= 0)
                {
                    if (allyCharacters[1].BattleStats.Hp <= 0 &&
                        allyCharacters[2].BattleStats.Hp <= 0)
                    {
                        win = false;
                        battleEnd = true;
                    }
                    else
                    {
                        UIPreset.CreateScriptTextArea("교체할 포켓몬을 선택하세요.", 1, true);
                        SelectNextCharacter();
                        setNewAlly = true;
                    }
                }

                if (foeCharacters[0].BattleStats.Hp <= 0)
                {
                    if (!SwapBattleOrder(ref foeCharacters[0], ref foeCharacters[1]) && !SwapBattleOrder(ref foeCharacters[0], ref foeCharacters[2]))
                    {
                        win = true;
                        battleEnd = true;

                    }
                    else
                    {
                        setNewFoe = true;
                    }
                }
            }
            BattleResult(win);

            return win;
        }

        private void BattleResult(bool win)
        {
            if (win)
            {
                UIPreset.CreateScriptTextArea("배틀에 승리했다!", 1, true);
                Console.ReadKey(true);
            }
            else
            {
                UIPreset.CreateScriptTextArea("눈앞이 깜깜해졌다...", 1, true);
                Console.ReadKey(true);
            }
		}

        private void SetUpBattleCharacter(Player player)
        {
			allyCharacters = new InBattleCharacter[CharacterData.BATTLE_CHARACTER_COUNT];
            Character[] foeArray = new Character[CharacterData.BATTLE_CHARACTER_COUNT];
            foeCharacters = new InBattleCharacter[CharacterData.BATTLE_CHARACTER_COUNT];

            for (int i = 0; i < CharacterData.BATTLE_CHARACTER_COUNT; i++)
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
        }

        private void ThrowBall()
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea($"가랏, {allyCharacters[0].BattleName}!", 1, true);
            Console.ReadKey(true);
        }

        private void FoeThrowBall()
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea($"상대는 {foeCharacters[0].DefaultName}을(를) 내보냈다.", 1, true);
            Console.ReadKey(true);
        }

        private int SelectAction()
        {
            int mySelecion = -1;
            bool isValid = false;

            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea("1.싸운다 2.교체한다", 1, true);

            while (!isValid)
			{
				switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        mySelecion = SelectSkill();
                        if (mySelecion == -1)
                        {
                            UIPreset.ClearAllScript();
                            UIPreset.CreateScriptTextArea("1.싸운다 2.교체한다", 1, true);
                            continue;
                        }
                        else
                            isValid = true;
                        break;

                    case ConsoleKey.D2:
                        bool end = Replace();
                        if (!end)
                        {
                            UIPreset.ClearAllScript();
                            UIPreset.CreateScriptTextArea("1.싸운다 2.교체한다", 1, true);
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
            UIPreset.CreateScriptTextArea("기술을 선택하세요.", 1, true);
            UIPreset.CreateScriptTextArea($"{allyCharacters[0].Skills[0]?.Name} {allyCharacters[0].Skills[1]?.Name} " +
                $"{allyCharacters[0].Skills[2]?.Name} {allyCharacters[0].Skills[3]?.Name}", 3, false);

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        UIPreset.ClearAllScript();
                        return 0;

                    case ConsoleKey.D2:
                        UIPreset.ClearAllScript();
                        return 1;

                    case ConsoleKey.D3:
                        UIPreset.ClearAllScript();
                        return 2;

                    case ConsoleKey.D4:
                        UIPreset.ClearAllScript();
                        return 3;

                    case ConsoleKey.Q:
                        return -1;
                }
            }
        }

        private bool Replace()
        {
            UIPreset.CreateScriptTextArea("교체할 포켓몬을 선택하세요.", 1, true);
            UIPreset.CreateScriptTextArea($"{allyCharacters[0].DefaultName} {allyCharacters[0].BattleStats.Hp}/{allyCharacters[0].DefaultStats.Hp} | " +
                $"{allyCharacters[1].DefaultName} {allyCharacters[1].BattleStats.Hp}/{allyCharacters[1].DefaultStats.Hp} | " +
                $"{allyCharacters[2].DefaultName} {allyCharacters[2].BattleStats.Hp}/{allyCharacters[2].DefaultStats.Hp}", 3, false);
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D2:
                        if (!SwapBattleOrder(ref allyCharacters[0], ref allyCharacters[1]))
                        {
                            UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 2, true);
                            continue;
                        }

                        ReturnToBall(1);
                        ThrowBall();
                        return true;

                    case ConsoleKey.D3:
                        if (!SwapBattleOrder(ref allyCharacters[0], ref allyCharacters[2]))
                        {
                            UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 2, true);
                            continue;
                        }

                        ReturnToBall(2);
                        ThrowBall();
                        return true;

                    case ConsoleKey.Q:
                        return false;
                }
            }
        }

        private int FoeSelectAction()
        {
            return new Random().Next(0, 3 + 1);
        }

        private (InBattleCharacter, InBattleCharacter, int, int) GetAttackerOrder(InBattleCharacter character1, InBattleCharacter character2, 
            int skillIndex1, int skillIndex2)
        {
            if (character1.BattleStats.Speed == character2.BattleStats.Speed)
            {
                if (new Random().Next(0, 99 + 1) < 50)
                    return (character1, character2, skillIndex1, skillIndex2);
                else
                    return (character2, character1, skillIndex2, skillIndex1);
            }
            else if (character1.BattleStats.Speed > character2.BattleStats.Speed)
                return (character1, character2, skillIndex1, skillIndex2);
            else
                return (character2, character1, skillIndex2, skillIndex1);
        }

        private bool SelectNextCharacter()
        {
            UIPreset.ClearAllScript();
            UIPreset.CreateScriptTextArea("내보낼 포켓몬을 선택하세요.", 1, true);
            UIPreset.CreateScriptTextArea($"{allyCharacters[0].DefaultName} {allyCharacters[0].BattleStats.Hp}/{allyCharacters[0].DefaultStats.Hp} | " +
    $"{allyCharacters[1].DefaultName} {allyCharacters[1].BattleStats.Hp}/{allyCharacters[1].DefaultStats.Hp} | " +
    $"{allyCharacters[2].DefaultName} {allyCharacters[2].BattleStats.Hp}/{allyCharacters[2].DefaultStats.Hp}", 3, false);
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 2, true);
                        continue;
                    case ConsoleKey.D2:
                        if (!SwapBattleOrder(ref allyCharacters[0], ref allyCharacters[1]))
                        {
                            UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 2, true);
                            continue;
                        }
                        UIPreset.ClearAllScript();
                        return true;

                    case ConsoleKey.D3:
                        if (!SwapBattleOrder(ref allyCharacters[0], ref allyCharacters[2]))
                        {
                            UIPreset.CreateScriptTextArea("기절한 포켓몬은 내보낼 수 없습니다.", 2, true);
                            continue;
                        }
                        UIPreset.ClearAllScript();
                        return true;
                }
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

			if (attacker.Conditon == StatusCondition.마비)
            {
                if (new Random().Next(0, 99 + 1) < 50)
                {
					skipAction = true;
					attacker.GetStatusConditionMessage();
                }
			}
			else if (attacker.Conditon == StatusCondition.얼음)
            {
                if (new Random().Next(0, 99 + 1) < 90)
                {
					skipAction = true;
					attacker.GetStatusConditionMessage();
                }
                else
                {
                    //얼음 해제
                    UIPreset.CreateScriptTextArea("얼음이 풀렸다!", 1, true);
                    Console.ReadKey(true);
                }
			}

            if (!skipAction)
            {
                UIPreset.CreateScriptTextArea($"{attacker.GetSkillMessage(skillIndex)}", 1, true);
                Console.ReadKey(true);

                if (attacker.Skills[skillIndex].Category != SkillCategory.변화)
                {
                    if (defender.BattleStats.Hp <= 0 || new Random().Next(0, 99 + 1) >= attacker.Skills[skillIndex].HitRate)
                    {
                        UIPreset.CreateScriptTextArea("그러나 빗나갔다.", 1, true);
                        Console.ReadKey(true);
                    }

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
                    defender.Damaged(damage);

                    
                    //UIPreset.CreateScriptTextArea(($"[{attacker.BattleName}] <{attacker.Skills[skillIndex].Name}> " +
                    //    $"{{{attacker.Skills[skillIndex].SkillType}}} 데미지: {damage}, 자속: {mainTypeBonus}, 크리: ({criticalDamage}, {isCritical}), " +
                    //    $"위력: {attacker.Skills[skillIndex].Power}, 명중률: {attacker.Skills[skillIndex].HitRate}, 상성: {typeEffectiveness}, " +
                    //    $"랜덤값: {randomValue}"), 5, false);

                    switch (typeEffectiveness)
                    {
                        case (int)TypeTable.TypeEffectiveness.없음:
                            UIPreset.CreateScriptTextArea("효과가 없었다...", 1, true);
                            break;
                        case (int)TypeTable.TypeEffectiveness.별로 * (int)TypeTable.TypeEffectiveness.별로:
                        case (int)TypeTable.TypeEffectiveness.별로 * (int)TypeTable.TypeEffectiveness.보통:
                            UIPreset.CreateScriptTextArea("효과가 별로였다.", 1, true);
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

                        defender.CheckStuned();
                }

                SkillData.GetSkillEffect(attacker.Skills[skillIndex].Id);
            }
		}

        private void PlayTurnEndAction(InBattleCharacter character)
        {
			if (character.Conditon == StatusCondition.독)
			{
                int maxHp = character.DefaultStats.Hp;
                character.Damaged(maxHp / 8);
                character.GetStatusConditionMessage();
                Console.ReadKey(true);
            }
			else if (character.Conditon == StatusCondition.화상)
			{
				int maxHp = character.DefaultStats.Hp;
				character.Damaged(maxHp / 8);
				character.GetStatusConditionMessage();
                Console.ReadKey(true);
            }

            character.CheckStuned();
        }
    }
}
