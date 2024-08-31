using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ControlNodeIf : NodeRuntime
    {
        public System.Boolean BoolValue;

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
                case "BoolValue": { BoolValue = (System.Boolean)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            
        }

        public override NodeRuntime GetNextExecuteNode()
        {
            var connectedPort = BoolValue ? OutputFlowPortList[0].ConnectedPort : OutputFlowPortList[1].ConnectedPort;
            if (connectedPort != null)
            {
                return connectedPort.Node;
            }
            return null;
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===