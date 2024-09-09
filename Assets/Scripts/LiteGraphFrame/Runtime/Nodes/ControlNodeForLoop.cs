using System;
using System.Collections.Generic;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ControlNodeForLoop : NodeRuntime
    {
        public int First;
        public int Last;
        public int Step;
        public int Index;

        public override object GetValue(string fieldName)
        {
            switch (fieldName)
            {
                case "First": return First;
                case "Last": return Last;
                case "Step": return Step;
                case "Index": return Index;
                default: return null;
            }
        }
        public override void SetValue(string fieldName, object value)
        {
            switch (fieldName)
            {
                case "First": { First = (int)value; break; };
                case "Last": { Last = (int)value; break; };
                case "Step": { Step = (int)value; break; };
                case "Index": { Index = (int)value; break; };
                default: break;
            }
        }

        // === Execute Logic Start ===
        public override void ExecuteLogic()
        {
            if (First >= Last)
            {
                return;
            }
            if (First <= Last && Step <= 0) 
            {
                return;
            }
            var bodyFollowingNodes = GetBodyFollowingNodes();
            for (int i = First; i < Last; i += Step)
            {
                // Index值更新并传递
                Index = i;
                UpdateOutputFieldPorts();
                // Body端口后续节点重置
                foreach(var node in bodyFollowingNodes)
                {
                    node.Reset();
                }
                // Body下一节点执行
                foreach(var edge in m_OutputFlowPortList[1].Edges)
                {
                    edge.InputPort.Node.Execute();
                }
            }
        }

        private List<NodeRuntime> GetBodyFollowingNodes()
        {
            var result = new List<NodeRuntime>();
            var nodeSet = new HashSet<string>();
            foreach(var edge in m_OutputFlowPortList[1].Edges)
            {
                FindNodes(ref result, ref nodeSet, edge.InputPort.Node);
            }
            return result;
        }

        private void FindNodes(ref List<NodeRuntime> result, ref HashSet<string> nodeSet, NodeRuntime curNode)
        {
            if (curNode == null || curNode.IsExecuted() || nodeSet.Contains(curNode.MyGUID))
            {
                return;
            }
            nodeSet.Add(curNode.MyGUID);
            result.Add(curNode);
            foreach(var port in curNode.OutputPortList)
            {
                foreach(var edge in port.Edges)
                {
                    FindNodes(ref result, ref nodeSet, edge.InputPort.Node);
                }
            }
            foreach (var port in curNode.InputPortList)
            {
                foreach (var edge in port.Edges)
                {
                    FindNodes(ref result, ref nodeSet, edge.OutputPort.Node);
                }
            }
        }

        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===