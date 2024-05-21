using LitJson;
using System;
using System.Collections.Generic;


namespace LiteGraphFrame
{
    abstract class NodeDataBase : IJsonable
    {
        public string MyGUID { get; private set; }
        public string Name { get; set; }
        public ENodeType NodeType { get; protected set; }
        public float[] Position { get; set; }

        public List<PortDataBase> PortList { get; set; }
        public Dictionary<string, PortDataBase> PortDict { get; set; }
        public Dictionary<string, ConnectionInfo> PortConnectionDict { get; set; } // 不需要序列化

        public NodeDataBase()
        {
            MyGUID = Guid.NewGuid().ToString("N");
            Name = string.Empty;
            Position = new float[2] { 0, 0 };
            PortList = new List<PortDataBase>();
            PortDict = new Dictionary<string, PortDataBase>();
            PortConnectionDict = new Dictionary<string, ConnectionInfo>();
        }

        public void Initliazation()
        {
            InitlizationPort();
        }

        public override JsonData Encoder()
        {
            var nodeJsonData = new JsonData();
            nodeJsonData["ClassType"] = this.GetType().Name;
            nodeJsonData["GUID"] = MyGUID;
            nodeJsonData["Name"] = Name;
            nodeJsonData["NodeType"] = (int)NodeType;
            var positionJsonData = new JsonData();
            _ = positionJsonData.Add((double)Position[0]);
            _ = positionJsonData.Add((double)Position[1]);
            nodeJsonData["Position"] = positionJsonData;
            if (PortList.Count > 0)
            {
                var portListJsonData = new JsonData();
                foreach (var portData in PortList)
                {
                    portListJsonData.Add(portData.Encoder());
                }
                nodeJsonData["Ports"] = portListJsonData;
            }
            return nodeJsonData;
        }

        public override void Decoder(JsonData jsonData)
        {
            MyGUID = (string)jsonData["GUID"];
            Name = (string)jsonData["Name"];
            NodeType = (ENodeType)(int)jsonData["NodeType"];
            var positionJsonData = jsonData["Position"];
            Position = new float[2] { (float)(double)positionJsonData[0], (float)(double)positionJsonData[1] };
            if (jsonData.ContainsKey("Ports"))
            {
                var portListJsonData = jsonData["Ports"];
                foreach (JsonData portJsonData in portListJsonData)
                {
                    var classType = $"{this.GetType().Namespace}.{(string)portJsonData["ClassType"]}";
                    var portData = (PortDataBase)Activator.CreateInstance(Type.GetType(classType));
                    portData.Initlization(this);
                    portData.Decoder(portJsonData);
                    PortList.Add(portData);
                    PortDict[portData.MyGUID] = portData;
                }
            }
        }

        protected abstract void InitlizationPort();
    }
}
