using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace LiteGraphFrame
{
    class PortView : Port
    {
        protected PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            
        }

        public static PortView Create(PortDataBase portData, EdgeConnectorListener edgeConnectorListener)
        {
            var direction = portData.IsInputPort ? Direction.Input : Direction.Output;
            var capacity = !portData.IsInputPort && portData is FieldPortData ? Port.Capacity.Multi : Port.Capacity.Single; // 输出数据端口可以连接多个其他数据端口
            var portView = new PortView(Orientation.Horizontal, direction, capacity, portData.GetType())
            {
                m_EdgeConnector = new EdgeConnector<Edge>(edgeConnectorListener)
            };
            portView.AddManipulator(portView.m_EdgeConnector);
            if (portData is FieldPortData valuePortData)
            {
                portView.portName = string.IsNullOrEmpty(valuePortData.FieldDescription) ? valuePortData.Name : $"{valuePortData.Name}({valuePortData.FieldDescription})";
            }
            else
            {
                portView.portName = portData.Name;
            }
            if (portData.IsInputPort && portData is FieldPortData fieldPortData)
            {
                // 端口输入值的编辑框
                var fieldInputView = new PortFieldInputView(fieldPortData);
                portView.Add(fieldInputView);
            }
            portView.userData = portData;
            return portView;
        }
    }
}