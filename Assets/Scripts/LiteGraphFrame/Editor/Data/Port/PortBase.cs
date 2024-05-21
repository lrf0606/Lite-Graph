using LitJson;
using System;

namespace LiteGraphFrame
{
    abstract class PortDataBase : IJsonable
    {
        public string MyGUID { get; private set; }
        public NodeDataBase OwnerNodeData { get; private set; }
        public bool IsInputPort { get; private set; }
        public string Name { get; private set; }
        public EPortType PortType { get; protected set; }

        public ConnectionInfo ConnectionInfo; // 不需要序列化

        public PortDataBase()
        {
            MyGUID = Guid.NewGuid().ToString("N");
            ConnectionInfo = new ConnectionInfo();
        }

        public void Initlization(NodeDataBase owner, bool isInputPort=true, string name="")
        {
            OwnerNodeData = owner;
            IsInputPort = isInputPort;
            Name = name;
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
            if (OwnerNodeData == otherPortData.OwnerNodeData)
            {
                return false;
            }
            if (IsInputPort == otherPortData.IsInputPort)
            {
                return false;
            }
            return true;
        }

        public override JsonData Encoder()
        {
            var jsonData = new JsonData();
            jsonData["ClassType"] = this.GetType().Name;
            jsonData["GUID"] = MyGUID;
            jsonData["IsInputPort"] = IsInputPort;
            jsonData["Name"] = Name;
            jsonData["PortType"] = (int)PortType;
            return jsonData;
        }

        public override void Decoder(JsonData jsonData)
        {
            MyGUID = (string)jsonData["GUID"];
            IsInputPort = (bool)jsonData["IsInputPort"];
            Name = (string)jsonData["Name"]; 
            PortType = (EPortType)(int)jsonData["PortType"];
        }
    }
}
