namespace LiteGraphFrame
{
    [NodeRegister("Func", "Func1")]
    sealed class FunctionNodeTest1 : FunctionNodeData
    {
        [NodeInput("this is a int")]
        public int A;
    }

    [NodeRegister("Func", "Func2")]
    sealed class FunctionNodeTest2 : FunctionNodeData
    {
        [NodeInput("this is a vec3")]
        public UnityEngine.Vector3 vec3;
    }
}


