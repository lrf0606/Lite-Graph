using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class LogicNodeNegate : NodeRuntime
    {
        public bool A;
        public bool R;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "A": return A;
                case "R": return R;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "A": { A = (bool)value; break; };
                case "R": { R = (bool)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            R = !A;
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===