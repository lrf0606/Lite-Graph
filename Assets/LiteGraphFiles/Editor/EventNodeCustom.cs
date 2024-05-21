namespace LiteGraphFrame
{
    [NodeRegister("Event", "Event1")]
    sealed class EventNodeEventTest1 : EventNodeData
    {
        public override int GetEventId()
        {
            return 1;
        }
    }

    [NodeRegister("Event", "Event2")]
    sealed class EventNodeEventTest2 : EventNodeData
    {
        public override int GetEventId()
        {
            return 2;
        }
    }
}