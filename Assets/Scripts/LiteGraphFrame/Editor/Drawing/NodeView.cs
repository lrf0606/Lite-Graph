using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Reflection;


namespace LiteGraphFrame
{
    class NodeView : Node
    {
        public NodeDataBase NodeData { get; private set; }
        public Dictionary<string, Port> PortDict { get; private set; }

        public NodeView() { }

        public void Initlization(NodeDataBase nodeData)
        {
            NodeData = nodeData;
            PortDict = new Dictionary<string, Port>();
            InitlizationTitle();
            InitlizationPort();
        }

        void InitlizationTitle()
        {
            if (string.IsNullOrEmpty(NodeData.Name))
            {

                var titleAttribute = NodeData.GetType().GetCustomAttribute<NodeRegisterAttribute>();
                this.title = titleAttribute.GetLastTitle();
            }
            else
            {
                this.title = NodeData.Name;
            }
        }

        void InitlizationPort()
        {
            foreach (var portData in NodeData.PortList)
            {
                Direction direction;
                if (portData.IsInputPort)
                {
                    direction = Direction.Input;
                }
                else
                {
                    direction = Direction.Output;
                }
                var portView = InstantiatePort(Orientation.Horizontal, direction, Port.Capacity.Single, portData.GetType());
                if (portData is FieldPortData valuePortData)
                {
                    if (string.IsNullOrEmpty(valuePortData.FieldDescription))
                    {
                        portView.portName = valuePortData.Name;
                    }
                    else
                    {
                        portView.portName = $"{valuePortData.Name}({valuePortData.FieldDescription})";
                    }
                }
                else
                {
                    portView.portName = portData.Name;
                }
                portView.userData = portData;
                if (portData.IsInputPort)
                {
                    inputContainer.Add(portView);
                    if (portData is FieldPortData fieldPortData)
                    {
                        var fieldInputView = new PortFieldInputView(fieldPortData);
                        portView.Add(fieldInputView);
                    }
                }
                else
                {
                    outputContainer.Add(portView); 
                }
                PortDict[portData.MyGUID] = portView;
            }
        }
        public void OnNodeConnected(Port port)
        {
            foreach(var view in port.Children())
            {
                if (view is PortFieldInputView fieldInputView)
                {
                    fieldInputView.RefreshVisible();
                }
            }
        }

        public void OnNodeDisconnected(Port port)
        {
            foreach (var view in port.Children())
            {
                if (view is PortFieldInputView fieldInputView)
                {
                    fieldInputView.RefreshVisible();
                }
            }
        }
    }
}
