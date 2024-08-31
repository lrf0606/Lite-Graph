using System;
using System.Collections.Generic;
// === LiteGraphFrame Code Generate Start ===
namespace LiteGraphFrame
{
    public partial class ControlNodeForLoop : NodeRuntime
    {
        public System.Int32 First;
        public System.Int32 Last;
        public System.Int32 Step;
        public System.Int32 Index;

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
                case "First": { First = (System.Int32)value; break; };
                case "Last": { Last = (System.Int32)value; break; };
                case "Step": { Step = (System.Int32)value; break; };
                case "Index": { Index = (System.Int32)value; break; };
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
            for (int i = First; i <= Last; i += Step)
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
                OutputFlowPortList[1].ConnectedPort?.Node.Execute();
            }
        }

        private List<NodeRuntime> GetBodyFollowingNodes()
        {
            var result = new List<NodeRuntime>();
            var nodeSet = new HashSet<string>();
            if (OutputFlowPortList[1].ConnectedPort != null)
            {
                FindNodes(ref result, ref nodeSet, OutputFlowPortList[1].ConnectedPort.Node);
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
                if (port.ConnectedPort != null)
                {
                    FindNodes(ref result, ref nodeSet, port.ConnectedPort.Node);
                }
            }
            foreach (var port in curNode.InputPortList)
            {
                if (port.ConnectedPort != null)
                {
                    FindNodes(ref result, ref nodeSet, port.ConnectedPort.Node);
                }
            }
        }

        // === Execute Logic End ===
    }
}
// === LiteGraphFrame Code Generate End ===