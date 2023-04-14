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
    public class XmlTest : TimeChecker
    {
        public XmlTest()
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.NodeChanged += XDoc_NodeChanged;
            xDoc.NodeChanging += XDoc_NodeChanging;
            xDoc.NodeInserted += XDoc_NodeInserted;
            xDoc.NodeInserting += XDoc_NodeInserting;
            xDoc.NodeRemoved += XDoc_NodeRemoved;
            xDoc.NodeRemoving += XDoc_NodeRemoving;
            
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/a.xml"));
            Console.Clear();

            ((XmlElement)xDoc.DocumentElement.FirstChild).SetAttribute("test", "abc");
            ((XmlElement)xDoc.DocumentElement.FirstChild).SetAttribute("test", "ddddd");

            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/b.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/a.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/b.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/a.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/b.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/a.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/b.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/a.xml"));
            Console.Clear();
            xDoc.LoadXml(File.ReadAllText("d:/b.xml"));

            PerformanceTest(xDoc);
        }

        private void XDoc_NodeRemoving(object sender, XmlNodeChangedEventArgs e)
        {
            Console.WriteLine("XDoc_NodeRemoving()" + Print(e));
        }

        private void XDoc_NodeRemoved(object sender, XmlNodeChangedEventArgs e)
        {
            Console.WriteLine("XDoc_NodeRemoved()" + Print(e));
        }

        private void XDoc_NodeInserting(object sender, XmlNodeChangedEventArgs e)
        {
            Console.WriteLine("XDoc_NodeInserting()" + Print(e));
        }

        private void XDoc_NodeInserted(object sender, XmlNodeChangedEventArgs e)
        {
            Console.WriteLine("XDoc_NodeInserted()" + Print(e));
        }

        private void XDoc_NodeChanging(object sender, XmlNodeChangedEventArgs e)
        {
            Console.WriteLine("XDoc_NodeChanging()" + Print(e));
        }

        private void XDoc_NodeChanged(object sender, XmlNodeChangedEventArgs e)
        {
            Console.WriteLine("XDoc_NodeChanged()" + Print(e));
        }

        private string Print(XmlNodeChangedEventArgs e)
        {
            return $"@{e.Action}@{Print(e.Node)}@{Print(e.OldParent)}@{Print(e.NewParent)}@{e.OldValue}@{e.NewValue}";
        }

        private string Print(XmlNode node)
        {
            if (node == null) return "null";

            return $"{Path(node)}({node.GetType().Name})";
        }

        private string Path(XmlNode node)
        {
            string rst = node.Name;
            while((node = node.ParentNode) != null)
            {
                rst = node.Name + "/" + rst;
            }
            return rst;
        }

        private void PerformanceTest(XmlDocument xDoc)
        {
            int cnt = 100000;
            var sb = new StringBuilder(100 * 1024 * 1024);
            var startElem = xDoc.DocumentElement.FirstChild;

            while (true)
            {

                BeginSample("ChildNodes");
                for (int i = 0; i < cnt; i++)
                {
                    IDX = 0;
                    SearchChildNodes(startElem, sb);
                }
                EndSample();

                BeginSample("SearchLink");
                for (int i = 0; i < cnt; i++)
                {
                    IDX = 0;
                    SearchLink(startElem, sb);
                }
                EndSample();

                //Proc("ChildNodes", cnt, () =>
                //{
                //    IDX = 0;
                //    SearchChildNodes(startElem, sb);
                //});
                //Proc("SearchLink", cnt, () =>
                //{
                //    IDX = 0;
                //    SearchChildNodes(startElem, sb);
                //});

                PrintSamples();
            }
        }

        static int IDX = 0;

        private void SearchChildNodes(XmlNode elem, StringBuilder sb)
        {
            //sb.Append(elem.Name);
            IDX++;

            var list = elem.ChildNodes;
            for (int i = 0, iLen = list.Count; i < iLen; i++)
            {
                SearchChildNodes(list[i], sb);
            }
        }

        private void SearchLink(XmlNode elem, StringBuilder sb)
        {
            //sb.Append(elem.Name);
            IDX++;

            XmlNode child = elem.FirstChild;
            while (child != null)
            {
                SearchLink(child, sb);
                child = child.NextSibling;
            }
        }
    }
}
