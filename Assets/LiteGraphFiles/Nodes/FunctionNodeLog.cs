using System;
using UnityEngine;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class FunctionNodeLog : NodeRuntime
    {
        public string LogMsg;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "LogMsg": return LogMsg;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "LogMsg": { LogMsg = (string)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            Debug.Log($"Log:{LogMsg}");
        }
        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===