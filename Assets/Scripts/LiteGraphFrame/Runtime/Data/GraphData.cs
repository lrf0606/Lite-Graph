using LitJson;
using System.Collections.Generic;
using System.IO;
namespace LiteGraphFrame
{
    class GraphRuntime
    {
        private string m_AssetPath;
        private Dictionary<string, NodeRuntime> m_NodeDict;
        private Dictionary<int, NodeRuntime> m_EventDict;

        public GraphRuntime(string assetPath)
        {
            m_AssetPath = assetPath;
            m_NodeDict = new Dictionary<string, NodeRuntime>();
            m_EventDict = new Dictionary<int, NodeRuntime>();

            Initlization(assetPath);
        }

        private void Initlization(string assetPath)
        {
            var fileData = File.ReadAllText(assetPath);
            var jsonData = JsonMapper.ToObject(fileData);
            Dictionary<string, PortRuntime> portDict = new Dictionary<string, PortRuntime>();
            if (jsonData.ContainsKey("Nodes"))
            {
                foreach (JsonData nodeJsonData in jsonData["Nodes"])
                {
                    string nodeGUID = (string)nodeJsonData["GUID"];
                    string classType = (string)nodeJsonData["ClassType"];
                    ENodeType nodeType = (ENodeType)(int)nodeJsonData["NodeType"];
                    var node = LiteGraphNodeFactory.CreateNodeRumtime(classType);
                    node.MyGUID = nodeGUID;
                    m_NodeDict[nodeGUID] = node;
                    if (nodeType == ENodeType.Event)
                    {
                        m_EventDict[node.GetEventId()] = node;
                    }
                    if (nodeJsonData.ContainsKey("Ports"))
                    {
                        foreach(JsonData portJsonData in nodeJsonData["Ports"])
                        {
                            var port = new PortRuntime();
                            port.Node = node;
                            port.PortType = (EPortType)(int)portJsonData["PortType"];
                            if (port.PortType == EPortType.Field)
                            {
                                port.FieldName = (string)portJsonData["FieldName"];
                                string typeName = (string)portJsonData["TypeName"];
                                string fieldValue = (string)portJsonData["FieldValue"];
                                port.FieldValue = ParseRuntimeUtil.ToObject(typeName, fieldValue);
                            }
                            var isInputPort = (bool)portJsonData["IsInputPort"];
                            if (isInputPort)
                            {
                                node.InputPortList.Add(port);
                            }
                            else
                            {
                                node.OutputPortList.Add(port);
                            }
                            portDict[(string)portJsonData["GUID"]] = port;
                        }
                    }
                }
            }
            if (jsonData.ContainsKey("Edges"))
            {
                foreach (var nodeGUID in jsonData["Edges"].Keys)
                {
                    if (!m_NodeDict.ContainsKey(nodeGUID))
                    {
                        continue;
                    }
                    var portConnectionJsonData = jsonData["Edges"][nodeGUID];
                    foreach (var portGUID in portConnectionJsonData.Keys)
                    {
                        if (portDict.TryGetValue(portGUID, out var port))
                        {
                            if (portDict.TryGetValue((string)portConnectionJsonData[portGUID][1], out var connectedPort))
                            {
                                port.ConnectedPort = connectedPort;
                            }     
                        }
                    }
                }
            }
        }

        public void RunEvent(int eventId)
        {
            if (!m_EventDict.TryGetValue(eventId, out var eventNode)) 
            {
                throw new System.Exception($"event({eventId} not exist in {m_AssetPath})");
            }
            var nodeList = GenerageNodeExecuteList(eventNode);
            foreach (var curNode in nodeList)
            {
                var inputFieldPortList = new List<PortRuntime>();
                var outputFieldPortList = new List<PortRuntime>();
                foreach(var port in curNode.InputPortList)
                {
                    if (port.PortType == EPortType.Field)
                    {
                        inputFieldPortList.Add(port);
                    }
                }
                foreach (var port in curNode.OutputPortList)
                {
                    if (port.PortType == EPortType.Field)
                    {
                        outputFieldPortList.Add(port);
                    }
                }

                // step1.input field port给node赋值
                foreach (var port in inputFieldPortList)
                {
                    curNode.SetValue(port.FieldName, port.FieldValue);
                }

                // step2.node execute
                curNode.Execute();
                
                // step3.node给下一个node的input field port赋值
                foreach (var port in outputFieldPortList)
                {
                    if (port.ConnectedPort == null)
                    {
                        continue;
                    }
                    var fieldValue = curNode.GetValue(port.FieldName);
                    port.FieldValue = fieldValue;
                    port.ConnectedPort.FieldValue = fieldValue;
                }
            }
        }

        private List<NodeRuntime> GenerageNodeExecuteList(NodeRuntime startNode)
        {
            var resultList = new List<NodeRuntime>() { };
            var addedNodeSet = new HashSet<string>() { };

            var curNode = startNode;
            var outputFlowPort = new List<PortRuntime>();
            while (curNode != null)
            {
                outputFlowPort.Clear();
                foreach (var port in curNode.OutputPortList)
                {
                    if (port.PortType == EPortType.Flow)
                    {
                        outputFlowPort.Add(port);
                    }
                }
                FindPreviousNodes(curNode, resultList, addedNodeSet);
                curNode = outputFlowPort.Count > 0 ? outputFlowPort[0].ConnectedPort?.Node : null;
            }
            return resultList;
        }

        private void FindPreviousNodes(NodeRuntime curNode, List<NodeRuntime> resultList, HashSet<string> addedNodeSet)
        {
            var prewNodeList = new List<NodeRuntime>();
            foreach (var port in curNode.InputPortList)
            {
                if (port.ConnectedPort != null)
                {
                    prewNodeList.Add(port.ConnectedPort.Node);
                }
            }
            foreach (var prevNode in prewNodeList)
            {
                if (addedNodeSet.Contains(prevNode.MyGUID))
                {
                    continue;
                }
                FindPreviousNodes(prevNode, resultList, addedNodeSet);
            }
            resultList.Add(curNode);
            addedNodeSet.Add(curNode.MyGUID);
        }
    }
}

