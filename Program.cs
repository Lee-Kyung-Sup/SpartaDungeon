﻿using System;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace _5week_assignment
{
    internal class Program
    {
        static bool isBattle = false;

        private static Character _player = new Character();
        private static List<Monster> monsterPool = new List<Monster>();
        private static List<Item> playerInventory = new List<Item>();
        public static int currentStage;

        static void Main(string[] args)
        {
            PrintStartLogo();
            GameDataSetting();
            startMenu();

        }


        private static void GameDataSetting()
        {
            Console.Clear();
            currentStage = 1;           
            AddMonster();
            Console.WriteLine("스파르타 마을에 오신걸 환영합니다!");
            _player.CreatePlayer();
            
        }


        static void AddItem(Item item)  // 플레이어 아이템 획득
        {
            playerInventory.Add(item);
        }

        static void AddMonster()
        {
            Random rand = new Random();
            int summonCnt;


            if (currentStage <= 1)
            {
                summonCnt = rand.Next(1, 5);            // 1 ~ 5단계에선 1마리 ~ 4마리까지의 몬스터가 등장하게끔
                for (int i = 0; i < summonCnt; i++)
                {
                    monsterPool.Add(new Monster(currentStage));
                }
            }
            else if (currentStage <= 2)
            {
                summonCnt = rand.Next(2, 5);            // 6 ~ 10단계에서 1마리 ~ 5마리까지의 몬스터가 등장하게끔
                for (int i = 0; i < summonCnt; i++)
                {
                    monsterPool.Add(new Monster(currentStage));
                }
            }
            else if(currentStage <= 3)
            {
                summonCnt = rand.Next(2, 7);            // 7~ 단계에서 2마리 ~ 5마리의 몬스터가 등장하게끔
                for (int i = 0; i < summonCnt; i++)
                {
                    monsterPool.Add(new Monster(currentStage));
                }
            }
            else
            {
                summonCnt = rand.Next(3, 10);
                for(int i = 0; i<summonCnt; i++)
                {
                    monsterPool.Add(new Monster(currentStage));
                }
            }
           
        }
        
        private static void PrintStartLogo()
        {
            Console.WriteLine("====================================================================");
            Console.WriteLine("                                __                     \r\n");
            Console.WriteLine("  ____________ _____  _______ _/  |_ _____             \r\n");
            Console.WriteLine(" /  ___/\\____ \\\\__  \\ \\_  __ \\\\   __\\\\__  \\            \r\n");
            Console.WriteLine(" \\___ \\ |  |_> >/ __ \\_|  | \\/ |  |   / __ \\_          \r\n");
            Console.WriteLine("/____  >|   __/(____  /|__|    |__|  (____  /          \r\n");
            Console.WriteLine("     \\/ |__|        \\/                    \\/           \r\n");
            Console.WriteLine("                                                       \r\n");
            Console.WriteLine("    .___                 ____                          \r\n");
            Console.WriteLine("  __| _/__ __   ____    / ___\\   ____   ____    ____   \r\n");
            Console.WriteLine(" / __ ||  |  \\ /    \\  / /_/  >_/ __ \\ /  _ \\  /    \\  \r\n");
            Console.WriteLine("/ /_/ ||  |  /|   |  \\ \\___  / \\  ___/(  <_> )|   |  \\ \r\n");
            Console.WriteLine("\\____ ||____/ |___|  //_____/   \\___  >\\____/ |___|  / \r\n");
            Console.WriteLine("     \\/            \\/               \\/             \\/  \r\n");
            Console.WriteLine("                                                       ");
            Console.WriteLine("====================================================================");
            Console.WriteLine("                        Press AnyKey To Start                       ");
            Console.WriteLine("====================================================================");
            Console.ReadKey();

        }

        static void startMenu()
        {
            isBattle = false;

            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점 방문");
            Console.Write("4. 전투 시작");
            Console.WriteLine($" (현재 진행 : {currentStage}층)");
            Console.WriteLine();


            switch (CheckValidInput(1, 4))
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
                case 3:
                    merchantMenu();
                    break;
                case 4:
                    BattleStart();
                    break;
            }


        }

        private static void BattleStart()
        {
            Console.Clear();
            ShowHighlightedText("Battle!!");
            Console.WriteLine();
            Console.WriteLine($"Stage : {currentStage}");
            Console.WriteLine();
            for(int i = 0; i < monsterPool.Count; i++)
            {
                monsterPool[i].MonsterInfo();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[내정보]");
            _player.PlayerInfo();
            Console.WriteLine();

            Console.WriteLine("1. 공격");
            Console.WriteLine();

            Console.WriteLine("2. 인벤토리");
            Console.WriteLine();

            Console.WriteLine("0. 도망치기");
            Console.WriteLine();
            switch (CheckValidInput(0,2))
            {
                case 0:
                    startMenu(); // 도망치기
                    break;
                case 1:
                    // 공격
                    Attack();
                    break;
                case 2:
                    // 인벤토리
                    isBattle = true;
                    InventoryMenu();
                    break;
            }
        }

        private static void Attack()
        {
            Console.Clear();
            ShowHighlightedText("Battle!!");
            Console.WriteLine();

            for (int i = 0; i < monsterPool.Count; i++)
            {
                monsterPool[i].MonsterInfo(true,i+1);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[내정보]");
            _player.PlayerInfo();

            Console.WriteLine();
            Console.WriteLine("0. 취소");

            First:                                                      //goto :  First
            int input = CheckValidInput(0, monsterPool.Count);
            
            switch (input)
            {
                case 0:
                    Console.Clear();
                    BattleStart();
                    break;
                case 1:
                    if (monsterPool[input - 1].isDead)                  // 내가 고른 번호의 몬스터가 이미 죽은 몬스터라면?
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                        goto First;                                     // First로 돌아가 input값을 다시 받는다.
                    }
                    Console.Clear();
                    PlayerAttackResult(input - 1);
                    break;
                case 2:
                    Console.Clear();
                    if (monsterPool[input - 1].isDead)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                        goto First;
                    }
                    PlayerAttackResult(input - 1);
                    break;
                case 3:
                    Console.Clear();
                    if (monsterPool[input - 1].isDead)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                        goto First;
                    }
                    PlayerAttackResult(input - 1);
                    break;
                case 4:
                    Console.Clear();
                    if (monsterPool[input - 1].isDead)              
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine();
                        goto First;
                    }
                    PlayerAttackResult(input - 1);
                    break;
            }
        }

        private static void PlayerAttackResult(int input)
        {
            int damaged = 0;

            ShowHighlightedText("■ PlayerTurn ■");
            Console.WriteLine();

            _player.PlayerAttack(monsterPool[input],out damaged);
            Console.WriteLine($"{_player.Name} 의 공격!");

            if(damaged == 0)
            {
                Console.WriteLine($"{monsterPool[input].Name} 을 공격했지만 회피하였습니다.");
            }
            else
            {
                Console.Write($"Lv.{monsterPool[input].Level} {monsterPool[input].Name} 를 맞췄습니다.");
                Console.WriteLine($" [데미지 : {damaged}]");
            }
            
            if (monsterPool[input].currentHp <= 0)
            {
                monsterPool[input].isDead = true;
            }

            Console.WriteLine();
            

            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();

            int inputKey = CheckValidInput(0, 0);

            if (inputKey == 0)
            {
                Console.Clear();
                MonsterTurn();
            }

            
        }

        private static void MonsterTurn()
        {
            int deadCount = 0;

            ShowHighlightedText("■ MonsterTurn ■");
            Console.WriteLine();

            for (int i = 0; i < monsterPool.Count; i++)
            {
                int beforeHitHP = _player.currentHP;

                if (!monsterPool[i].isDead)
                {
                    monsterPool[i].MonsterAttack(_player,out int damaged);
                    Console.WriteLine($"Lv.{monsterPool[i].Level} {monsterPool[i].Name} 의 공격!");

                    if(damaged ==0)
                    {
                        Console.WriteLine($"{_player.Name} 을 공격했지만 회피하였습니다.");
                    }
                    else
                    {
                        Console.WriteLine($"{_player.Name} ({_player.Job})을(를) 맞췄습니다.  [데미지 : {damaged}]");
                    }
                    
                    Console.WriteLine();
                    Console.WriteLine($"Lv. {_player.Level} {_player.Name} {_player.Job}");
                    Console.WriteLine($"HP {beforeHitHP} -> {_player.currentHP}");


                    Console.WriteLine();
                    Console.WriteLine("0.다음");
                    Console.WriteLine();


                    
                    
                    if (_player.currentHP <= 0)
                    {
                        _player.currentHP = 0;
                        _player.isDead = true;

                        if(_player.isDead)
                        {
                            Console.Clear();

                            Console.Clear();
                            ShowHighlightedText("■ You Lose :( ■");

                            Console.WriteLine();
                            Console.WriteLine($"Lv.{_player.Level} {_player.Name}");
                            Console.WriteLine($"HP {_player.Hp} -> {_player.currentHP}");

                            Console.WriteLine();
                            Console.WriteLine("0. 다음");

                            int inputKey2 = CheckValidInput(0, 0);

                            if (inputKey2 == 0)
                            {
                                monsterPool.Clear();
                                GameDataSetting();
                                startMenu();
                            }
                        }
                    }

                    int inputKey = CheckValidInput(0, 0);

                    if (inputKey == 0) { continue; }
                }
                else
                {
                    deadCount++;
                    if(deadCount == monsterPool.Count)
                    {
                        Console.Clear();
                        ShowHighlightedText("■ Victory :) ■");

                        Console.WriteLine();
                        Console.WriteLine($"던전에서 몬스터 {deadCount}마리를 잡았습니다.");

                        Console.WriteLine();
                        Console.WriteLine($"Lv.{_player.Level} {_player.Name}");
                        Console.WriteLine($"HP {_player.Hp} -> {_player.currentHP}");

                        Console.WriteLine();
                        ShowHighlightedText("Stage Clear");
                        Console.WriteLine($"Stage : {currentStage} -> {currentStage + 1}");
                        currentStage++;

                        Console.WriteLine();
                        ShowHighlightedText("[ 전리품 획득 !! ]");

                        looting();

                        Console.WriteLine();
                        Console.WriteLine("0. 다음");

                        int inputKey = CheckValidInput(0, 0);

                        if (inputKey == 0)
                        {
                            monsterPool.Clear();
                            AddMonster();
                            startMenu();
                        }


                    }
                }
            }

            Attack();
        }

        private static void looting()
        {
            for (int i = 0; i < monsterPool.Count; i++)
            {
                Random rand = new Random();
                int randomGold = rand.Next(-50, 51);

                Monster lootingMonster = monsterPool[i];
                int gold = lootingMonster.Gold + randomGold;

                Console.WriteLine($"exp : {monsterPool[i].Exp}");
                Console.WriteLine($"{monsterPool[i].Gold}  Gold");
                if (monsterPool[i].monsterDropItem != null)
                {
                    Console.WriteLine($"{monsterPool[i].monsterDropItem.Name}");
                    AddItem(monsterPool[i].monsterDropItem);
                }
                
                Console.WriteLine();
                Console.WriteLine();

                _player.Exp += monsterPool[i].Exp;
                _player.Gold += gold;
                
            }

        }

        private static int CheckValidInput(int min, int max)
        {
            int keyInput;
            bool result;

            Console.WriteLine("원하시는 행동을 입력하세요");
            Console.Write(">>");

            do
            {
                result = int.TryParse(Console.ReadLine(), out keyInput);
                if (!result)
                {
                    Console.WriteLine("다시 입력하세요");
                    Console.Write(">>");
                    continue;
                }
            }
            while (result == false || CheckIfValid(keyInput, min, max) == false);

            return keyInput;

        }

        private static bool CheckIfValid(int keyInput, int min, int max)
        {
            if (min <= keyInput && keyInput <= max)
            {
                return true;
            }
            else
            {
                Console.WriteLine("다시 입력하세요");
                Console.Write(">>");
                return false;
            }
        }
        private static void ShowHighlightedText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void PrintTextWithHighlights(string s1, string s2, string s3 = "")
        {
            Console.Write(s1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(s2);
            Console.ResetColor();
            Console.WriteLine(s3);
        }

        private static void StatusMenu()
        {
            Console.Clear();

            ShowHighlightedText("상 태  보 기");
            Console.WriteLine("캐릭터의 정보가 표기됩니다.");

            PrintTextWithHighlights("Lv. ", _player.Level.ToString("00"));
            Console.WriteLine();
            Console.WriteLine("{0} ( {1} )", _player.Name, _player.Job);

            PrintTextWithHighlights("공격력 : ", $"{_player.Atk.ToString()}");
            PrintTextWithHighlights("방어력 : ", $"{_player.Def.ToString()}");
            PrintTextWithHighlights("체력 : ", $"{_player.currentHP.ToString()}");

            int[] bonusStat = getSumBonusStat();
            PrintTextWithHighlights("공격력 : ", (bonusStat[0]).ToString(), bonusStat[0] - _player.Atk > 0 ? string.Format(" (+{0})", bonusStat[0] - _player.Atk) : "");
            PrintTextWithHighlights("방어력 : ", (bonusStat[1]).ToString(), bonusStat[1] - _player.Def > 0 ? string.Format(" (+{0})", bonusStat[1] - _player.Def) : "");

            PrintTextWithHighlights("체력 : ", $"{_player.currentHP.ToString()} / {_player.Hp.ToString()}");


            PrintTextWithHighlights("골드 : ", _player.Gold.ToString());
            PrintTextWithHighlights("경험치 : ", $"{_player.Exp.ToString()}");
            Console.WriteLine();
            Console.WriteLine("0. 뒤로 가기");
            Console.WriteLine();

            

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    startMenu();
                    break;
            }
        }
        
        private static int[] getSumBonusStat()
        {
            int Atk = 0;
            int Def = 0;
            for (int i = 0; i < playerInventory.Count; i++)
            {
                if (playerInventory[i].isEquipped)
                {
                    Atk += playerInventory[i].Atk;
                    Def += playerInventory[i].Def;
                }
            }

            int[] bonusStat = { _player.Atk + Atk, _player.Def + Def };

            return bonusStat;
        }


        private static void InventoryMenu()
        {
            Console.Clear();

            ShowHighlightedText("인 벤  토 리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < playerInventory.Count; i++)
            {
                playerInventory[i].PrintItemStatDescription(true, i + 1);
            }

            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("");

            switch (CheckValidInput(0, 1))
            {
                case 0:
                    if(isBattle == false)
                    {
                        startMenu();
                    }
                    else
                    {
                        BattleStart();
                    }
                    break;
                case 1:
                    EquipMenu();
                    break;
            }
        }

        private static void EquipMenu()
        {
            Console.Clear();

            ShowHighlightedText("인 벤  토 리 - 장 착  관 리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < playerInventory.Count; i++)
            {
                playerInventory[i].PrintItemStatDescription(true, i + 1);
            }

            Console.WriteLine("");
            Console.WriteLine("0. 나가기");

            int keyInput = CheckValidInput(0, playerInventory.Count);

            switch (keyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    ToggleEquipStatus(keyInput - 1);
                    EquipMenu();
                    break;
            }
        }

        private static void ToggleEquipStatus(int idx)
        {
            for (int i = 0; i < playerInventory.Count; i++)
            {
                if (playerInventory[i].isEquipped == true)
                {
                    if (playerInventory[i].Type == playerInventory[idx].Type && i != idx)
                    {
                        playerInventory[i].isEquipped = !playerInventory[i].isEquipped;
                    }
                }
            }

            if (playerInventory[idx].Type == 2)
            {
                _player.currentHP += playerInventory[idx].Hp;

                if (_player.currentHP > _player.Hp)
                {
                    _player.currentHP = _player.Hp;
                }

                playerInventory.RemoveAt(idx);

                return;
            }

            if (playerInventory[idx] == null)
            {
                return;
            }

            playerInventory[idx].isEquipped = !playerInventory[idx].isEquipped;

        }

        private static void merchantMenu()
        {
            throw new NotImplementedException();
        }



    }
}
