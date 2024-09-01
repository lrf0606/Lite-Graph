namespace LiteGraphFrame
{
    [NodeRegister("Logic", "And")]
    sealed class LogicNodeAnd : LogicNodeData, IBuiltinNode
    {
        [NodeInput]
        public bool A;
        [NodeInput]
        public bool B;

        [NodeOutput("A & B")]
        public bool R;
    }

    [NodeRegister("Logic", "Or")]
    sealed class LogicNodeOr : LogicNodeData, IBuiltinNode
    {
        [NodeInput]
        public bool A;
        [NodeInput]
        public bool B;

        [NodeOutput("A | B")]
        public bool R;
    }

    [NodeRegister("Logic", "Exclusive Or")]
    sealed class LogicNodeExclusiveOr : LogicNodeData, IBuiltinNode
    {
        [NodeInput]
        public bool A;
        [NodeInput]
        public bool B;

        [NodeOutput("A ¨’ B")]
        public bool R;
    }

    [NodeRegister("Logic", "Negate")]
    sealed class LogicNodeNegate : LogicNodeData, IBuiltinNode
    {
        [NodeInput]
        public bool A;

        [NodeOutput("~A")]
        public bool R;
    }

    [NodeRegister("Logic", "Comparison")]
    sealed class LogicNodeComparison : LogicNodeData, IBuiltinNode
    {
        [NodeInput]
        public float A;
        [NodeInput]
        public float B;

        [NodeOutput("A < B")]
        public bool R1;
        [NodeOutput("A ¡Ü B")]
        public bool R2;
        [NodeOutput("A = B")]
        public bool R3;
        [NodeOutput("A ¡Ù B")]
        public bool R4;
        [NodeOutput("A > B")]
        public bool R5;
        [NodeOutput("A ¡Ý B")]
        public bool R6;
    }

}