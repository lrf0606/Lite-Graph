namespace LiteGraphFrame
{
    abstract class EventNodeData : NodeDataBase
    {
        public EventNodeData() : base()
        {
            NodeType = ENodeType.Event;
        }

        // 事件节点只有一个输出流程端口
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
