namespace LiteGraphFrame
{
    [NodeRegister("Value", "GetRandomInt")]
    sealed class ValueNodeGetRandomInt : ValueNodeData
    {
        [NodeInput("range min")]
        public int Min;
        [NodeInput("range max")]
        public int Max = 100;

        [NodeOutput()]
        public int R;
    }
}