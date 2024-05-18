using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace LiteGraphFrame
{
    public class LiteGraphRuntimeObject
    {
        private GraphData m_GraphData;

        public LiteGraphRuntimeObject(string assetPath)
        {
            var fileData = File.ReadAllText(assetPath);
            var graphData = new GraphData();
            graphData.Initlization(assetPath);
            graphData.Deserialize(fileData);
            m_GraphData = graphData;
        } 

        public void RunEvent(EBPEventId eEventId)
        {
            if (!m_GraphData.EventNodeDict.TryGetValue(eEventId, out var startNodeData))
            {
                throw new System.Exception($"Event {eEventId} not exist in {m_GraphData}");
            }
            var nodeList = GenerageNodeExecuteList(startNodeData);
            foreach (var curNode in nodeList)
            {
                var inputFieldPortList = new List<FieldPortData>();
                var outputFieldPortList = new List<FieldPortData>();
                foreach (var port in curNode.PortList)
                {
                    if (port is FieldPortData fieldPort)
                    {
                        if (port.IsInputPort)
                        {
                            inputFieldPortList.Add(fieldPort);
                        }
                        else
                        {
                            outputFieldPortList.Add(fieldPort);
                        }
                    }
                }

                // step1.input field port给node赋值
                foreach (var port in inputFieldPortList)
                {
                    var fieldInfo = curNode.GetType().GetField(port.FieldName);
                    // input field port未连接使用本地的FieldValue，连接使用运行时的RuntimeFieldValue
                    if (port.ConnectionInfo.NodeData == null)
                    {
                        fieldInfo.SetValue(curNode, port.FieldValue);
                    }
                    else
                    {
                        fieldInfo.SetValue(curNode, port.RuntimeFieldValue);
                    }
                }

                // step2.node execute
                curNode.Execute();

                foreach (var port in outputFieldPortList)
                {
                    if (port.ConnectionInfo.NodeData == null)
                    { 
                        continue; 
                    }
                    // step3.node 给output field port赋值
                    var fieldInfo = curNode.GetType().GetField(port.FieldName);
                    port.RuntimeFieldValue = fieldInfo.GetValue(curNode);

                    // step4.output field port给连接的 input field port赋值
                    var connectFieldPort = (FieldPortData)port.ConnectionInfo.PortData;
                    connectFieldPort.RuntimeFieldValue = port.RuntimeFieldValue;
                }
            }
        }

        private List<NodeDataBase> GenerageNodeExecuteList(NodeDataBase startNode)
        {
            var resultList = new List<NodeDataBase>() {};
            var addedNodeSet = new HashSet<string>() {};

            var curNode = startNode;
            var outputFlowPort = new List<PortDataBase>();
            while (curNode != null)
            {
                outputFlowPort.Clear();
                foreach (var port in curNode.PortList)
                {
                    if (!port.IsInputPort && port is FlowPortData flowPort)
                    {
                        outputFlowPort.Add(flowPort);
                    }
                }
                FindPreviousNodes(curNode, resultList, addedNodeSet);
                curNode = outputFlowPort.Count > 0 ? outputFlowPort[0].ConnectionInfo.NodeData : null;
            }
            return resultList;
        }

        private void FindPreviousNodes(NodeDataBase curNode, List<NodeDataBase> resultList, HashSet<string> addedNodeSet)
        {
            var prewNodeList = new List<NodeDataBase>();
            foreach(var port in curNode.PortList)
            {
                if (port.IsInputPort && port.ConnectionInfo.NodeData != null)
                {
                    prewNodeList.Add(port.ConnectionInfo.NodeData);
                }
            }
            foreach(var prevNode in prewNodeList)
            {
                if (addedNodeSet.Contains(prevNode.MyGUID))
                {
                    continue;
                }
                FindPreviousNodes(prevNode, resultList, addedNodeSet);
            }
            resultList.Add(curNode);
            addedNodeSet.Add(curNode.MyGUID);
        }
    }
}
