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
    public class Search_Dict_vs_List : TimeChecker
    {
        public class CLASS
        {
            public int id;
            public long id1;
            public long id2;
            public long id3;
            public long id4;
            public long id5;
            public long id6;
            public long id7;
            public long id8;
            public long id9;
            public long id10;
            public long id11;
            public long id12;
            public long id13;
            public long id14;
            public long id15;
            public long id16;
            public long id17;
            public long id18;
            public long id19;
            public long id20;
            public long id21;
            public long id22;
            public long id23;
            public long id24;
            public long id25;
            public long id26;
            public long id27;
            public long id28;
            public long id29;
            public long id30;
            public long id31;
        }

        public struct STRUCT
        {
            public int id;
            public long id1;
            public long id2;
            public long id3;
            public long id4;
            public long id5;
            public long id6;
            public long id7;
            public long id8;
            public long id9;
            public long id10;
            public long id11;
            public long id12;
            public long id13;
            public long id14;
            public long id15;
            public long id16;
            public long id17;
            public long id18;
            public long id19;
            public long id20;
            public long id21;
            public long id22;
            public long id23;
            public long id24;
            public long id25;
            public long id26;
            public long id27;
            public long id28;
            public long id29;
            public long id30;
            public long id31;
        }

        public Search_Dict_vs_List()
        {
            const int RANGE = 10;
            var dictC = new Dictionary<int, CLASS>[RANGE];
            var dictS = new Dictionary<int, STRUCT>[RANGE];
            var listC = new List<CLASS>[RANGE];
            var listS = new List<STRUCT>[RANGE];

            for (int i = 0; i < RANGE; i++)
            {
                int len = (i+1) * 5;
                listC[i] = new List<CLASS>(Enumerable.Range(0, len).Select(v => new CLASS { id = v }));
                dictC[i] = new Dictionary<int, CLASS>(listC[i].Select(v => new KeyValuePair<int, CLASS>(v.id, v)));
                listS[i] = new List<STRUCT>(Enumerable.Range(0, len).Select(v => new STRUCT { id = v }));
                dictS[i] = new Dictionary<int, STRUCT>(listS[i].Select(v => new KeyValuePair<int, STRUCT>(v.id, v)));
            }

            int cnt = 1000000;
            Random rnd = new Random();
            CLASS cls = null;
            STRUCT st = default;
            int request = 0;

            Console.WriteLine("Start");

            while (true)
            {
                ForceGC();

                for (int i = 0; i < RANGE; i++)
                {
                    int idx = rnd.Next(0, dictC[i].Count);

                    BeginSample("dictC(" + dictC[i].Count + ")");
                    for (int j = 0; j < cnt; j++)
                    {
                        cls = dictC[i][idx];
                        request += st.id;
                    }
                    EndSample();

                    //BeginSample("dictS(" + dictS[i].Count + ")");
                    //for (int j = 0; j < cnt; j++)
                    //{
                    //    st = dictS[i][idx];
                    //    request += st.id;
                    //}
                    //EndSample();

                    BeginSample("listC(" + listC[i].Count + ")");
                    for (int j = 0; j < cnt; j++)
                    {
                        var lst = listC[i];
                        for (int k = 0, len = lst.Count; k < len; k++)
                        {
                            if (lst[k].id == idx)
                            {
                                cls = lst[k];
                                break;
                            }
                        }
                        request += cls.id;
                    }
                    EndSample();

                    //BeginSample("listS(" + listS[i].Count + ")");
                    //for (int j = 0; j < cnt; j++)
                    //{
                    //    var lst = listS[i];
                    //    for (int k = 0, len = lst.Count; k < len; k++)
                    //    {
                    //        if (lst[k].id == idx)
                    //        {
                    //            st = lst[k];
                    //            break;
                    //        }
                    //    }
                    //    request += st.id;
                    //}
                    //EndSample();

                    if (i == 0)
                    {
                        BeginSample("Linq listC(" + listC[i].Count + ")");
                        for (int j = 0; j < cnt; j++)
                        {
                            cls = (from v in listC[i] where v.id == idx select v).FirstOrDefault();
                            request += cls.id;
                        }
                        EndSample();

                        //BeginSample("Linq listS(" + listS[i].Count + ")");
                        //for (int j = 0; j < cnt; j++)
                        //{
                        //    st = (from v in listS[i] where v.id == idx select v).FirstOrDefault();
                        //    request += st.id;
                        //}
                        //EndSample();
                    }
                }

                Console.WriteLine(request);
                PrintSamples();
            }
        }
    }
}
