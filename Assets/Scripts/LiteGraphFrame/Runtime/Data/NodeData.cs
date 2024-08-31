using System.Collections.Generic;
using LitJson;

namespace LiteGraphFrame
{
    public enum ENodeType
    { 
        Event,
        Function,
        Value,
        Control,
    }

    public abstract class NodeRuntime
    {
        public string MyGUID;
        public GraphRuntime Graph;
        public List<PortRuntime> InputPortList;
        public List<PortRuntime> OutputPortList;
        public List<PortRuntime> InputFieldPortList;
        public List<PortRuntime> OutputFieldPortList;
        public List<PortRuntime> InputFlowPortList;
        public List<PortRuntime> OutputFlowPortList;
        private bool m_IsExecuted;

        public NodeRuntime()
        {
            InputPortList = new List<PortRuntime>();
            OutputPortList = new List<PortRuntime>();
            InputFieldPortList = new List<PortRuntime>();
            OutputFieldPortList = new List<PortRuntime>();
            InputFlowPortList = new List<PortRuntime>();
            OutputFlowPortList = new List<PortRuntime>();
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

        public virtual NodeRuntime GetNextExecuteNode()
        {
            if (OutputFlowPortList.Count == 0)
            {
                return null;
            }
            var connectedPort = OutputFlowPortList[0].ConnectedPort;
            if (connectedPort == null)
            {
                return null;
            }
            return connectedPort.Node;
        }

        public void Reset()
        {
            m_IsExecuted = false;
        }

        public bool IsExecuted()
        {
            return m_IsExecuted;
        }

        public void Execute()
        {
            if (m_IsExecuted)
            {
                return;
            }
            m_IsExecuted = true;

            // step1.递归先去执行上一个节点
            foreach(var port in InputFieldPortList) 
            {
                if (port.ConnectedPort != null)
                {
                    port.ConnectedPort.Node.Execute();
                }
                // step2.根据输入端口更新节点数据
                SetValue(port.FieldName, port.FieldValue);
            }

            // step3.执行节点自定义逻辑
            ExecuteLogic();

            // step4.更新输出端口数据、端口值传递给连接端口
            UpdateOutputFieldPorts();

            // step5.进入下一个节点
            GetNextExecuteNode()?.Execute();
            return;
        }

        protected void UpdateOutputFieldPorts()
        {
            foreach (var port in OutputFieldPortList)
            {
                if (port.ConnectedPort == null)
                {
                    continue;
                }
                var fieldValue = GetValue(port.FieldName);
                port.FieldValue = fieldValue;
                if (string.IsNullOrEmpty(port.TargetTypeName))
                {
                    port.ConnectedPort.FieldValue = fieldValue;
                }
                else
                {
                    // 基本类型转化
                    port.ConnectedPort.FieldValue = ValuePraseUtil.ToObject(port.TargetTypeName, ValuePraseUtil.ToString(port.SourceTypeName, fieldValue));
                }
            }
        }

    }

}
