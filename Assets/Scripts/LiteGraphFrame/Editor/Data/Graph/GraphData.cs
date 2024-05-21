using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LiteGraphFrame
{
    class GraphData : IJsonable
    {
        public string AssetPath { get; private set; }

        public Dictionary<string, NodeDataBase> NodeDict { get; private set; }
        public Dictionary<string, Dictionary<string, ConnectionInfo>> NodeConnectionDict { get; private set; }

        public Dictionary<int, NodeDataBase> EventNodeDict { get; private set; }

        public GraphData()
        {
            NodeDict = new Dictionary<string, NodeDataBase>();
            NodeConnectionDict = new Dictionary<string, Dictionary<string, ConnectionInfo>>();
            EventNodeDict = new Dictionary<int, NodeDataBase>();
        }

        public void Initlization(string assetPath)
        {
            AssetPath = assetPath;
        }
      
        public bool AddNode(NodeDataBase nodeData)
        {
            // 相同事件节点只能出现一次
            if (nodeData is EventNodeData eventNodeData)
            {
                if (EventNodeDict.ContainsKey(eventNodeData.GetEventId()))
                {
                    return false;
                }
                else
                {
                    EventNodeDict[eventNodeData.GetEventId()] = nodeData;
                }
            }
            NodeDict[nodeData.MyGUID] = nodeData;
            NodeConnectionDict[nodeData.MyGUID] = new Dictionary<string, ConnectionInfo>();
            return true;
        }

        public void RemoveNode(NodeDataBase nodeData)
        {
            NodeDict.Remove(nodeData.MyGUID);
            if (nodeData is EventNodeData eventNodeData)
            {
                EventNodeDict.Remove(eventNodeData.GetEventId());
            }
            NodeConnectionDict.Remove(nodeData.MyGUID);
        }

        public void ConnectNode(NodeDataBase fromNodeData, PortDataBase fromPortData, NodeDataBase toNodeData, PortDataBase toPortData)
        {
            var connectionInfo1 = new ConnectionInfo(toNodeData, toPortData);
            NodeConnectionDict[fromNodeData.MyGUID][fromPortData.MyGUID] = connectionInfo1;
            fromNodeData.PortConnectionDict[fromPortData.MyGUID] = connectionInfo1;
            fromPortData.ConnectionInfo = connectionInfo1;

            var connectionInfo2 = new ConnectionInfo(fromNodeData, fromPortData);
            NodeConnectionDict[toNodeData.MyGUID][toPortData.MyGUID] = connectionInfo2;
            toNodeData.PortConnectionDict[toPortData.MyGUID] = connectionInfo2;
            toPortData.ConnectionInfo = connectionInfo2;
        }

        public void DisconnectNode(NodeDataBase fromNodeData, PortDataBase fromPortData, NodeDataBase toNodeData, PortDataBase toPortData)
        {
            NodeConnectionDict[fromNodeData.MyGUID].Remove(fromPortData.MyGUID);
            fromNodeData.PortConnectionDict.Remove(fromPortData.MyGUID);
            fromPortData.ConnectionInfo.Clear();

            NodeConnectionDict[toNodeData.MyGUID].Remove(toPortData.MyGUID);
            toNodeData.PortConnectionDict.Remove(toPortData.MyGUID);
            toPortData.ConnectionInfo.Clear();
        }

        public void UpdateNodePosition(NodeDataBase nodeData, float x, float y)
        {
            nodeData.Position[0] = x;
            nodeData.Position[1] = y;
        }

        public override JsonData Encoder()
        {
            var graphData = new JsonData();
            graphData["Path"] = AssetPath;
            if (NodeDict.Count > 0)
            {
                var nodeListJsonData = new JsonData();
                graphData["Nodes"] = nodeListJsonData;
                foreach (var nodeData in NodeDict.Values)
                {
                    nodeListJsonData.Add(nodeData.Encoder());
                }
            }
            if (NodeConnectionDict.Count > 0)
            {
                bool hasEdge = false;
                var nodeConnectionJsonData = new JsonData();
                foreach (var kv1 in NodeConnectionDict)
                {
                    string nodeGUID = kv1.Key;
                    var portConnectionDict = kv1.Value;
                    if (portConnectionDict.Count > 0)
                    {
                        var portConnectionsJsonData = new JsonData();
                        foreach(var kv2 in portConnectionDict)
                        {
                            string portGUID = kv2.Key;
                            var connectionInfo = kv2.Value;
                            var connectionJsonData = new JsonData();
                            connectionJsonData.Add(connectionInfo.NodeData.MyGUID);
                            connectionJsonData.Add(connectionInfo.PortData.MyGUID);
                            portConnectionsJsonData[portGUID] = connectionJsonData;
                            hasEdge = true;
                        }
                        nodeConnectionJsonData[nodeGUID] = portConnectionsJsonData;
                    }
                }
                if (hasEdge)
                {
                    graphData["Edges"] = nodeConnectionJsonData;
                }
            }
            return graphData;
        }

        public override void Decoder(JsonData jsonData)
        {
            if (jsonData.ContainsKey("Nodes"))
            {
                var nodeListJsonData = jsonData["Nodes"];
                foreach(JsonData nodeJsonData in nodeListJsonData)
                {
                    var classType = $"{this.GetType().Namespace}.{(string)nodeJsonData["ClassType"]}";
                    var type = Type.GetType(classType);
                    if (type == null)
                    {
                        continue;
                    }
                    var nodeData = (NodeDataBase)Activator.CreateInstance(type);
                    nodeData.Decoder(nodeJsonData);
                    AddNode(nodeData);
                }
            }
            if (jsonData.ContainsKey("Edges"))
            {
                var nodeConnectionJsonData = jsonData["Edges"];
                foreach(var nodeGUID in nodeConnectionJsonData.Keys)
                {
                    if (NodeDict.TryGetValue(nodeGUID, out var fromNodeData))
                    {
                        var portConnectionJsonData = nodeConnectionJsonData[nodeGUID];
                        foreach (var portGUID in portConnectionJsonData.Keys)
                        {
                            var connectionJsonData = portConnectionJsonData[portGUID];
                            var targetNodeGUID = (string)connectionJsonData[0];
                            var targetPortGUID = (string)connectionJsonData[1];
                            var fromPortData = fromNodeData.PortDict[portGUID];
                            var toNodeData = NodeDict[targetNodeGUID];
                            var toPortData = toNodeData.PortDict[targetPortGUID];
                            ConnectNode(fromNodeData, fromPortData, toNodeData, toPortData);
                        }
                    }
                }
            }
        }
    }
}
