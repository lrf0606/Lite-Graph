namespace LiteGraphFrame
{
    abstract class EventNodeData : NodeDataBase
    {
        public EventNodeData() : base()
        {
            NodeType = ENodeType.Event;
        }

        // �¼��ڵ�ֻ��һ��������̶˿�
        protected override void InitlizationPort()
        {
            var outputFlowPort = new FlowPortData();
            outputFlowPort.Initlization(this, false, "Out");
            PortList.Add(outputFlowPort);
            PortDict[outputFlowPort.MyGUID] = outputFlowPort;
        }

        public abstract int GetEventId();
    }
}
