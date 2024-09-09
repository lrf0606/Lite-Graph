using LitJson;
using System.Collections.Generic;


namespace LiteGraphFrame
{
    public enum EPortType
    {
        Flow = 0,
        Field,
    }

    public class PortRuntime
    {
        private NodeRuntime m_Node;
        private bool m_IsInputPort;
        private EPortType m_PortType;
        private List<EdgeRuntime> m_EdgeList;
        private string m_FieldName;
        private object m_FieldValue;
        private string m_FieldTypeName;

        public NodeRuntime Node => m_Node;
        public bool IsInputPort => m_IsInputPort;
        public EPortType PortType => m_PortType;
        public IEnumerable<EdgeRuntime> Edges => m_EdgeList;
        public string FieldName => m_FieldName;
        public object FieldValue => m_FieldValue;
        public string FieldTypeName => m_FieldTypeName;

        public PortRuntime(NodeRuntime node, EPortType portType, bool isInputPort)
        {
            m_Node = node;
            m_PortType = portType;
            m_IsInputPort = isInputPort;
            m_EdgeList = new List<EdgeRuntime>();
        }

        public void InitFieldInfo(string fieldName, object fieldValue, string fieldTypeName)
        {
            m_FieldName = fieldName;
            m_FieldValue = fieldValue;
            m_FieldTypeName = fieldTypeName;
        }

        public void AddEdge(EdgeRuntime edge) 
        {
            m_EdgeList.Add(edge);
        }

        public void UpdateFieldValue(object fieldValue, PortRuntime connectedPort = null)
        {
            if (connectedPort == null) 
            {
                m_FieldValue = fieldValue;
                return;
            }
            if (m_FieldTypeName == connectedPort.FieldTypeName)
            {
                m_FieldValue = connectedPort.FieldValue;
            }
            else
            {
                m_FieldValue = ValueParserUtil.ToObject(m_FieldTypeName, connectedPort.FieldValue.ToString());
            }
        }
    }


}
