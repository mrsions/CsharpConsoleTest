using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleTest
{
    public class ParentEntitySortSpeed : TimeChecker
    {
        public class Entity
        {
            public int Id;
            public int ParentId;
        }

        public ParentEntitySortSpeed()
        {
            while (true)
            {
                const int SIZE = 10; // 배열의 길이를 10으로 고정
                Entity[] temp = new Entity[SIZE]; // 클래스 배열 생성 -> Entity 타입 temp 배열의 길이를 SIZE 만큼 생성 하였음.
                for (int i = 0; i < SIZE; i++) // SIZE 길이 만큼 반복
                {
                    temp[i] = new Entity(); // 생성된 클래스 배열 temp의 각각의 i번째 인덱스 요소에 Entity 클래스 초기화
                    temp[i].Id = i + 1; // Id 번호를 1 ~ 10까지 부여
                }

                int seed = Environment.TickCount;
                Console.WriteLine(seed);
                seed = 69995593;
                Random rnd = new Random(seed); // 랜덤 클래스 변수 rnd 생성 및 해당 시드 값(1)으로 초기화
                for (int i = 0; i < SIZE; i++) // SIZE만큼 반복
                {
                    if (rnd.NextDouble() < 0.7) // 랜덤 클래스 변수 rnd가 0.0 ~ 1.0의 값을 가질 때, 그 값이 0.7 이하이면 해당 조건문을 실행
                    {
                        temp[i].ParentId = temp[rnd.Next(0, temp.Length)].Id; // temp 배열의 ParentId i번째 인덱스에 1 <= ParentId < 11 사이의 값이 들어감
                    }
                }
                for (int i = 0; i < temp.Length; i++)
                {
                    int j = UnityEngine.Random.Range(0, temp.Length);
                    int k = UnityEngine.Random.Range(0, temp.Length);

                    Swap(temp, j, k);
                }

                Sort(temp);
                Validate(temp);
            }
        }

        private static void Print(Entity[] temp)
        {
            for (int i = 0; i < temp.Length; i++) // temp의 길이만큼 반복
            {
                Console.WriteLine(string.Format("ID: {0,3} / PID: {1,3}", temp[i].Id, temp[i].ParentId));
                // 첫번째 인자에 temp[i].Id를 사용하고 3칸 오른쪽으로 정렬 / 두번째 인자에 temp[i].ParentId를 사용하고 3칸 오른쪽 정렬
            }
        }

        private static void Sort(Entity[] temp)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                for (int j = i + 1; j < temp.Length; j++)
                {
                    if (temp[j].ParentId == 0)
                    {
                        Swap(temp, i, j);
                        for (int k = j + 1; k < temp.Length; k++)
                        {
                            Swap(temp, j, k);
                        }

                    }
                    else if (temp[j].Id == temp[i].ParentId)
                    {
                        Swap(temp, i, j);
                    }
                }
            }
        }

        public static void Swap(Entity[] tempArray, int i, int j)
        {
            Entity tempValue;
            tempValue = tempArray[i];
            tempArray[i] = tempArray[j];
            tempArray[j] = tempValue;
        }

        private static void Validate(Entity[] temp)
        {
            List<int> list = new List<int>();

            for (int i = 0; i < temp.Length; i++)
            {
                var t = temp[i];
                if (t.ParentId != 0 && !list.Contains(t.ParentId))
                {
                    Console.WriteLine("ERROR");
                    Print(temp);
                    while (true) ;
                }
                list.Add(t.Id);
            }
        }

    }
}
