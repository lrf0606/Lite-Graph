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
            var portDict = new Dictionary<string, PortRuntime>();
            if (jsonData.ContainsKey("Nodes"))
            {
                foreach (JsonData nodeJsonData in jsonData["Nodes"])
                {
                    string nodeGUID = (string)nodeJsonData["GUID"];
                    string classType = (string)nodeJsonData["ClassType"];
                    ENodeType nodeType = (ENodeType)(int)nodeJsonData["NodeType"];
                    var node = LiteGraphNodeFactory.CreateNodeRumtime(classType);
                    node.InitGuid(nodeGUID);
                    m_NodeDict[nodeGUID] = node;
                    if (nodeType == ENodeType.Event)
                    {
                        m_EventDict[node.GetEventId()] = node;
                    }
                    var portList = new List<PortRuntime>();
                    foreach (JsonData portJsonData in nodeJsonData["Ports"])
                    {
                        var port = new PortRuntime(node, (EPortType)(int)portJsonData["PortType"], (bool)portJsonData["IsInputPort"]);
                        if (port.PortType == EPortType.Field)
                        {
                            string fieldTypeName = (string)portJsonData["FieldTypeName"];
                            port.InitFieldInfo((string)portJsonData["FieldName"], ValueParserUtil.ToObject(fieldTypeName, (string)portJsonData["FieldValue"]), fieldTypeName);
                        }
                        portList.Add(port);
                        portDict[(string)portJsonData["GUID"]] = port;
                    }
                    node.InitPorts(portList);
                }
            }
            if (jsonData.ContainsKey("Edges"))
            {
                var edgesJsonData = jsonData["Edges"];
                foreach (var edgeGUID in edgesJsonData.Keys)
                {
                    var connectionJson = edgesJsonData[edgeGUID];
                    var outputPort = portDict[(string)connectionJson[1]];
                    var inputPort = portDict[(string)connectionJson[3]];
                    var edge = new EdgeRuntime(outputPort, inputPort);
                    outputPort.AddEdge(edge);
                    inputPort.AddEdge(edge);
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

