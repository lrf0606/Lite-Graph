using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public static partial class LiteGraphNodeFactory
    {
        public static void InitCreateFuncDict()
        {
            RegisterCreateNodeFunc("EventNodeEventTest1", () => { return new EventNodeEventTest1(); });
            RegisterCreateNodeFunc("EventNodeEventTest2", () => { return new EventNodeEventTest2(); });
            RegisterCreateNodeFunc("FunctionNodeTest1", () => { return new FunctionNodeTest1(); });
            RegisterCreateNodeFunc("FunctionNodeTest2", () => { return new FunctionNodeTest2(); });
            RegisterCreateNodeFunc("ValueNodeGetRandomInt", () => { return new ValueNodeGetRandomInt(); });

        }
    }
}
// === LiteGraphFrame Code Generate End ===












