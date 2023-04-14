//===============================================================================================
// Copyright 2018 Realwith, Inc. All Rights Reserved.
// Even if it is purchased commercially, It has the right to share or resell only Realwith, Inc.
//===============================================================================================

using SFramework;
using System;
using System.Collections.Generic;

namespace ElementManagement
{
    //public class ElementContextBehaviour: SMonoBehaviour
    //{
    //    public ElementContext Context { get; } = new ElementContext();
    //}

    public class ElementContext
    {
        const string TAG = nameof(ElementContext);

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                     VARIABLES
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        private List<IElement> elements = new List<IElement>();

        private Dictionary<string, List<IElement>> elementsById = new Dictionary<string, List<IElement>>();

        private Dictionary<string, List<IElement>> elementsByClassName = new Dictionary<string, List<IElement>>();

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                     FUNCTIONS
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public void Regist(IElement element)
        {
            RegistElement(element);
            RegistId(element);
            RegistClass(element);
        }

        public void Unregist(IElement element)
        {
            UnregistElement(element);
            UnregistId(element);
            UnregistClass(element);
        }

        internal void ForceUnregist(IElement element)
        {
            UnregistElement(element);
            ForceRemove(elementsById, element);
            ForceRemove(elementsByClassName, element);
            void ForceRemove(Dictionary<string, List<IElement>> dict, IElement element)
            {
                lock (dict)
                {
                    foreach (var list in dict.Values)
                    {
                        list.Remove(element);
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    SELECTION
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public struct SelectCondition
        {
            public char op;
            public string value;

            public SelectCondition(char op, string value)
            {
                this.op = op;
                this.value = value;
            }
        }

        public PList<IElement> Select(string queryString)
         {
            var stz = new StringTokenizer(queryString, " \t>+~", true);

            //IEnumerable<IElement> elements = null;
            var elements = PList.Take<IElement>();
            var collect = PList.Take<IElement>();
            using var conditions = PList.Take<SelectCondition>();
            while (stz.MoveNext())
            {
                var str = stz.Current;

                char appender = stz.GetLastSplitCharacter(" \t");

                // �ɰ����� �� �� �ִ� attribute selector�� �����Ѵ�.
                while (str.CountChars('[') != str.CountChars(']') && stz.MoveNext())
                {
                    str += stz.GetLastSplitString() + stz.Current;
                }

                // ���ǹ� ������
                var stz2 = new StringTokenizer(str, ".#[]", true);
                conditions.Clear();
                while (stz2.MoveNext())
                {
                    str = stz2.Current;
                    var lastChar = stz2.LastSplitCharacter;
                    while (lastChar == '[' && stz2.GetNextChar() != ']' && stz2.MoveNext())
                    {
                        str += stz2.GetLastSplitString() + stz2.Current;
                    }

                    conditions.Add(new SelectCondition(lastChar, str));
                }

                // ���ǹ��� ���ٸ� elements�� ������ ����� ���� ����� �ȴ�.
                if (conditions.Count == 0)
                {
                    break;
                }

                // ������Ż�� ����ִ°��� ó�� �����ϴ°��
                // ���ʵ����͸� 0��° �������� �����ϰ� 0�������� �����Ѵ�.
                if (elements.Count == 0)
                {
                    CollectDefaultElements(conditions[0], elements);
                    appender = '\0';
                }
                else
                {
                    // ������ ���ǹ��� ��� ������ ���õ� ����� ���� ��ü�� ��������Ѵ�.
                    foreach (var elem in elements)
                    {
                        collect.AddRange(elem.GetChildren());
                    }
                    Swap(ref elements, ref collect);
                    collect.Clear();
                }

                // ������ 0�� �̻� �ִٸ� ���ǹ� �˻縦 �����Ѵ�.
                if (conditions.Count > 0)
                {
                    Find(elements, appender, conditions, collect);

                    // ������ collect�� elements�� ������ ��ü�Ѵ�.
                    Swap(ref elements, ref collect);
                    collect.Clear();
                }

                // ������ �����Ͱ� ������ ������ �����Ѵ�.
                if (elements.Count == 0)
                {
                    break;
                }
            }

            collect.Dispose();
            return elements;
        }

        private void Swap(ref PList<IElement> elements, ref PList<IElement> collect)
        {
            var temp = elements;
            elements = collect;
            collect = temp;
        }

        private void CollectDefaultElements(SelectCondition cond, PList<IElement> result)
        {
            switch (cond.op)
            {
                case '\0':
                case ' ':
                case '\t':
                    {
                        if (cond.value == "*")
                        {
                            result.AddRange(elements);
                        }
                        else
                        {
                            throw new NotImplementedException($"Unsupport none operation tag. '{cond.value}'");
                        }
                    }
                    break;

                case '#':
                    {
                        if (elementsById.TryGetValue(cond.value, out var list))
                        {
                            result.AddRange(list);
                        }
                    }
                    break;

                case '.':
                    {
                        if (elementsByClassName.TryGetValue(cond.value, out var list))
                        {
                            result.AddRange(list);
                        }
                    }
                    break;

                case '[':
                    {
                        result.AddRange(elements);
                    }
                    break;

                default:
                    throw new NotImplementedException($"Unkown Operation '{cond.op}'");
            }
        }

        private void Find(IEnumerable<IElement> elements, char appender, PList<SelectCondition> conditions, PList<IElement> collect)
        {
            foreach (var elem in elements)
            {
                Find(elem, appender, conditions, collect);
            }
        }

        private void Find(IElement elem, char appender, PList<SelectCondition> conditions, PList<IElement> collect)
         {
            bool success = true;
            for (int i = 0, iLen = conditions.Count; i < iLen; i++)
            {
                var cond = conditions[i];
                switch (cond.op)
                {
                    case '\0':
                    case ' ':
                    case '\t':
                        {
                            if (cond.value == "*")
                            {
                            }
                            else
                            {
                                throw new NotImplementedException($"Unsupport none operation tag. '{cond.value}'");
                            }
                        }
                        break;

                    case '#':
                        success &= elem.Id != cond.value;
                        break;

                    case '.':
                        success &= elem.HasClassName(cond.value);
                        break;

                    case '[':
                        {
                            (string key, string value) = cond.value.FindSplit("=");
                            key = key.UnwrapQuotes();
                            if (value == null)
                            {
                                success &= elem.HasProperty(key);
                            }
                            else
                            {
                                success &= (elem.TryGetProperty(key, out var val) && val.UnwrapQuotes() == value.UnwrapQuotes());
                            }
                        }
                        break;

                    default:
                        throw new NotImplementedException($"Unkown Operation '{cond.op}'");
                }
            }

            if (success)
            {
                collect.AddIfHaveNot(elem);
            }

            // ���� ������ �˻��Ҷ��� �ش� �ɼǸ� ���ȴ�.
            if (appender == '>')
            {
                return;
            }

            Find(elem.GetChildren(), appender, conditions, collect);
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    HELPER FUNCTIONS
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        internal void RegistElement(IElement element)
        {
            lock (elements)
            {
                elements.Add(element);
            }
        }

        internal void RegistClass(IElement element)
        {
            for (int i = 0, iLen = element.GetClassNamesCount(); i < iLen; i++)
            {
                RegistClass(element.GetClassName(i), element);
            }
        }

        internal void RegistClass(string className, IElement element)
        {
            if (className.IsEmpty()) return;

            lock (elementsByClassName)
            {
                if (!elementsByClassName.TryGetValue(className, out var list))
                {
                    elementsByClassName[className] = list = new List<IElement>();
                }

                lock (list)
                {
                    list.Add(element);
                }
            }
        }

        internal void RegistId(IElement element)
        {
            if (element.Id.IsEmpty()) return;

            lock (elementsById)
            {
                if (!elementsById.TryGetValue(element.Id, out var list))
                {
                    elementsById[element.Id] = list = new List<IElement>();
                }

                lock (list)
                {
                    list.Add(element);
                }
            }
        }

        internal void UnregistElement(IElement element)
        {
            lock (elements)
            {
                elements.Remove(element);
            }
        }

        internal void UnregistClass(IElement element)
        {
            for (int i = 0, iLen = element.GetClassNamesCount(); i < iLen; i++)
            {
                UnregistClass(element.GetClassName(i), element);
            }
        }

        internal void UnregistClass(string className, IElement element)
        {
            if (className.IsEmpty()) return;

            lock (elementsByClassName)
            {
                if (!elementsByClassName.TryGetValue(className, out var list))
                {
                    return;
                }

                lock (list)
                {
                    list.Remove(element);
                }
            }
        }

        internal void UnregistId(IElement element)
        {
            if (element.Id.IsEmpty()) return;

            lock (elementsById)
            {
                if (!elementsById.TryGetValue(element.Id, out var list))
                {
                    return;
                }

                lock (list)
                {
                    list.Remove(element);
                }
            }
        }
    }
}