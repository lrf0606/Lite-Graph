using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class LogicNodeComparison : NodeRuntime
    {
        public System.Single A;
        public System.Single B;
        public System.Boolean R1;
        public System.Boolean R2;
        public System.Boolean R3;
        public System.Boolean R4;
        public System.Boolean R5;
        public System.Boolean R6;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "A": return A;
                case "B": return B;
                case "R1": return R1;
                case "R2": return R2;
                case "R3": return R3;
                case "R4": return R4;
                case "R5": return R5;
                case "R6": return R6;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "A": { A = (System.Single)value; break; };
                case "B": { B = (System.Single)value; break; };
                case "R1": { R1 = (System.Boolean)value; break; };
                case "R2": { R2 = (System.Boolean)value; break; };
                case "R3": { R3 = (System.Boolean)value; break; };
                case "R4": { R4 = (System.Boolean)value; break; };
                case "R5": { R5 = (System.Boolean)value; break; };
                case "R6": { R6 = (System.Boolean)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            R1 = A < B;
            R2 = A <= B;
            R3 = UnityEngine.Mathf.Approximately(A, B);
            R4 = !R3;
            R5 = A > B;
            R6 = A >= B;
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===