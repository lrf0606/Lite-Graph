using System;
using UnityEngine;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ValueNodeGetRandomInt : NodeRuntime
    {
        public int Min;
        public int Max;
        public int R;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "Min": return Min;
                case "Max": return Max;
                case "R": return R;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "Min": { Min = (int)value; break; };
                case "Max": { Max = (int)value; break; };
                case "R": { R = (int)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            R = UnityEngine.Random.Range(Min, Max);
            Debug.Log($"ValueNodeGetRandomInt ExecuteLogic Min={Min} Max={Max} R={R}");
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===