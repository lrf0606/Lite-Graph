namespace LiteGraphFrame
{
    abstract class ControlNodeData : NodeDataBase
    {
        public ControlNodeData() : base()
        {
            m_NodeType = ENodeType.Control;
        }
    }
}
