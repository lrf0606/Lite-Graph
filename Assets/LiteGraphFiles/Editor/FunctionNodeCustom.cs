namespace LiteGraphFrame
{
    [NodeRegister("Func", "Log")]
    sealed class FunctionNodeLog : FunctionNodeData
    {
        [NodeInput]
        public string LogMsg;
    }
}


