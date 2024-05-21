namespace LiteGraphFrame
{
    public enum EPortType
    {
        Flow = 0,
        Field,
    }

    public class PortRuntime
    {
        public NodeRuntime Node;
        public EPortType PortType;
        public PortRuntime ConnectedPort;
        public string FieldName;
        public object FieldValue;
    }


}
