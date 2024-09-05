using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LiteGraphFrame
{
    class LiteGraphSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private PortDataBase m_ToConnectPort;
        private Func<SearchTreeEntry, SearchWindowContext, bool> m_OnSelectEntryCallback;

        public PortDataBase ToConnectPort { get { return m_ToConnectPort; } set { m_ToConnectPort = value; } }
        public Func<SearchTreeEntry, SearchWindowContext, bool> OnSelectEntryCallback { get { return m_OnSelectEntryCallback; } set { m_OnSelectEntryCallback = value; } }

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
                    if (m_ToConnectPort == null)
                    {
                        types.Add(type);
                    }
                    else
                    {
                        // 根据ToConnectPort进行筛选
                        var tempNodeData = (NodeDataBase)Activator.CreateInstance((Type)type);
                        tempNodeData.Initliazation();
                        if (FindNeedConnectPort(tempNodeData) != null)
                        {
                            types.Add(type);
                        }
                    }
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
                if (kv.Value.Count == 0)
                {
                    continue;
                }
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
            if (m_OnSelectEntryCallback == null)
            {
                return false;
            }
            return m_OnSelectEntryCallback.Invoke(searchTreeEntry, context);
        }

        // 获取第一个能和ToConnectPort连接的端口
        public PortDataBase FindNeedConnectPort(NodeDataBase nodeData)
        {
            if (m_ToConnectPort == null || nodeData == null)
            {
                return null;
            }
            foreach (var portData in nodeData.PortList)
            {
                if (m_ToConnectPort.IsInputPort)
                {
                    if (portData.CanConnectTo(m_ToConnectPort))
                    {
                        return portData;
                    }
                }
                else
                {
                    if (m_ToConnectPort.CanConnectTo(portData))
                    {
                        return portData;
                    }
                }
            }
            return null;
        }
    }
}
