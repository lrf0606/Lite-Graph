using System.Reflection;


namespace LiteGraphFrame
{
    abstract class ValueNodeData : NodeDataBase
    {
        public ValueNodeData() : base()
        {
            NodeType = ENodeType.Value;
        }

        protected override void InitlizationPort()
        {
            // 数据节点只有数据输入输出端口
            AddInputFieldPorts();
            AddOutputFieldPorts();
        }
    }
}
