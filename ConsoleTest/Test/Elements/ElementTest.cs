//===============================================================================================
// Copyright 2018 Realwith, Inc. All Rights Reserved.
// Even if it is purchased commercially, It has the right to share or resell only Realwith, Inc.
//===============================================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace ElementManagement
{
    public class ElementTest
    {
        ElementContext context = new ElementContext();

        public void Run()
        {
            Uri uri3 = new Uri("addr://addressable/asdfas/asdf.png?query=query");
            XmlDocument xml = new XmlDocument();
            xml.Load("d:/c.xml");

            var elem = MakeNode(xml.DocumentElement);
            elem.Context = context;

            var list = context.Select("#form");
            list = context.Select("#form [type=text]");
            list = context.Select(".stx.gz.kz *");
            list = context.Select("[href=http://sions.kr/bbs/search.php?stx=%EC%A1%B0%ED%95%A9%EB%8F%84%EC%9A%B0%EB%AF%B8]");
        }

        private BaseElement MakeNode(XmlNode node)
        {
            var elem = new BaseElement();
            //elem.Id = TryGetAttr(node, "id");
            //foreach (var cls in TryGetAttr(node, "class").Split(" \t".ToCharArray()))
            //{
            //    elem.AddClassName(cls);
            //}
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    switch (attr.Name.ToLower())
                    {
                        case "id":
                            elem.Id = attr.Value;
                            break;

                        case "class":
                            foreach (var cls in attr.Value.Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            {
                                elem.AddClassName(cls);
                            }
                            break;

                        default:
                            elem.SetProperty(attr.Name, attr.Value);
                            break;
                    }
                }
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode is XmlText
                    || childNode is XmlAttribute) continue;

                var childElem = MakeNode(childNode);
                childElem.Parent = elem;
            }

            return elem;
        }

        //private string TryGetAttr(XmlNode node, string v)
        //{
        //    try
        //    {
        //        return node.Attributes[v].Value;
        //    }
        //    catch { }
        //    return "";
        //}
    }
}