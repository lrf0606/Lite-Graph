using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LiteGraphFrame
{
    abstract class PortDataBase : IJsonable
    {
        private string m_GUID;
        private string m_Name;
        private bool m_IsInputPort;
        protected EPortType m_PortType;
        private NodeDataBase m_NodeData;
        private HashSet<EdgeData> m_EdgeSet;
        
        public string MyGUID => m_GUID;
        public string Name => m_Name;
        public bool IsInputPort => m_IsInputPort;
        public NodeDataBase NodeData => m_NodeData;
        public IEnumerable<EdgeData> Edges => m_EdgeSet;

        public PortDataBase()
        {

        }

        public void Initlization(NodeDataBase owner, bool isInputPort=true, string name="")
        {
            m_GUID = Guid.NewGuid().ToString("N");
            m_NodeData = owner;
            m_IsInputPort = isInputPort;
            m_Name = name;
            m_EdgeSet = new HashSet<EdgeData>();
        }

        public virtual bool CanConnectTo(PortDataBase otherPortData)
        {
            if (otherPortData == null)
            {
                return false;
            }
            if (this == otherPortData)
            {
                return false;
            }
            if (m_NodeData == otherPortData.NodeData)
            {
                return false;
            }
            if (m_IsInputPort == otherPortData.IsInputPort)
            {
                return false;
            }
            return true;
        }
        
        public virtual void OnConnectedChange(bool isConnected, PortDataBase otherPortData, EdgeData edgeData)
        {
            if (isConnected) 
            {
                m_EdgeSet.Add(edgeData);
            }
            else
            {
                m_EdgeSet.Remove(edgeData);
            }
        }
    
        public bool IsConnected()
        {
            return m_EdgeSet.Count > 0;
        }


        public EdgeData GetEdgeDataByConnectedPort(PortDataBase connectedPortData)
        {
            foreach(var edgeData in m_EdgeSet)
            {
                var targetPortData = IsInputPort ? edgeData.OutputPortData : edgeData.InputPortData;
                if (targetPortData == connectedPortData)
                {
                    return edgeData;
                }    
            }
            return null;
        }

        public override JsonData Encoder()
        {
            var jsonData = new JsonData();
            jsonData["ClassType"] = this.GetType().Name;
            jsonData["GUID"] = m_GUID;
            jsonData["IsInputPort"] = m_IsInputPort;
            jsonData["Name"] = m_Name;
            jsonData["PortType"] = (int)m_PortType;
            return jsonData;
        }

        public override void Decoder(JsonData jsonData)
        {
            m_GUID = (string)jsonData["GUID"];
            m_IsInputPort = (bool)jsonData["IsInputPort"];
            m_Name = (string)jsonData["Name"];
            m_PortType = (EPortType)(int)jsonData["PortType"];
        }
    }
}
