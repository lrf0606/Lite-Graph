using UnityEngine;

namespace LiteGraphFrame
{
    [NodeTitle("Value", "GetInt")]
    class ValueNode_GetInt : ValueNodeData
    {
        [NodeInput("int")]
        public int InputInt;

        [NodeOutput("int")]
        public int OutputInt;

        public override void ExecuteLogic()
        {
            OutputInt = InputInt;
            Debug.Log($"ValueNode_GetInt Execute OutputInt{OutputInt}");
        }
    }

    [NodeTitle("Value", "GetString")]
    class ValueNode_GetString : ValueNodeData
    {
        [NodeInput("string")]
        public string InputString;

        [NodeOutput("string")]
        public string OutputString;

        public override void ExecuteLogic()
        {
            OutputString = InputString;
            Debug.Log($"ValueNode_GetString Execute OutputString:{OutputString}");
        }
    }

    [NodeTitle("Value", "GetFloat")]
    class ValueNode_GetFloat : ValueNodeData
    {
        [NodeInput("float")]
        public float InputFloat;

        [NodeOutput("float")]
        public float OutputFloat;

        public override void ExecuteLogic()
        {
            OutputFloat = InputFloat;
            Debug.Log($"ValueNode_GetFloat Execute OutputFloat:{OutputFloat}");
        }
    }

    [NodeTitle("Value", "GetVector3")]
    class ValueNode_GetVector3 : ValueNodeData
    {
        [NodeInput("vector3")]
        public Vector3 InputVector3;

        [NodeOutput("vector3")]
        public Vector3 OutputVector3;
        public override void ExecuteLogic()
        {
            OutputVector3 = InputVector3;
            Debug.Log($"ValueNode_GetVector3 Execute OutputVector3:{OutputVector3}");
        }
    }

    [NodeTitle("Value", "GetRandomInt")]
    class ValueNode_GetRandomInt : ValueNodeData
    {
        [NodeOutput("一个随机的int值")]
        public int RandomInt;
        public override void ExecuteLogic()
        {
            RandomInt = Random.Range(-100, 100);
            Debug.Log($"ValueNode_GetRandomInt Execute RandomInt:{RandomInt}");
        }
    }

    [NodeTitle("Value", "MakeVector3")]
    class ValueNode_MakeVector3 : ValueNodeData
    {
        [NodeInput("X")]
        public float InputFloatX;
        [NodeInput("Y")]
        public float InputFloatY;
        [NodeInput("Z")]
        public float InputFloatZ;
        [NodeOutput("Vector3")]
        public Vector3 OutputVector3;

        public override void ExecuteLogic()
        {
            OutputVector3 = new Vector3(InputFloatX, InputFloatY, InputFloatZ);
            Debug.Log($"ValueNode_MakeVector3 Execute OutputVector3:{OutputVector3}");
        }
    }

    [NodeTitle("Value", "MoreValue")]
    class ValueNode_MoreInputAndOutput : ValueNodeData
    {
        [NodeInput("int")]
        public int InputInt;
        [NodeInput("string")]
        public string InputString;
        [NodeInput("float")]
        public float InputFloat;
        [NodeInput("vector3")]
        public Vector3 InputVector3;

        [NodeOutput("int")]
        public int OutputInt;
        [NodeOutput("string")]
        public string OutputString;
        [NodeOutput("float")]
        public float OutputFloat;
        [NodeOutput("vector3")]
        public Vector3 OutputVector3;

        public override void ExecuteLogic()
        {
            OutputInt = InputInt * (int)InputFloat;
            OutputString = $"{InputInt} {InputString} {InputFloat} {InputVector3}";
            OutputFloat = InputInt * InputFloat;
            OutputVector3 = InputVector3 * InputInt * InputFloat;
            Debug.Log($"ValueNode_MoreInputAndOutput Execute OutputInt:{OutputInt} OutputString:{OutputString} OutputFloat:{OutputFloat} OutputVector3:{OutputVector3}");
        }
    }
}
