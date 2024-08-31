using System;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public static partial class LiteGraphNodeFactory
    {
        public static void InitBuiltinFactory()
        {
            RegisterCreateNodeFunc("ControlNodeIf", () => { return new ControlNodeIf(); });
            RegisterCreateNodeFunc("ControlNodeForLoop", () => { return new ControlNodeForLoop(); });
            RegisterCreateNodeFunc("ValueNodeCreateVector2", () => { return new ValueNodeCreateVector2(); });
            RegisterCreateNodeFunc("ValueNodeCreateVector3", () => { return new ValueNodeCreateVector3(); });
        }
    }
}
// === LiteGraphFrame Code Generate End ===