namespace LiteGraphFrame
{
    abstract class EventNodeData : NodeDataBase
    {
        public EventNodeData() : base()
        {
            NodeType = ENodeType.Event;
        }

        protected override void InitlizationPort()
        {
            // 事件节点只有一个输出流程端口
            AddFlowPort(false, "Out");
        }

        public abstract int GetEventId();
    }
}
