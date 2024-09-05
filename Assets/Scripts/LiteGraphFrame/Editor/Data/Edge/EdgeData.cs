using LitJson;
using System;

namespace LiteGraphFrame
{
    class EdgeData
    {
        private string m_GUID;
        private PortDataBase m_OutputPortData; // 连接的输出端口
        private PortDataBase m_InputPortData; // 连接的输入端口
        public string MyGUID => m_GUID;
        public PortDataBase OutputPortData => m_OutputPortData;
        public PortDataBase InputPortData => m_InputPortData;

        public EdgeData()
        {

        }

        public void Initlization(PortDataBase outputPort, PortDataBase inputPort)
        {
            m_GUID = Guid.NewGuid().ToString("N");
            m_OutputPortData = outputPort;
            m_InputPortData = inputPort;
        }

    }
}