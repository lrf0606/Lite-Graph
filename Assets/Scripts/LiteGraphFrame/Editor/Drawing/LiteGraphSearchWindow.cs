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

            var titleDict = new Dictionary<string, List<Type>>();

            foreach (var type in types)
            {
                var titileAttribute = type.GetCustomAttribute<NodeRegisterAttribute>();
                if (titileAttribute == null)
                {
                    continue;
                }
                if (titleDict.TryGetValue(titileAttribute.Directory, out List<Type> typeList))
                {
                    typeList.Add(type);
                }
                else
                {
                    titleDict[titileAttribute.Directory] = new List<Type> { type };
                }
            }

            foreach (var kv in titleDict)
            {
                var directory = kv.Key;
                var group = new SearchTreeGroupEntry(new GUIContent(kv.Key), 1);
                searchTreeEntries.Add(group);
                foreach (var type in kv.Value)
                {
                    var title = type.GetCustomAttribute<NodeRegisterAttribute>().Title;
                    var entry = new SearchTreeEntry(new GUIContent(title));
                    entry.level = 2;
                    entry.userData = type; // 保存菜单代表的节点类型，用于选中后创建节点
                    searchTreeEntries.Add(entry);
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
