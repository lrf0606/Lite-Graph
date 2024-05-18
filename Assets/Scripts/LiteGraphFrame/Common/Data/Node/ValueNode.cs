using System.Reflection;


namespace LiteGraphFrame
{
    abstract class ValueNodeData : NodeDataBase
    {
        // ���ݽڵ�n���������ݽڵ㡢n��������ݽڵ�
        protected override void InitlizationPort()
        {
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
                
                var outputAttribut = field.GetCustomAttribute<NodeOutputAttribute>();
                if (outputAttribut != null)
                {
                    var outputFieldPort = new FieldPortData();
                    outputFieldPort.Initlization(this, false, field.Name);
                    outputFieldPort.InitFieldIfno(field, outputAttribut.FiledDescription);
                    PortList.Add(outputFieldPort);
                    PortDict[outputFieldPort.MyGUID] = outputFieldPort;
                }
            }
        }
    }
}
