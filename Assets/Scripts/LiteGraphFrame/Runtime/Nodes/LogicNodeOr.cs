using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class LogicNodeOr : NodeRuntime
    {
        public System.Boolean A;
        public System.Boolean B;
        public System.Boolean R;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "A": return A;
                case "B": return B;
                case "R": return R;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "A": { A = (System.Boolean)value; break; };
                case "B": { B = (System.Boolean)value; break; };
                case "R": { R = (System.Boolean)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            R = A | B;
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===