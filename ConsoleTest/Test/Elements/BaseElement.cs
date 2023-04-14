//===============================================================================================
// Copyright 2018 Realwith, Inc. All Rights Reserved.
// Even if it is purchased commercially, It has the right to share or resell only Realwith, Inc.
//===============================================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ElementManagement
{
    public class BaseElement : IElement
    {
        const string TAG = nameof(BaseElement);

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    MEMBER VARIABLES
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        //-- Public

        //-- Private
        private ElementContext mContext;
        private string mId;
        private List<string> mClassNames;
        private IElement mParent;
        private Dictionary<string, string> properties;
        private List<IElement> children;

        //-- Events

        //-- Properties for variables
        public ElementContext Context
        {
            get => mContext;
            set
            {
                if (mContext != null)
                {
                    mContext.Unregist(this);
                }

                mContext = value;
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        child.Context = value;
                    }
                }

                if (mContext != null)
                {
                    mContext.Regist(this);
                }
            }
        }

        public string Id
        {
            get => mId;
            set
            {
                if (Context != null)
                {
                    if (mId != value)
                    {
                        Context.UnregistId(this);
                        mId = value;
                        if (!string.IsNullOrWhiteSpace(mId))
                        {
                            Context.RegistId(this);
                        }
                    }
                }
                else
                {
                    mId = value;
                }
            }
        }

        public IElement Parent
        {
            get => mParent;
            set
            {
                if ((IElement)mParent != value)
                {
                    mParent?.RemoveChild(this);
                    mParent = value;
                    if (value != null)
                    {
                        if (value.Context != Context)
                        {
                            Context = value.Context;
                        }
                        value.AddChild(this);
                    }
                }
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    LIFECYCLE
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        public void Destroy()
        {
            Context = null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    ACTIVE METHODS
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        #region ClassNames
        private void EnsureClassNames()
        {
            if (mClassNames == null)
            {
                mClassNames = new List<string>();
            }
        }

        public int GetClassNamesCount()
        {
            EnsureClassNames();
            return mClassNames.Count;
        }

        public string GetClassName(int index)
        {
            EnsureClassNames();
            return mClassNames[index];
        }

        public void AddClassName(string className)
        {
            EnsureClassNames();
            mClassNames.Add(className);
            Context?.RegistClass(className, this);
        }

        public void RemoveClassName(string className)
        {
            EnsureClassNames();
            if (mClassNames.Contains(className))
            {
                mClassNames.Remove(className);
                Context?.UnregistClass(className, this);
            }
        }

        public bool HasClassName(string className)
        {
            EnsureClassNames();
            return mClassNames.Contains(className);
        }

        public IEnumerable<string> GetClassNames()
        {
            EnsureClassNames();
            return mClassNames;
        }
        #endregion ClassNames

        #region Properties
        private void EnsureProperties()
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }
        }

        public string GetProperty(string key)
        {
            EnsureProperties();
            return properties[key];
        }

        public bool TryGetProperty(string key, out string value)
        {
            EnsureProperties();
            return properties.TryGetValue(key, out value);
        }

        public bool HasProperty(string key)
        {
            EnsureProperties();
            return properties.ContainsKey(key);
        }

        public void SetProperty(string key, string value)
        {
            EnsureProperties();
            properties[key] = value;
        }

        public void RemoveProperty(string key)
        {
            EnsureProperties();
            properties.Remove(key);
        }

        public IEnumerable<KeyValuePair<string, string>> GetProperties()
        {
            EnsureProperties();
            return properties;
        }
        #endregion Properties

        #region Children
        private void EnsureChildren()
        {
            if (children == null)
            {
                children = new List<IElement>();
            }
        }

        public IElement GetChild(int index)
        {
            EnsureChildren();
            return children[index];
        }

        public int GetChildrenCount()
        {
            EnsureChildren();
            return children.Count;
        }

        public void AddChild(IElement element)
        {
            EnsureChildren();
            children.Add(element);
        }

        public void RemoveChild(IElement element)
        {
            EnsureChildren();
            children.Remove(element);
        }

        public IEnumerable<IElement> GetChildren()
        {
            EnsureChildren();
            return children;
        }
        #endregion Children

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    HELPER
        //
        ///////////////////////////////////////////////////////////////////////////////////////
        
    }
}