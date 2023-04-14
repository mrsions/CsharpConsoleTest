//===============================================================================================
// Copyright 2018 Realwith, Inc. All Rights Reserved.
// Even if it is purchased commercially, It has the right to share or resell only Realwith, Inc.
//===============================================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ElementManagement
{
    public interface IElement
    {
        ElementContext Context { get; set; }
        string Id { get; set; }
        IElement Parent { get; set; }

        int GetClassNamesCount();
        string GetClassName(int index);
        void AddClassName(string className);
        void RemoveClassName(string className);
        bool HasClassName(string className);
        IEnumerable<string> GetClassNames();

        string GetProperty(string key);
        bool TryGetProperty(string key, out string value);
        bool HasProperty(string key);
        void SetProperty(string key, string value);
        void RemoveProperty(string key);
        IEnumerable<KeyValuePair<string, string>> GetProperties();

        IElement GetChild(int index);
        int GetChildrenCount();
        void AddChild(IElement element);
        void RemoveChild(IElement element);
        IEnumerable<IElement> GetChildren();
    }
}