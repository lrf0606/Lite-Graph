using System.Collections.Generic;

namespace LiteGraphFrame
{
    public enum ENodeType
    { 
        Event,
        Function,
        Value,
        Control,
        Logic,
    }

    public abstract class NodeRuntime
    {
        private string m_GUID;
        public string MyGUID => m_GUID;

        private List<PortRuntime> m_InputPortList;
        private List<PortRuntime> m_OutputPortList;
        private List<PortRuntime> m_InputFieldPortList;
        private List<PortRuntime> m_OutputFieldPortList;
        private List<PortRuntime> m_InputFlowPortList;
        protected List<PortRuntime> m_OutputFlowPortList;

        public IEnumerable<PortRuntime> InputPortList => m_InputPortList;
        public IEnumerable<PortRuntime> OutputPortList => m_OutputPortList;
        private bool m_IsExecuted;

        public void InitGuid(string guid)
        {
            m_GUID = guid;
        }

        public void InitPorts(List<PortRuntime> portList) 
        {
            m_InputPortList = new List<PortRuntime>();
            m_OutputPortList = new List<PortRuntime>();
            m_InputFieldPortList = new List<PortRuntime>();
            m_OutputFieldPortList = new List<PortRuntime>();
            m_InputFlowPortList = new List<PortRuntime>();
            m_OutputFlowPortList = new List<PortRuntime>();
            foreach (var port in portList)
            {
                if (port.IsInputPort)
                {
                    m_InputPortList.Add(port);
                    if (port.PortType == EPortType.Field)
                    {
                        m_InputFieldPortList.Add(port);
                    }
                    else
                    {
                        m_InputFlowPortList.Add(port);
                    }
                }
                else
                {
                    m_OutputPortList.Add(port);
                    if (port.PortType == EPortType.Field)
                    {
                        m_OutputFieldPortList.Add(port);
                    }
                    else
                    {
                        m_OutputFlowPortList.Add(port);
                    }
                }
            }
        }

        public NodeRuntime()
        {
            m_InputPortList = new List<PortRuntime>();
            m_OutputPortList = new List<PortRuntime>();
            m_InputFieldPortList = new List<PortRuntime>();
            m_OutputFieldPortList = new List<PortRuntime>();
            m_InputFlowPortList = new List<PortRuntime>();
            m_OutputFlowPortList = new List<PortRuntime>();
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
            if (m_OutputFlowPortList.Count == 0)
            {
                return null;
            }
            foreach(var edge in m_OutputFlowPortList[0].Edges) 
            {
                return edge.InputPort.Node;
            }
            return null;
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
            foreach(var port in m_InputFieldPortList) 
            {
                foreach(var edge in port.Edges)
                {
                    edge.OutputPort.Node.Execute();
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
            foreach (var port in m_OutputFieldPortList)
            {
                port.UpdateFieldValue(GetValue(port.FieldName));
                foreach(var edge in port.Edges)
                {
                    edge.InputPort.UpdateFieldValue(port.FieldValue, connectedPort: port);
                }
            }
        }
    }

}
