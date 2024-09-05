using LitJson;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace LiteGraphFrame
{
    abstract class NodeDataBase : IJsonable
    {
        private string m_GUID;
        private string m_Name;
        protected ENodeType m_NodeType;
        private float[] m_Position;

        private Dictionary<string, PortDataBase> m_PortDict;
        private List<PortDataBase> m_PortList;

        public string MyGUID => m_GUID;
        public string Name => m_Name;
        public float[] Position => m_Position;
        public IEnumerable<PortDataBase> PortList => m_PortList;


        public NodeDataBase()
        {

        }

        public void Initliazation(bool isDecoded=false)
        {
            if (!isDecoded)
            {
                m_GUID = Guid.NewGuid().ToString("N");
            }
            m_Name = string.Empty;
            m_Position = new float[2] { 0, 0 };
            m_PortList = new List<PortDataBase>();
            m_PortDict = new Dictionary<string, PortDataBase>();
            if (!isDecoded)
            {
                InitlizationPort();
            }
        }

        protected abstract void InitlizationPort();

        public override JsonData Encoder()
        {
            var nodeJsonData = new JsonData();
            nodeJsonData["ClassType"] = this.GetType().Name;
            nodeJsonData["GUID"] = m_GUID;
            nodeJsonData["Name"] = m_Name;
            nodeJsonData["NodeType"] = (int)m_NodeType;
            var positionJsonData = new JsonData();
            _ = positionJsonData.Add((double)m_Position[0]);
            _ = positionJsonData.Add((double)m_Position[1]);
            nodeJsonData["Position"] = positionJsonData;
            if (m_PortList.Count > 0)
            {
                var portListJsonData = new JsonData();
                foreach (var portData in m_PortList)
                {
                    portListJsonData.Add(portData.Encoder());
                }
                nodeJsonData["Ports"] = portListJsonData;
            }
            return nodeJsonData;
        }

        public override void Decoder(JsonData jsonData)
        {
            m_GUID = (string)jsonData["GUID"];
            m_Name = (string)jsonData["Name"];
            m_NodeType = (ENodeType)(int)jsonData["NodeType"];
            var positionJsonData = jsonData["Position"];
            SetPosition((float)(double)positionJsonData[0], (float)(double)positionJsonData[1]);
            if (jsonData.ContainsKey("Ports"))
            {
                var portListJsonData = jsonData["Ports"];
                foreach (JsonData portJsonData in portListJsonData)
                {
                    var classType = $"{this.GetType().Namespace}.{(string)portJsonData["ClassType"]}";
                    var portData = (PortDataBase)Activator.CreateInstance(Type.GetType(classType));
                    portData.Initlization(this);
                    portData.Decoder(portJsonData);
                    m_PortList.Add(portData);
                    m_PortDict[portData.MyGUID] = portData;
                }
            }
        }

        public void AddFlowPort(bool isInputPort = true, string name = "")
        {
            var flowPort = new FlowPortData();
            flowPort.Initlization(this, isInputPort, name);
            m_PortList.Add(flowPort);
            m_PortDict[flowPort.MyGUID] = flowPort;
        }

        public void AddInputFieldPorts()
        {
            foreach (var field in this.GetType().GetFields())
            {
                var inputAttribute = field.GetCustomAttribute<NodeInputAttribute>();
                if (inputAttribute != null)
                {
                    var inputFieldPort = new FieldPortData();
                    inputFieldPort.Initlization(this, true, field.Name);
                    inputFieldPort.InitFieldIfno(field, inputAttribute.FiledDescription);
                    m_PortList.Add(inputFieldPort);
                    m_PortDict[inputFieldPort.MyGUID] = inputFieldPort;
                }
            }
        }

        public void AddOutputFieldPorts()
        {
            foreach (var field in this.GetType().GetFields())
            {
                var outputAttribut = field.GetCustomAttribute<NodeOutputAttribute>();
                if (outputAttribut != null)
                {
                    var outputFieldPort = new FieldPortData();
                    outputFieldPort.Initlization(this, false, field.Name);
                    outputFieldPort.InitFieldIfno(field, outputAttribut.FiledDescription);
                    m_PortList.Add(outputFieldPort);
                    m_PortDict[outputFieldPort.MyGUID] = outputFieldPort;
                }
            }
        }

        public void SetPosition(float x, float y)
        {
            m_Position[0] = x;
            m_Position[1] = y;
        }

        public PortDataBase GetPortByGUID(string guid)
        {
            m_PortDict.TryGetValue(guid, out var portData);
            return portData;
        }
    }
}
