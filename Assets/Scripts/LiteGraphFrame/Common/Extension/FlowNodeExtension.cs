using UnityEngine;

namespace LiteGraphFrame
{

    [NodeTitle("Function", "NoneFunction")]
    class FunctionNode_None : FunctionNodeData
    {
        public override void ExecuteLogic()
        {
            Debug.Log($"NoneFunction Execute");
        }
    }

    [NodeTitle("Function", "FunctionTest1")]
    class FunctionNode_Test1 : FunctionNodeData
    {
        [NodeInput("����һ��int")]
        public int InputInt;
        [NodeInput("����һ��string")]
        public string InputString;

        public override void ExecuteLogic()
        {
            Debug.Log($"FunctionNode_Test1 Execute InputInt:{InputInt} InputString:{InputString}");
        }
    }

    [NodeTitle("Function", "FunctionTest3")]
    class FunctionNode_Test2 : FunctionNodeData
    {
        [NodeInput("����һ��vecto3")]
        public Vector3 InputVector3;

        public override void ExecuteLogic()
        {
            Debug.Log($"FunctionNode_Test2 Execute InputVector3:{InputVector3}");
        }
    }
}