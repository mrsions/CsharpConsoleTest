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
    public class TaskVsLambdaSpeedTest : TimeChecker
    {
        public TaskVsLambdaSpeedTest()
        {
            int cnt = 100;

            while (true)
            {
                

                PrintSamples();
            }
        }

        public class Client
        {
            public int id;
            public string name;
            public string tag;
            public float pos_x;
            public float pos_y;
            public float pos_z;
            public float rot_x;
            public float rot_y;
            public float rot_z;
            public float scale_x;
            public float scale_y;
            public float scale_z;
        }
        public class TaskClient : Client
        {
            public async Task OnHandle(BinaryReader reader)
            {
                int op = reader.ReadInt32();
                switch (op)
                {
                    default:
                        await Action(op, reader);
                        break;
                }
            }

            private async Task Action(int op, BinaryReader reader)
            {
                var vv = reader.ReadString();
                await Task.Yield();
            }
        }
    }
}
