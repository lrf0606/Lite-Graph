namespace LiteGraphFrame
{
    public enum EBPEventId
    {
        Test1,
    }

    [NodeTitle("Event", "EventTest1")]
    class EventNode_Test1 : EventNodeData
    {
        public override EBPEventId GetEventId()
        {
            return EBPEventId.Test1;
        }
    }
}
