using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LiteGraphFrame
{
    class LiteGraphSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public Func<SearchTreeEntry, SearchWindowContext, bool> OnSelectEntryCallback;

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node")),
            };

            // 寻找所有继承自NodeDataBase的子类
            var baseType = typeof(NodeDataBase);
            List<Type> types = new List<Type>();
            var assembly = this.GetType().Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType))
                {
                    types.Add(type);
                }
            }

            // 根据找到的子类构建节点菜单
            HashSet<string> titleSet = new HashSet<string>();
            foreach (var type in types)
            {
                var titileAttribute = type.GetCustomAttribute<NodeRegisterAttribute>();
                if (titileAttribute == null)
                {
                    continue;
                }
                if (titileAttribute.Titles == null)
                {
                    Debug.LogWarning($"{type.Name} NodeTitleAttribute is null");
                    continue;
                }
                int length = titileAttribute.Titles.Length;
                for (int i = 0; i < length; i++)
                {
                    string title = titileAttribute.Titles[i];
                    if (string.IsNullOrEmpty(title))
                    {
                        continue;
                    }
                    if (titleSet.Contains(title))
                    {
                        continue;
                    }
                    if (i == length - 1)
                    {
                        var entry = new SearchTreeEntry(new GUIContent(title));
                        entry.level = i + 1;
                        entry.userData = type; // 保存菜单代表的节点类型，用于选中后创建节点
                        searchTreeEntries.Add(entry);
                    }
                    else
                    {
                        var group = new SearchTreeGroupEntry(new GUIContent(title), i + 1);
                        searchTreeEntries.Add(group);
                    }
                    titleSet.Add(title);
                }
            }
            return searchTreeEntries;
        }

        // 选中某项
        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (OnSelectEntryCallback != null)
            {
                return OnSelectEntryCallback.Invoke(searchTreeEntry, context);
            }
            return true;
        }
    }
}
