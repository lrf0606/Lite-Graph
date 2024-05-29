using System;
using UnityEngine;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class FunctionNodeTest1 : NodeRuntime
    {
        public System.Int32 A;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "A": return A;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "A": { A = (System.Int32)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            Debug.Log($"FunctionNodeTest1.ExectueLogic,A={A}");
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===



