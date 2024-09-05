using LitJson;
using System;
using System.Collections.Generic;

namespace LiteGraphFrame
{
    sealed class GraphData : IJsonable
    {
        private string m_AssetPath;

        private Dictionary<string, NodeDataBase> m_NodeDict;
        private Dictionary<int, NodeDataBase> m_EventNodeDict;

        private List<NodeDataBase> m_AddedNodes;
        private List<NodeDataBase> m_RemovedNodes;
        private List<EdgeData> m_AddedEdges;
        private List<EdgeData> m_RemovedEdges;

        public IEnumerable<NodeDataBase> AllNodes => m_NodeDict.Values;
        public IEnumerable<NodeDataBase> AddedNodes => m_AddedNodes;
        public IEnumerable<NodeDataBase> RemovedNodes => m_RemovedNodes;
        public IEnumerable<EdgeData> AddedEdges => m_AddedEdges;
        public IEnumerable<EdgeData> RemovedEdges => m_RemovedEdges;

        public GraphData(string assetPath)
        {
            m_AssetPath = assetPath;
            m_NodeDict = new Dictionary<string, NodeDataBase>();
            m_EventNodeDict = new Dictionary<int, NodeDataBase>();
            m_AddedNodes = new List<NodeDataBase>();
            m_RemovedNodes = new List<NodeDataBase>();
            m_AddedEdges = new List<EdgeData>();
            m_RemovedEdges = new List<EdgeData>();
        }
        public void ClearChanges()
        {
            m_AddedNodes.Clear();
            m_RemovedNodes.Clear();
            m_AddedEdges.Clear();
            m_RemovedEdges.Clear();
        }

        public bool AddNode(NodeDataBase nodeData, out string failReason)
        {
            // 相同事件节点只能出现一次
            if (nodeData is EventNodeData eventNodeData)
            {
                if (m_EventNodeDict.ContainsKey(eventNodeData.GetEventId()))
                {
                    failReason = $"事件节点一张视图[{nodeData}]只能出现一次";
                    return false;
                }
                else
                {
                    m_EventNodeDict[eventNodeData.GetEventId()] = nodeData;
                }
            }
            m_NodeDict[nodeData.MyGUID] = nodeData;
            m_AddedNodes.Add(nodeData);
            failReason = "";
            return true;
        }

        public void RemoveNode(NodeDataBase nodeData)
        {
            m_NodeDict.Remove(nodeData.MyGUID);
            if (nodeData is EventNodeData eventNodeData)
            {
                m_EventNodeDict.Remove(eventNodeData.GetEventId());
            }
            m_RemovedNodes.Add(nodeData);
        }

        public void ConnectNode(PortDataBase outputPort, PortDataBase inputPort)
        {
            var edgeData = new EdgeData();
            edgeData.Initlization(outputPort, inputPort);
            m_AddedEdges.Add(edgeData);
            outputPort.OnConnectedChange(true, inputPort, edgeData);
            inputPort.OnConnectedChange(true, outputPort, edgeData);
        }

        public void DisconnectNode(PortDataBase outputPort, PortDataBase inputPort)
        {
            var edgeData = outputPort.GetEdgeDataByConnectedPort(inputPort);
            m_RemovedEdges.Add(edgeData);
            outputPort.OnConnectedChange(false, inputPort, edgeData);
            inputPort.OnConnectedChange(false, outputPort, edgeData);
        }

        public override JsonData Encoder()
        {
            var graphData = new JsonData();
            graphData["Path"] = m_AssetPath;
            var edgeSet = new HashSet<EdgeData>();
            if (m_NodeDict.Count > 0)
            {
                var nodeListJsonData = new JsonData();
                graphData["Nodes"] = nodeListJsonData;
                foreach (var nodeData in m_NodeDict.Values)
                {
                    nodeListJsonData.Add(nodeData.Encoder());
                    foreach (var portData in nodeData.PortList)
                    {
                        foreach (var edgeData in portData.Edges)
                        {
                            edgeSet.Add(edgeData);
                        }
                    }
                }
            }
            if (edgeSet.Count > 0)
            {
                var edgesJson = new JsonData();
                graphData["Edges"] = edgesJson;
                foreach (var edgeData in edgeSet)
                {
                    var connectionJson = new JsonData();
                    connectionJson.Add(edgeData.OutputPortData.NodeData.MyGUID);
                    connectionJson.Add(edgeData.OutputPortData.MyGUID);
                    connectionJson.Add(edgeData.InputPortData.NodeData.MyGUID);
                    connectionJson.Add(edgeData.InputPortData.MyGUID);
                    edgesJson[edgeData.MyGUID] = connectionJson;
                }
            }
            return graphData;
        }

        public override void Decoder(JsonData jsonData)
        {
            if (jsonData.ContainsKey("Nodes"))
            {
                var nodeListJsonData = jsonData["Nodes"];
                foreach (JsonData nodeJsonData in nodeListJsonData)
                {
                    var classType = $"{this.GetType().Namespace}.{(string)nodeJsonData["ClassType"]}";
                    var type = Type.GetType(classType);
                    if (type == null)
                    {
                        continue;
                    }
                    var nodeData = (NodeDataBase)Activator.CreateInstance(type);
                    nodeData.Initliazation(isDecoded: true);
                    nodeData.Decoder(nodeJsonData);
                    AddNode(nodeData, out _);
                }
            }
            if (jsonData.ContainsKey("Edges"))
            {
                var edgesJsonData = jsonData["Edges"];
                foreach (var edgeGUID in edgesJsonData.Keys)
                {
                    var connectionJson = edgesJsonData[edgeGUID];
                    var outputNodeGUID = (string)connectionJson[0];
                    var outputPortGUID = (string)connectionJson[1];
                    var inputNodeGUID = (string)connectionJson[2];
                    var inputPortGUID = (string)connectionJson[3];
                    ConnectNode(m_NodeDict[outputNodeGUID].GetPortByGUID(outputPortGUID), m_NodeDict[inputNodeGUID].GetPortByGUID(inputPortGUID));
                }
            }
        }
    }
}
