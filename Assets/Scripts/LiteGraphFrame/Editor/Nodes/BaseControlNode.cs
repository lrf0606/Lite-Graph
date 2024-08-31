namespace LiteGraphFrame
{
    [NodeRegister("Control", "If")]
    sealed class ControlNodeIf : ControlNodeData, IBuiltinNode
    {
        [NodeInput]
        public bool BoolValue;

        protected override void InitlizationPort()
        {
            // If 有一个流程输入端口、两个流程输出端口、数据输入端口
            AddFlowPort(true, "In");
            AddFlowPort(false, "True");
            AddFlowPort(false, "False");
            AddInputFieldPorts();
        }
    }

    [NodeRegister("Control", "For")]
    sealed class ControlNodeForLoop : ControlNodeData, IBuiltinNode
    {
        [NodeInput]
        public int First = 0;
        [NodeInput]
        public int Last = 10;
        [NodeInput]
        public int Step = 1;

        [NodeOutput]
        public int Index;

        protected override void InitlizationPort()
        {
            // ForLoop 有一个流程输入端口、两个流程输出端口、数据输入端口、数据输出端口
            AddFlowPort(true, "In");
            AddFlowPort(false, "Exit");
            AddFlowPort(false, "Body");
            AddInputFieldPorts();
            AddOutputFieldPorts();
        }
    }

}