namespace LiteGraphFrame
{
    public class EdgeRuntime
    {
        private PortRuntime m_OutputPort;
        private PortRuntime m_InputPort;
        public PortRuntime OutputPort => m_OutputPort;
        public PortRuntime InputPort => m_InputPort;

        public EdgeRuntime(PortRuntime outputPort, PortRuntime inputPort)
        {
            m_OutputPort = outputPort;
            m_InputPort = inputPort;
        }
    }
}