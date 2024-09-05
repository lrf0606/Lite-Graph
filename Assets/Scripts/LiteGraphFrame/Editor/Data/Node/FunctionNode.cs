namespace LiteGraphFrame
{
    abstract class FunctionNodeData : NodeDataBase
    {
        public FunctionNodeData() : base()
        {
            m_NodeType = ENodeType.Function;
        }

        // 功能节点有一个输入流程节点、一个输出流程节点、n个输入数据节点
        protected override void InitlizationPort()
        {
            // 功能节点一个输入流程节点、一个输出流程节点、输入数据节点
            AddFlowPort(true, "In");
            AddFlowPort(false, "Out");
            AddInputFieldPorts();
        }
    }
}

