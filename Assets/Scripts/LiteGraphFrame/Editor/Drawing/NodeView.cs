using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Reflection;
using UnityEngine.UIElements;


namespace LiteGraphFrame
{
    class NodeView : Node
    {
        private NodeDataBase m_NodeData;
        private Dictionary<string, Port> m_PortDict;
        private EdgeConnectorListener m_EdgeConnectorListener;
        public NodeDataBase NodeData => m_NodeData;

        public NodeView(EdgeConnectorListener edgeConnectorListener)
        {
            m_EdgeConnectorListener = edgeConnectorListener;
            m_PortDict = new Dictionary<string, Port>();
        }

        public void Initlization(NodeDataBase nodeData)
        {
            m_NodeData = nodeData;
            InitlizationTitle();
            InitlizationPorts();
        }

        void InitlizationTitle()
        {
            if (string.IsNullOrEmpty(m_NodeData.Name))
            {
                var titleAttribute = m_NodeData.GetType().GetCustomAttribute<NodeRegisterAttribute>();
                this.title = titleAttribute.Title;
            }
            else
            {
                this.title = m_NodeData.Name;
            }
        }

        void InitlizationPorts()
        {
            foreach (var portData in m_NodeData.PortList)
            {
                var portView = PortView.Create(portData, m_EdgeConnectorListener) ;
                if (portData.IsInputPort)
                {
                    inputContainer.Add(portView);
                }
                else
                {
                    outputContainer.Add(portView); 
                }
                m_PortDict[portData.MyGUID] = portView;
            }
        }

        public void OnNodeViewConnectedChange(bool isConnected, Port port)
        {
            foreach (var view in port.Children())
            {
                if (view is PortFieldInputView fieldInputView)
                {
                    fieldInputView.RefreshVisible();
                }
            }
        }
        public Port GetPortViewByGUID(string guid)
        {
            m_PortDict.TryGetValue(guid, out var portView);
            return portView;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // 暂时不需要"Disconnect all"功能，需要的话在GraphView里重新实现
            // base.BuildContextualMenu(evt);
            return;
        }

    }
}
