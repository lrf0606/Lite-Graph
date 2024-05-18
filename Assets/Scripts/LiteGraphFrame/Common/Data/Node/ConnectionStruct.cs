namespace LiteGraphFrame
{
    struct ConnectionInfo
    {
        public NodeDataBase NodeData;
        public PortDataBase PortData;

        public ConnectionInfo(NodeDataBase nodeData, PortDataBase portData)
        {
            this.NodeData = nodeData;
            this.PortData = portData;
        }

        public void Clear()
        {
            this.NodeData = null;
            this.PortData = null;
        }
    }
}
