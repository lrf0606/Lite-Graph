namespace LiteGraphFrame
{
    abstract class LogicNodeData : NodeDataBase
    {
        public LogicNodeData() : base()
        {
            NodeType = ENodeType.Logic;
        }

        protected override void InitlizationPort()
        {
            // 逻辑节点只有数据输入输出端口
            AddInputFieldPorts();
            AddOutputFieldPorts();
        }
    }
}
