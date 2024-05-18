namespace LiteGraphFrame
{
    abstract class EventNodeData : NodeDataBase
    {
        // �¼��ڵ�ֻ��һ��������̶˿�
        protected override void InitlizationPort()
        {
            var outputFlowPort = new FlowPortData();
            outputFlowPort.Initlization(this, false, "Out");
            PortList.Add(outputFlowPort);
            PortDict[outputFlowPort.MyGUID] = outputFlowPort;
        }

        public override void ExecuteLogic()
        {
            return; // event�ڵ㲻��Ҫ��дִ���߼�
        }

        public abstract EBPEventId GetEventId();
    }
}
