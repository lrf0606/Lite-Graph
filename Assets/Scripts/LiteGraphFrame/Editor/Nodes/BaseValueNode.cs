using UnityEngine;

namespace LiteGraphFrame
{
    [NodeRegister("Value", "Create Vector2")]
    sealed class ValueNodeCreateVector2 : ValueNodeData, INoGenerate
    {
        [NodeInput("")]
        public float X;
        [NodeInput("")]
        public float Y;

        [NodeOutput("")]
        public Vector2 Vec2;
    }

    [NodeRegister("Value", "Create Vector3")]
    sealed class ValueNodeCreateVector3 : ValueNodeData, INoGenerate
    {
        [NodeInput("")]
        public float X;
        [NodeInput("")]
        public float Y;
        [NodeInput("")]
        public float Z;

        [NodeOutput("")]
        public Vector3 Vec3;
    }
}
