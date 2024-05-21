using System.Reflection;

namespace LiteGraphFrame
{
    abstract class FunctionNodeData : NodeDataBase
    {
        public FunctionNodeData() : base()
        {
            NodeType = ENodeType.Function;
        }

        // ���ܽڵ���һ���������̽ڵ㡢һ��������̽ڵ㡢n���������ݽڵ�
        protected override void InitlizationPort()
        {
            var intputFlowPort = new FlowPortData();
            intputFlowPort.Initlization(this, true, "In");
            PortList.Add(intputFlowPort);
            PortDict[intputFlowPort.MyGUID] = intputFlowPort;

            foreach (var field in this.GetType().GetFields())
            {
                var inputAttribute = field.GetCustomAttribute<NodeInputAttribute>();
                if (inputAttribute != null)
                {
                    var inputFieldPort = new FieldPortData();
                    inputFieldPort.Initlization(this, true, field.Name);
                    inputFieldPort.InitFieldIfno(field, inputAttribute.FiledDescription);
                    PortList.Add(inputFieldPort);
                    PortDict[inputFieldPort.MyGUID] = inputFieldPort;
                }
            }

            var outputFlowPort = new FlowPortData();
            outputFlowPort.Initlization(this, false, "Out");
            PortList.Add(outputFlowPort);
            PortDict[outputFlowPort.MyGUID] = outputFlowPort;
        }
    }
}

