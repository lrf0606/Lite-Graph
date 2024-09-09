using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ControlNodeIf : NodeRuntime
    {
        public bool BoolValue;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "BoolValue": return BoolValue;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "BoolValue": { BoolValue = (bool)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            
        }

        public override NodeRuntime GetNextExecuteNode()
        {
            var port = BoolValue ? m_OutputFlowPortList[0] : m_OutputFlowPortList[1];
            foreach(var edge in port.Edges)
            {
                return edge.InputPort.Node;
            }
            return null;
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===