using LitJson;
using System.Collections.Generic;
using System.IO;
namespace LiteGraphFrame
{
    public class GraphRuntime
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
                    node.Graph = this;
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
                                string sourceTypeName = (string)portJsonData["SourceTypeName"];
                                string targetTypeName = (string)portJsonData["TargetTypeName"];        
                                string fieldValue = (string)portJsonData["FieldValue"];
                                port.FieldValue = ValuePraseUtil.ToObject(sourceTypeName, fieldValue);
                                port.SourceTypeName = sourceTypeName;
                                port.TargetTypeName = targetTypeName;
                            }
                            var isInputPort = (bool)portJsonData["IsInputPort"];
                            if (isInputPort)
                            {
                                node.InputPortList.Add(port);
                                if (port.PortType == EPortType.Field)
                                {
                                    node.InputFieldPortList.Add(port);
                                }
                                else
                                {
                                    node.InputFlowPortList.Add(port);
                                }                            }
                            else
                            {
                                node.OutputPortList.Add(port);
                                if (port.PortType == EPortType.Field)
                                {
                                    node.OutputFieldPortList.Add(port);
                                }
                                else
                                {
                                    node.OutputFlowPortList.Add(port);
                                }
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
            foreach (var node in m_NodeDict.Values)
            {
                node.Reset();
            }
            eventNode.Execute();
        }
    }
}

