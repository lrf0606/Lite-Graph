using System;
using UnityEngine;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class FunctionNodeTest2 : NodeRuntime
    {
        public UnityEngine.Vector3 vec3;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "vec3": return vec3;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "vec3": { vec3 = (UnityEngine.Vector3)value; break; };
                default: break;
            }
        }
        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            Debug.Log($"FunctionNodeTest2.ExectueLogic,vec3={vec3}");
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===
