using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleTest
{
    public class forSpeedTest : TimeChecker
    {
        public forSpeedTest()
        {
            const int SIZE = 1000;
            int[][] test = new int[SIZE][];
            int result = 0;
            for (int i = 0; i < test.Length; i++) test[i] = new int[SIZE];

            int cnt = 100;

            while (true)
            {
                Proc("Test", cnt, () =>
                 {
                     var array = test;
                     int temp = 0;
                     for (int i = 0; i < SIZE; i++)
                     {
                         for (int j = 0; j < SIZE; j++)
                         {
                             temp = array[i][j];
                         }
                     }
                     result = temp;
                 });

                Proc("Test2", cnt, () =>
                {
                    var array = test;
                    int temp = 0;
                    for (int j = 0; j < SIZE; j++)
                    {
                        for (int i = 0; i < SIZE; i++)
                        {
                            temp = array[i][j];
                        }
                    }
                    result = temp;
                });

                Proc("Test3", cnt, () =>
                {
                    var array = test;
                    int temp = 0;
                    for (int i = 0; i < SIZE; i++)
                    {
                        var array2 = array[i];
                        for (int j = 0; j < SIZE; j++)
                        {
                            temp = array2[j];
                        }
                    }
                    result = temp;
                });

                Proc("Test4", cnt, () =>
                {
                    var array = test;
                    int temp = 0;
                    Parallel.For(0, SIZE, i =>
                    {
                        for (int j = 0; j < SIZE; j++)
                        {
                            temp = array[i][j];
                        }
                    });
                    result = temp;
                });

                Proc("Test5", cnt, () =>
                {
                    var array = test;
                    int temp = 0;
                    Parallel.For(0, SIZE, i =>
                    {
                        var array2 = array[i];
                        for (int j = 0; j < SIZE; j++)
                        {
                            temp = array2[j];
                        }
                    });
                    result = temp;
                });

                PrintSamples();
            }
        }
    }
}
