using LitJson;
using System;
using System.Collections.Generic;

namespace LiteGraphFrame
{
    sealed class CopyPasteGraphData : IJsonable
    {
        private Dictionary<string, NodeDataBase> m_CopyNodeDict;
        private Dictionary<string, NodeDataBase> m_PasteNodeDict;
        private List<EdgeData> m_CopyEdgeList;
        private List<EdgeData> m_PasteEdgeList;

        public IEnumerable<NodeDataBase> PasteNodes => m_PasteNodeDict.Values;
        public IEnumerable<EdgeData> PasteEdges => m_PasteEdgeList;

        public CopyPasteGraphData() 
        {
            m_CopyNodeDict = new Dictionary<string, NodeDataBase>();
            m_PasteNodeDict = new Dictionary<string, NodeDataBase>();
            m_CopyEdgeList = new List<EdgeData>();
            m_PasteEdgeList = new List<EdgeData>();
        }

        public void Clear()
        {
            m_CopyNodeDict.Clear();
            m_PasteNodeDict.Clear();
            m_CopyEdgeList.Clear();
            m_PasteEdgeList.Clear();
        }

        public void AddCopyElements(IEnumerable<NodeDataBase> nodes, IEnumerable<EdgeData> edges)
        {
            foreach(var nodeData in nodes)
            {
                // 事件节点只能有一个，不复制
                if (nodeData is EventNodeData)
                {
                    continue;
                }
                m_CopyNodeDict[nodeData.MyGUID] = nodeData;
            }
            foreach(var edgeData in edges)
            {
                // 去掉无意义的连线
                if (m_CopyNodeDict.ContainsKey(edgeData.InputPortData.NodeData.MyGUID) && m_CopyNodeDict.ContainsKey(edgeData.OutputPortData.NodeData.MyGUID))
                {
                    m_CopyEdgeList.Add(edgeData);
                }
            }
        }

        public void PasteOffsetNodesPostion(float x, float y)
        {
            if (m_PasteNodeDict.Count <= 0)
            {
                return;
            }
            NodeDataBase mostLeftNode = null;
            foreach (var nodeData in m_PasteNodeDict.Values)
            {
                if (mostLeftNode == null || nodeData.Position[0] < mostLeftNode.Position[0])
                {
                    mostLeftNode = nodeData;
                }
            }
            var offsetX = x - mostLeftNode.Position[0];
            var offsetY = y - mostLeftNode.Position[1];
            if (Math.Abs(offsetX) < 100)
            {
                var deltaX = offsetX < 0 ? -100 : 100;
                offsetX += deltaX;
            }
            foreach (var nodeData in m_PasteNodeDict.Values)
            {
                nodeData.SetPosition(nodeData.Position[0] + offsetX, nodeData.Position[1] + offsetY);
            }
        }

        public override void Decoder(JsonData jsonData)
        {
            m_PasteNodeDict.Clear();
            m_PasteEdgeList.Clear();
            var guidDict = new Dictionary<string, string>();
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
                    // node和port生成新的GUID
                    var newNodeGUID = Guid.NewGuid().ToString("N");
                    guidDict[(string)nodeJsonData["GUID"]] = newNodeGUID;
                    nodeJsonData["GUID"] = newNodeGUID;
                    if (nodeJsonData.ContainsKey("Ports"))
                    {
                        foreach (JsonData portJsonData in nodeJsonData["Ports"])
                        {
                            var newPortGUID = Guid.NewGuid().ToString("N");
                            guidDict[(string)portJsonData["GUID"]] = newPortGUID;
                            portJsonData["GUID"] = newPortGUID;
                        }
                    }
                    var nodeData = (NodeDataBase)Activator.CreateInstance(type);
                    nodeData.Initliazation(isDecoded: true);
                    nodeData.Decoder(nodeJsonData);
                    m_PasteNodeDict[nodeData.MyGUID] = nodeData;    
                }
            }
            if (jsonData.ContainsKey("Edges"))
            {
                var edgesJsonData = jsonData["Edges"];
                foreach (var edgeGUID in edgesJsonData.Keys)
                {
                    var connectionJson = edgesJsonData[edgeGUID];
                    var outputNodeGUID = guidDict[(string)connectionJson[0]];
                    var outputPortGUID = guidDict[(string)connectionJson[1]];
                    var inputNodeGUID = guidDict[(string)connectionJson[2]];
                    var inputPortGUID = guidDict[(string)connectionJson[3]];
                    var edgeData = new EdgeData();
                    edgeData.Initlization(m_PasteNodeDict[outputNodeGUID].GetPortByGUID(outputPortGUID), m_PasteNodeDict[inputNodeGUID].GetPortByGUID(inputPortGUID));
                    m_PasteEdgeList.Add(edgeData);
                }
            }
        }

        public override JsonData Encoder()
        {
            var graphData = new JsonData();
            // 开头
            graphData["Head"] = "CopyPasteGraphData";
            // 生成拷贝Node数据
            if (m_CopyNodeDict.Count > 0)
            {
                var nodeListJsonData = new JsonData();
                graphData["Nodes"] = nodeListJsonData;
                foreach (var nodeData in m_CopyNodeDict.Values)
                {
                    var nodeJsonData = nodeData.Encoder();
                    nodeListJsonData.Add(nodeJsonData);
                }
            }
            // 生成拷贝Edge数据，同时修改Edge的GUID防止冲突
            if (m_CopyEdgeList.Count > 0)
            {
                var edgesJson = new JsonData();
                graphData["Edges"] = edgesJson;
                foreach (var edgeData in m_CopyEdgeList)
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
    }
}