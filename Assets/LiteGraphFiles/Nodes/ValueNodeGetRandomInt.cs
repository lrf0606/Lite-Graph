using UnityEngine;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ValueNodeGetRandomInt : NodeRuntime
    {
        public System.Int32 Min;
        public System.Int32 Max;
        public System.Int32 R;

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
                case "Min": { Min = (System.Int32)value; break; };
                case "Max": { Max = (System.Int32)value; break; };
                case "R": { R = (System.Int32)value; break; };
                default: break;
            }
        }
        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            var i = Random.Range(Min, Max);
            Debug.Log($"ValueNodeGetRandomInt Min={Min} Max={Max} randomint={i}");
            R = i;
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===
