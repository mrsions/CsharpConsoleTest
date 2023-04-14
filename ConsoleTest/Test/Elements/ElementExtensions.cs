//===============================================================================================
// Copyright 2018 Realwith, Inc. All Rights Reserved.
// Even if it is purchased commercially, It has the right to share or resell only Realwith, Inc.
//===============================================================================================

using SFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElementManagement
{
    public static class ElementExtensions
    {
        public static string Path(this IElement element)
        {
            using var elems = PList.Take<IElement>();
            var e = element;
            do
            {
                elems.Add(e);
            }
            while ((e = e.Parent) != null);

            StringBuilder sb = new StringBuilder();
            for (int i = elems.Count - 1; i >= 0; i--)
            {
                if (sb.Length != 0)
                {
                    sb.Append(' ');
                }

                var elem = elems[i];
                if (!elem.Id.IsEmpty()) sb.Append('#').Append(elem.Id);
                foreach (var classname in elem.GetClassNames())
                {
                    sb.Append('.').Append(classname);
                }
                foreach (var attr in elem.GetProperties())
                {
                    sb.Append('[').Append(attr.Key).Append("=\"").Append(attr.Value).Append("\"]");
                }
            }
            return sb.ToString();
        }
    }
}