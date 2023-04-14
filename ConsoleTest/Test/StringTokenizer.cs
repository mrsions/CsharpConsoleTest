using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;


namespace ConsoleTest
{
    public class StringTokenizerTest : TimeChecker
    {
        public StringTokenizerTest()
        {
            //string[] test = new string[]
            //{
            //    "abc aaa ddd fff gg",
            //    " abc aaa ddd fff gg",
            //    "abc aaa ddd fff gg ",
            //    "  abc   aaa    ddd  \t fff   gg ",
            //    "  abc   aaa\tddd   fff   gg\t\t",
            //    " \t abc   a aa\t  d  dd   fff   gg \t \t ",
            //    "\t \t abc   a aa\t  d  dd   fff   gg \t \t "
            //};

            List<string> v = new List<string>();

            v.AddRange(new List<string>());
            v.AddRange(null);

            var stz = new StringTokenizer("#id2.cl1.cl2 .cl3 >   .cl4[ cl4p1=cl4v1, cl4p2=cl4v2] #id2 ", " #.,[]=>", false);
            
            bool beginParam = false;
            foreach (var w in stz)
            {
                if (w.Length == 0)
                {
                    switch(stz.LastSplitCharacter)
                    {
                        case ' ':
                            break;
                        case '[':
                            break;
                        case ']':
                            break;
                    }
                    continue;
                }

                switch (stz.LastSplitCharacter)
                {
                    case '#':
                        Console.WriteLine("[FindId] " + w);
                        break;
                    case '.':
                        Console.WriteLine("[FindClass] " + w);
                        break;
                    case '[':
                        break;
                }



                Console.WriteLine($"'{stz.LastSplitCharacter}' = '{w}'");
            }

        }

        private bool SEquals(string[] lArray, string[] tArray)
        {
            if (lArray.Length != tArray.Length) return false;

            for (int i = 0; i < lArray.Length; i++)
            {
                if (lArray[i] != tArray[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    public struct StringTokenizer : IEnumerable<string>, IEnumerator<string>
    {
        public static StringTokenizer ReadLines(string sourceString, bool skipEmpty = false) => new StringTokenizer(sourceString, "\r\n", skipEmpty);
        public static StringTokenizer ReadSpaces(string sourceString, bool skipEmpty = false) => new StringTokenizer(sourceString, " \t\r\n", skipEmpty);
        public static StringTokenizer ReadSpace(string sourceString, bool skipEmpty = false) => new StringTokenizer(sourceString, " ", skipEmpty);

        private string sourceString;
        private string splitChars;
        private bool skipEmpty;
        private int index;
        private int befIndex;
        private int loop;
        private int _countCache;

        public char LastSplitCharacter => GetLastSplitCharacter(0);
        public char GetLastSplitCharacter(int idx)
        {
            if ((befIndex - idx) < 0)
            {
                return '\0';
            }
            else
            {
                return sourceString[(befIndex - idx)];
            }
        }
        public char GetLastSplitCharacter(string skip)
        {
            for (int i = 0; i < befIndex + 1; i++)
            {
                char c = GetLastSplitCharacter(-i);
                if (!skip.Contains(c))
                {
                    return c;
                }
            }
            return '\0';
        }

        public StringTokenizer(string sourceString, string splitChars, bool skipEmpty = true) : this()
        {
            this.sourceString = sourceString;
            this.splitChars = splitChars;
            this.skipEmpty = skipEmpty;
            this.befIndex = -1;
            this._countCache = -1;
        }

        // Implements IEnumerable<string>
        public string Current { get; private set; }
        public IEnumerator<string> GetEnumerator()
        {
            return this;
        }

        // Implements IEnumerable
        object IEnumerator.Current => Current;
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        // Implements IEnumerator<string>
        public void Dispose()
        {
            index = 0;
            loop = 0;
        }

        public void Reset()
        {
            index = 0;
            loop = 0;
        }

        public int GetSplitCount()
        {
            if (_countCache != -1)
            {
                return _countCache;
            }

            int count = 0;
            int tLen = sourceString.Length;
            int sLen = splitChars.Length;

            if (skipEmpty)
            {
                bool befSearch = true;
                for (int i = 0; i < tLen; i++)
                {
                    char c = sourceString[i];

                    // i번째 문자가 분리용 문자인지를 확인한다.
                    bool search = false;
                    for (int j = 0; j < sLen; j++)
                    {
                        if (c == splitChars[j])
                        {
                            search = true;
                            break;
                        }
                    }

                    if (befSearch && !search)
                    {
                        count++;
                    }
                    befSearch = search;
                }
            }
            else
            {
                count++;
                for (int i = 0; i < tLen; i++)
                {
                    char c = sourceString[i];

                    // i번째 문자가 분리용 문자인지를 확인한다.
                    bool search = false;
                    for (int j = 0; j < sLen; j++)
                    {
                        if (c == splitChars[j])
                        {
                            search = true;
                            break;
                        }
                    }

                    if (search)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public string[] ToSplitArray()
        {
            Reset();
            string[] arr = new string[GetSplitCount()];
            for (int i = 0; i < arr.Length && MoveNext(); i++)
            {
                arr[i] = Current;
            }
            return arr;
        }

        public bool MoveNext()
        {
            loop++;

            befIndex = index;

            int tLen = sourceString.Length;
            if (tLen == this.index)
            {
                Current = null;
                return false;
            }

            int sLen = splitChars.Length;

            if (!skipEmpty && loop == 1)
            {
            }
            else
            {
                for (; index < tLen; index++)
                {
                    char c = sourceString[index];

                    // i번째 문자가 분리용 문자인지를 확인한다.
                    bool search = false;
                    for (int i = 0; i < sLen; i++)
                    {
                        if (c == splitChars[i])
                        {
                            search = true;
                            break;
                        }
                    }
                    if (search)
                    {
                        if (!skipEmpty)
                        {
                            index++;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // 분리용 문자가 끝나는 지점까지 검색한다.
            int startIndex = index;
            for (; index < tLen; index++)
            {
                char c = sourceString[index];

                // i번째 문자가 분리용 문자인지를 확인한다.
                bool search = false;
                for (int i = 0; i < sLen; i++)
                {
                    if (c == splitChars[i])
                    {
                        search = true;
                        break;
                    }
                }
                if (search)
                {
                    break;
                }
            }

            int len = index - startIndex;
            if (len == 0)
            {
                if (skipEmpty && index == tLen) return false;

                Current = "";
            }
            else
            {
                Current = sourceString.Substring(startIndex, len);
            }
            return true;
        }

        public string Next()
        {
            if (!MoveNext())
            {
                return "";
            }
            return Current;
        }
        public string Next(string def)
        {
            if (!MoveNext())
            {
                return def;
            }
            return Current;
        }

        public float NextF()
        {
            return float.Parse(Next());
        }
        public float NextF(int def)
        {
            if (!MoveNext())
            {
                return def;
            }
            return float.Parse(Current);
        }

        public int NextD()
        {
            return int.Parse(Next());
        }
        public int NextD(int def)
        {
            if (!MoveNext())
            {
                return def;
            }
            return int.Parse(Current);
        }

    }

    public struct StringOperatorTokenizer : IEnumerable<string>, IEnumerator<string>
    {
        private string sourceString;
        private string splitChars;
        private string operatorChars;
        private bool skipEmpty;
        private int index;
        private int befIndex;
        private int loop;
        private int _countCache;

        public char LastSplitCharacter
        {
            get
            {
                if (befIndex == 0)
                {
                    return '\0';
                }
                else
                {
                    return sourceString[befIndex];
                }
            }
        }

        public StringOperatorTokenizer(string sourceString, string splitChars, string operatorChars, bool skipEmpty = true) : this()
        {
            this.sourceString = sourceString;
            this.operatorChars = operatorChars;
            this.splitChars = splitChars;
            this.skipEmpty = skipEmpty;
            this._countCache = -1;
        }

        // Implements IEnumerable<string>
        public string Current { get; private set; }
        public IEnumerator<string> GetEnumerator()
        {
            return this;
        }

        // Implements IEnumerable
        object IEnumerator.Current => Current;
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        // Implements IEnumerator<string>
        public void Dispose()
        {
            index = 0;
            loop = 0;
        }

        public void Reset()
        {
            index = 0;
            loop = 0;
        }

        public int GetSplitCount()
        {
            if (_countCache != -1)
            {
                return _countCache;
            }

            int count = 0;
            int tLen = sourceString.Length;
            int sLen = splitChars.Length;

            if (skipEmpty)
            {
                bool befSearch = true;
                for (int i = 0; i < tLen; i++)
                {
                    char c = sourceString[i];

                    // i번째 문자가 분리용 문자인지를 확인한다.
                    bool search = false;
                    for (int j = 0; j < sLen; j++)
                    {
                        if (c == splitChars[j])
                        {
                            search = true;
                            break;
                        }
                    }

                    if (befSearch && !search)
                    {
                        count++;
                    }
                    befSearch = search;
                }
            }
            else
            {
                count++;
                for (int i = 0; i < tLen; i++)
                {
                    char c = sourceString[i];

                    // i번째 문자가 분리용 문자인지를 확인한다.
                    bool search = false;
                    for (int j = 0; j < sLen; j++)
                    {
                        if (c == splitChars[j])
                        {
                            search = true;
                            break;
                        }
                    }

                    if (search)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public string[] ToSplitArray()
        {
            Reset();
            string[] arr = new string[GetSplitCount()];
            for (int i = 0; i < arr.Length && MoveNext(); i++)
            {
                arr[i] = Current;
            }
            return arr;
        }

        public bool MoveNext()
        {
            loop++;

            befIndex = index;

            int tLen = sourceString.Length;
            if (tLen == this.index)
            {
                Current = null;
                return false;
            }

            int sLen = splitChars.Length;

            if (!skipEmpty && loop == 1)
            {
            }
            else
            {
                for (; index < tLen; index++)
                {
                    char c = sourceString[index];

                    // i번째 문자가 분리용 문자인지를 확인한다.
                    bool search = false;
                    for (int i = 0; i < sLen; i++)
                    {
                        if (c == splitChars[i])
                        {
                            search = true;
                            break;
                        }
                    }
                    if (search)
                    {
                        if (!skipEmpty)
                        {
                            index++;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // 분리용 문자가 끝나는 지점까지 검색한다.
            int startIndex = index;
            for (; index < tLen; index++)
            {
                char c = sourceString[index];

                // i번째 문자가 분리용 문자인지를 확인한다.
                bool search = false;
                for (int i = 0; i < sLen; i++)
                {
                    if (c == splitChars[i])
                    {
                        search = true;
                        break;
                    }
                }
                if (search)
                {
                    break;
                }
            }

            int len = index - startIndex;
            if (len == 0)
            {
                if (skipEmpty && index == tLen) return false;

                Current = "";
            }
            else
            {
                Current = sourceString.Substring(startIndex, len);
            }
            return true;
        }

        public string Next()
        {
            if (!MoveNext())
            {
                return "";
            }
            return Current;
        }
        public string Next(string def)
        {
            if (!MoveNext())
            {
                return def;
            }
            return Current;
        }

        public float NextF()
        {
            return float.Parse(Next());
        }
        public float NextF(int def)
        {
            if (!MoveNext())
            {
                return def;
            }
            return float.Parse(Current);
        }

        public int NextD()
        {
            return int.Parse(Next());
        }
        public int NextD(int def)
        {
            if (!MoveNext())
            {
                return def;
            }
            return int.Parse(Current);
        }

    }

}
