namespace LiteGraphFrame
{
    abstract class EventNodeData : NodeDataBase
    {
        // 事件节点只有一个输出流程端口
        protected override void InitlizationPort()
        {
            var outputFlowPort = new FlowPortData();
            outputFlowPort.Initlization(this, false, "Out");
            PortList.Add(outputFlowPort);
            PortDict[outputFlowPort.MyGUID] = outputFlowPort;
        }

        public override void ExecuteLogic()
        {
            return; // event节点不需要重写执行逻辑
        }

        public abstract EBPEventId GetEventId();
    }
}
