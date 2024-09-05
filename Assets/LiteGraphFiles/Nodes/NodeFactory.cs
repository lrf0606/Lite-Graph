using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public static partial class LiteGraphNodeFactory
    {
        public static void InitCustomFactory()
        {
            RegisterCreateNodeFunc("EventNodeEventTest1", () => { return new EventNodeEventTest1(); });
            RegisterCreateNodeFunc("EventNodeEventTest2", () => { return new EventNodeEventTest2(); });
            RegisterCreateNodeFunc("FunctionNodeLog", () => { return new FunctionNodeLog(); });
            RegisterCreateNodeFunc("ValueNodeGetRandomInt", () => { return new ValueNodeGetRandomInt(); });
        }
    }
}
// === LiteGraphFrame Code Generate End ===