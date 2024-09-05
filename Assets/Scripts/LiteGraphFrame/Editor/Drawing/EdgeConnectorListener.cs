using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace LiteGraphFrame
{
    class EdgeConnectorListener : IEdgeConnectorListener
    {
        private LiteGraphView m_GraphView;
        private GraphData m_GraphData;

        public EdgeConnectorListener(LiteGraphView graphView, GraphData graphData)
        {
            m_GraphView = graphView;
            m_GraphData = graphData;
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            // 删除端口连线
            if (edge.input.capacity == Port.Capacity.Single)
            {
                foreach (var connection in edge.input.connections)
                {
                    if (connection != edge)
                    {            
                        m_GraphData.DisconnectNode((PortDataBase)connection.output.userData, (PortDataBase)connection.input.userData);
                    }
                }
            }

            if (edge.output.capacity == Port.Capacity.Single)
            {
                foreach (var connection in edge.output.connections)
                {
                    if (connection != edge)
                    {
                        m_GraphData.DisconnectNode((PortDataBase)connection.output.userData, (PortDataBase)connection.input.userData);
                    }
                }
            }
            // 连接端口连线
            m_GraphData.ConnectNode((PortDataBase)edge.output.userData, (PortDataBase)edge.input.userData);
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            var portData = edge.output != null ? (PortDataBase)edge.output.userData : (PortDataBase)edge.input.userData;
            m_GraphView.ShowCreateNodeSearchWindow(position, isScreenMousePosition: false, toConnectPort: portData);
        }
    }
}