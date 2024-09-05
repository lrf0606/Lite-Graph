namespace LiteGraphFrame
{
    abstract class ValueNodeData : NodeDataBase
    {
        public ValueNodeData() : base()
        {
            m_NodeType = ENodeType.Value;
        }

        protected override void InitlizationPort()
        {
            // 数据节点只有数据输入输出端口
            AddInputFieldPorts();
            AddOutputFieldPorts();
        }
    }
}
