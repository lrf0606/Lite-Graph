using System.Collections.Generic;

namespace LiteGraphFrame
{
    public enum ENodeType
    { 
        Event,
        Function,
        Value,
    }

    public abstract class NodeRuntime
    {
        public string MyGUID;
        public List<PortRuntime> InputPortList;
        public List<PortRuntime> OutputPortList;

        public NodeRuntime()
        {
            InputPortList = new List<PortRuntime>();
            OutputPortList = new List<PortRuntime>();
        }


        public virtual object GetValue(string fieldName)
        {
            return null;
        }

        public virtual void SetValue(string fieldName, object value)
        {

        }

        public virtual void ExecuteLogic()
        {

        }

        public virtual int GetEventId()
        {
            return 0;
        }

        public void Execute()
        {
            ExecuteLogic();
        }
    }


}
