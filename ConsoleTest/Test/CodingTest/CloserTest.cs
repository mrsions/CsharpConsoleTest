using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleTest
{
    public class CloserTest
    {
        static void proc(ref int answer, int left, int right)
        {
            if (left == 0 && right == 0)
            {   // 괄호를 모두 사용하면 경우의 수 1 늘리고 종료
                answer++;
                return;
            }

            if (right > left || left < 0 || right < 0)  // 괄호의 갯수 제한이 넘거나 닫힌 괄호가 열린 괄호보다 많으면 종료
                return;

            proc(ref answer, left - 1, right);
            proc(ref answer, left, right - 1);
        }

        public static void Run()
        {
            for (int n = 0; n < 10; n++)
            {
                int answer = 0;
                int left = n;
                int right = n;
                proc(ref answer, left, right);

                Console.WriteLine(n + " / " + answer);
            }
        }
    }
}
