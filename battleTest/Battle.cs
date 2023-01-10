using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace battleTest
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

        //[]
        //기술 인덱스 메서드
        //타입 상성 배열

        //attacker, defender
        int[,] typechart = new int[(int)Type.없음 + 1, (int)Type.없음 + 1];
        int battleTurn = 0;

        Character[] characters;
        InBattleCharacter[] allyCharacters;
        InBattleCharacter[] foeCharacters;

        bool win = false;
        bool battleEnd = false;
        bool setNewAlly = true;
        bool setNewFoe = true;

        public bool PlayBattle()
        {
            LoadData();

            SetUp();

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

                Console.WriteLine($"{allyCharacters[0].BattleName}:HP({allyCharacters[0].BattleStats.Hp}) / {foeCharacters[0].BattleName}:HP({foeCharacters[0].BattleStats.Hp})");

                int mySkillIndex = Selection1();
                int foeSkillIndex = FoeSelection();

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

				allyCharacters[0].AddTurnCount();
				foeCharacters[0].AddTurnCount();

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
                        Replace();
                        setNewAlly = true;
                    }
                }

                if (foeCharacters[0].BattleStats.Hp <= 0)
                {
                    if (foeCharacters[1].BattleStats.Hp > 0)
                    {
                        SwapBattleOrder(foeCharacters[0], foeCharacters[1]);
                        setNewFoe = true;
                    }
                    else if (foeCharacters[2].BattleStats.Hp > 0)
                    {
                        SwapBattleOrder(foeCharacters[0], foeCharacters[2]);
                        setNewFoe = true;
                    }
                    else
                    {
                        win = true;
                        battleEnd = true;
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
                Console.WriteLine("배틀에 승리하였습니다.");
            }
            else
            {
				Console.WriteLine("배틀에 패배하였습니다.");
			}
		}

        private void LoadData()
        {
            SkillData.Initialize(new List<Skill>
            {
                new Skill(0, "default", 0, 100, Type.없음),
				new Skill(1, "화염방사", 95, 100, Type.불꽃),
				new Skill(2, "불대문자", 120, 85, Type.불꽃),
				new Skill(3, "에어슬래시", 80, 100, Type.비행),
                new Skill(4, "몸통박치기", 40, 100, Type.노말),
				new Skill(5, "기가드레인", 40, 100, Type.풀),
				new Skill(6, "파도타기", 95, 100, Type.물),
				new Skill(7, "오물폭탄", 80, 100, Type.독)
			});
            CharacterData.Initialize(new List<Character>
            {
                new Character(0, "default", Type.없음, Type.없음, new CharacterStats(1,1,1,1), new Skill[1]{new Skill(0)}),
                new Character(1, "리자몽", Type.불꽃, Type.비행, new CharacterStats(100,100,100,100), new Skill[4]{new Skill(1), new Skill(2), new Skill(3), new Skill(4)}),
                new Character(2, "이상해꽃", Type.풀, Type.독, new CharacterStats(75,75,125,125), new Skill[4]{new Skill(4), new Skill(5), new Skill(7), new Skill(6)}),
                new Character(3, "거북왕", Type.물, Type.없음, new CharacterStats(111,99,88,55), new Skill[4]{new Skill(1), new Skill(4), new Skill(6), new Skill(7)}),
            }) ;
        }

        private void SetUp()
        {
            characters = new Character[Player.MAX_CHARACTER_COUNT];
            characters[0] = CharacterData.GetRandomCharacter();
            characters[1] = CharacterData.GetRandomCharacter();
			characters[2] = CharacterData.GetRandomCharacter();

            Character[] foeCharactersList = new Character[Player.MAX_CHARACTER_COUNT];
			foeCharactersList[0] = CharacterData.GetRandomCharacter();
			foeCharactersList[1] = CharacterData.GetRandomCharacter();
			foeCharactersList[2] = CharacterData.GetRandomCharacter();

			allyCharacters = new InBattleCharacter[3];
            allyCharacters[0] = new InBattleCharacter(characters[0], characters[0].DefaultName);
            allyCharacters[1] = new InBattleCharacter(characters[1], characters[1].DefaultName);
            allyCharacters[2] = new InBattleCharacter(characters[2], characters[2].DefaultName);

            foeCharacters = new InBattleCharacter[3];
            foeCharacters[0] = new InBattleCharacter(foeCharactersList[0], $"상대 {foeCharactersList[0].DefaultName}");
            foeCharacters[1] = new InBattleCharacter(foeCharactersList[1], $"상대 {foeCharactersList[1].DefaultName}");
            foeCharacters[2] = new InBattleCharacter(foeCharactersList[2], $"상대 {foeCharactersList[2].DefaultName}");
        }

        private void ReturnBall()
        {
            Console.WriteLine($"돌아와, {allyCharacters[0].BattleName}");
        }

        private void ThrowBall()
        {
            Console.WriteLine($"가랏, {allyCharacters[0].BattleName}!");
        }

        private void FoeThrowBall()
        {
            Console.WriteLine($"상대는 {foeCharacters[0].DefaultName}을(를) 내보냈다.");
        }

        private int Selection1()
        {
            int mySelecion = -1;
            bool isValid = false;

		    Console.WriteLine("1. 전투, 2. 교체");

			while (!isValid)
			{
				switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        mySelecion = SelectSkill();
                        if (mySelecion == -1)
                        {
							Console.WriteLine("1. 전투, 2. 교체");
							continue;
                        }
                        else
                            isValid = true;
                        break;

                    case ConsoleKey.D2:
                        bool end = Replace();
                        if (!end)
                        {
							Console.WriteLine("1. 전투, 2. 교체");
							continue;
                        }
                        else
                            isValid = true;
                        break;
                }

                Console.WriteLine("-------");
            }

			return mySelecion;
		}

        private int SelectSkill()
        {
            Console.WriteLine("전투: 기술 1번, 기술 2번, 기술 3번, 기술 4번");

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("1!");
                        return 0;

                    case ConsoleKey.D2:
                        Console.WriteLine("2!");
                        return 1;

                    case ConsoleKey.D3:
                        Console.WriteLine("3!");
                        return 2;

                    case ConsoleKey.D4:
                        Console.WriteLine("4!");
                        return 3;

                    case ConsoleKey.Q:
                        return -1;
                }
            }
        }

        private bool Replace()
        {
            Console.WriteLine("교체: 2번, 3번");
			while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D2:
                        ReturnBall();
                        SwapBattleOrder(allyCharacters[0], allyCharacters[1]);
                        ThrowBall();
                        return true;

                    case ConsoleKey.D3:
						ReturnBall();
						SwapBattleOrder(allyCharacters[0], allyCharacters[2]);
						ThrowBall();
                        return true;

                    case ConsoleKey.Q:
                        return false;
                }
            }
        }

        private int FoeSelection()
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

        private void SwapBattleOrder(InBattleCharacter mainCharacter, InBattleCharacter character)
        {
            InBattleCharacter temp = mainCharacter;
            mainCharacter = character;
            character = temp;
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
                    Console.WriteLine("해제");
                }
			}

            if (!skipAction)
            {
                attacker.GetSkillMessage(skillIndex);

                if (defender.BattleStats.Hp <= 0 || new Random().Next(0, 99 + 1) >= attacker.Skills[skillIndex].HitRate)
                {
                    Console.WriteLine("그러나 빗나갔다.");
                }

                float mainTypeBonus = 1f;
                if (attacker.FirstType == attacker.Skills[skillIndex].SkillType ||
                attacker.SecondType == attacker.Skills[skillIndex].SkillType)
                {
                    mainTypeBonus = 1.5f;
                }
                int typeEffectiveness = TypeTable.table[(int)attacker.Skills[skillIndex].SkillType, (int)defender.FirstType] *
                TypeTable.table[(int)attacker.Skills[skillIndex].SkillType, (int)defender.SecondType];

                int criticalRate = 10;
                float criticalDamage = 1f;
                if (new Random().Next(0, 99 + 1) < criticalRate)
                {
                    criticalDamage = 1.5f;
                    isCritical = true;
                }

                int damage = (int)((float)attacker.Skills[skillIndex].Power * mainTypeBonus * criticalDamage * (float)typeEffectiveness / 4);
                defender.Damaged(damage);

                switch (typeEffectiveness)
                {
                    case (int)TypeTable.TypeEffectiveness.없음:
                        Console.WriteLine("효과가 없었다...");
                        break;
                    case (int)TypeTable.TypeEffectiveness.별로:
						Console.WriteLine("효과가 별로였다.");
						break;
                    case (int)TypeTable.TypeEffectiveness.보통:
                        //do nothing
                        break;
                    default:
						Console.WriteLine("효과가 굉장했다!");
						break;
                }

                if (isCritical)
                    Console.WriteLine("급소에 맞았다.");

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
			}
			else if (character.Conditon == StatusCondition.화상)
			{
				int maxHp = character.DefaultStats.Hp;
				character.Damaged(maxHp / 8);
				character.GetStatusConditionMessage();
			}
		}
    }
}
