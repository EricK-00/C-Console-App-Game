using System;
using System.Collections.Generic;
using System.Linq;
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
        int battleTurn = 1;

        Character[] characters;
        InBattleCharacter[] allyCharacters;
        InBattleCharacter[] foeCharacters;

        public void PlayBattle()
        {
            SetUp();

            ThrowBall_Foe(0);
            ThrowBall(0);

            Selection1();
            int foeNum = FoeSelection();

            if (CompareSpeed(allyCharacters[0], foeCharacters[0]))
            {

            }
            else
            {

            }

            ++battleTurn;
        }

        private void SetUp()
        {
            characters = new Character[Player.MAX_CHARACTER_COUNT];
            characters[0] = new Character("리자몽", new CharacterStats(105, 110, 80, 108));
            characters[1] = new Character("이상해꽃", new CharacterStats(100, 110, 80, 99));
            characters[2] = new Character("거북왕", new CharacterStats(99, 110, 80, 1));

            allyCharacters = new InBattleCharacter[3];
            allyCharacters[0] = new InBattleCharacter(characters[0]);
            allyCharacters[1] = new InBattleCharacter(characters[1]);
            allyCharacters[2] = new InBattleCharacter(characters[2]);

            foeCharacters = new InBattleCharacter[3];
            foeCharacters[0] = new InBattleCharacter(characters[2]);
            foeCharacters[1] = new InBattleCharacter(characters[1]);
            foeCharacters[2] = new InBattleCharacter(characters[0]);
        }

        private void ThrowBall(int index)
        {
            Console.WriteLine($"가랏, {allyCharacters[index].BaseCharacter.Name}!");
        }

        private void ThrowBall_Foe(int index)
        {
            Console.WriteLine($"상대는 {foeCharacters[index].BaseCharacter.Name}을(를) 내보냈다.");
        }

        private void Selection1()
        {
            Console.WriteLine("1. 전투, 2. 교체");

            switch(Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Selection2_1();
                    break;

                case ConsoleKey.D2:
                    Selection2_2();
                    break;
            }
        }

        private void Selection2_1()
        {
            Console.WriteLine("전투: 기술 1번, 기술 2번, 기술 3번, 기술 4번");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("1!");
                    break;

                case ConsoleKey.D2:
                    Console.WriteLine("2!");
                    break;

                case ConsoleKey.D3:
                    Console.WriteLine("3!");
                    break;

                case ConsoleKey.D4:
                    Console.WriteLine("4!");
                    break;

                case ConsoleKey.Q:
                    Selection1();
                    break;
            }
        }

        private void Selection2_2()
        {
            Console.WriteLine("교체: 2번, 3번");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D2:
                    ThrowBall(1);
                    break;

                case ConsoleKey.D3:
                    ThrowBall(2);
                    break;

                case ConsoleKey.Q:
                    Selection1();
                    break;
            }
        }

        private int FoeSelection()
        {
            return new Random().Next(0, 3 + 1);
        }

        private bool CompareSpeed(InBattleCharacter ally, InBattleCharacter foe)
        {
            if (ally.BattleStats.Speed == foe.BattleStats.Speed)
            {
                if (new Random().Next(0, 99 + 1) < 50)
                    return true;
                else
                    return false;
            }
            else if (ally.BattleStats.Speed > foe.BattleStats.Speed)
                return true;
            else
                return false;
        }
    }
}
