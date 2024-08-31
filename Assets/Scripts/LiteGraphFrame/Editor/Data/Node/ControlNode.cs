namespace LiteGraphFrame
{
    abstract class ControlNodeData : NodeDataBase
    {
        public ControlNodeData() : base()
        {
            NodeType = ENodeType.Control;
        }
    }
}
