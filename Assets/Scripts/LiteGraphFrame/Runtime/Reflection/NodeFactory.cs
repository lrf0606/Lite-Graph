using System.Collections.Generic;

namespace LiteGraphFrame
{
    public delegate NodeRuntime CreateRuntimeNodeDeleagte();

    public static partial class LiteGraphNodeFactory
    {
        private static Dictionary<string, CreateRuntimeNodeDeleagte> m_CreateFuncDict;
        
        static LiteGraphNodeFactory()
        {
            m_CreateFuncDict = new Dictionary<string, CreateRuntimeNodeDeleagte>();
            // 新增内置节点，在这里注册
            RegisterCreateNodeFunc("ValueNodeCreateVector2", () => { return new ValueNodeCreateVector2(); });
            RegisterCreateNodeFunc("ValueNodeCreateVector3", () => { return new ValueNodeCreateVector3(); });
        }

        public static void RegisterCreateNodeFunc(string nodeType, CreateRuntimeNodeDeleagte func)
        {
            m_CreateFuncDict[nodeType] = func;
        }

        public static NodeRuntime CreateNodeRumtime(string nodeType)
        {
            if (m_CreateFuncDict.TryGetValue(nodeType, out var func))
            {
                return func();
            }
            else
            {
                throw new System.Exception($"NodeFactory create node failed, nodeType={nodeType}");
            }
        }
    }
}
