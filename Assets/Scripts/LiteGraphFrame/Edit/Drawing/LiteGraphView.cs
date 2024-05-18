using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

namespace LiteGraphFrame
{
    class LiteGraphView : GraphView
    {
        private LiteGraphEditorWindow m_OwnerEditorWindow;

        private GraphData m_GraphData;

        private LiteGraphSearchWindow m_SearchWindow;

        public LiteGraphView(LiteGraphEditorWindow ownerEditorWindow)
        {
            m_OwnerEditorWindow = ownerEditorWindow;
        }

        public void Initlization(GraphData graphData)
        {
            m_GraphData = graphData;
            // style
            style.flexGrow = 1;
            // 滚轮缩放、拖拽、选择、框选
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            // 初始化搜索菜单
            m_SearchWindow = ScriptableObject.CreateInstance<LiteGraphSearchWindow>();
            m_SearchWindow.OnSelectEntryCallback = OnSearchWindowSelectEntryCallback;
            nodeCreationRequest = OnNodeCreationRequest;
            // 监听视图变化
            graphViewChanged = OnGraphViewChanged;
            // 创建节点和连线
            var nodeViewDict = new Dictionary<string, NodeView>();
            foreach(var nodeData in m_GraphData.NodeDict.Values)
            {
                var nodeView = new NodeView();
                nodeView.Initlization(nodeData);
                nodeView.SetPosition(new Rect(new Vector2(nodeData.Position[0], nodeData.Position[1]), Vector2.zero));
                nodeViewDict.Add(nodeData.MyGUID, nodeView);
                AddElement(nodeView);
            }
            foreach(var nodeData in m_GraphData.NodeDict.Values)
            {
                foreach(var kv in nodeData.PortConnectionDict)
                {
                    var portData = nodeData.PortDict[kv.Key];
                    if (portData.IsInputPort)
                    {
                        continue;
                    }
                    var otherNodeData = kv.Value.NodeData;
                    var otherPortData = kv.Value.PortData;
                    var fromPortView = nodeViewDict[nodeData.MyGUID].PortDict[portData.MyGUID];
                    var toPortView = nodeViewDict[otherNodeData.MyGUID].PortDict[otherPortData.MyGUID];
                    var edge = new Edge
                    {
                        input = toPortView,
                        output = fromPortView,
                    };
                    edge.input.Connect(edge);
                    edge.output.Connect(edge);
                    AddElement(edge);
                }
            }
        }

        void OnNodeCreationRequest(NodeCreationContext context)
        {
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), m_SearchWindow);
        }

        bool OnSearchWindowSelectEntryCallback(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            // 屏幕鼠标位置转为编辑界面局部坐标
            var windowMousePos = m_OwnerEditorWindow.rootVisualElement.ChangeCoordinatesTo(m_OwnerEditorWindow.rootVisualElement.parent, context.screenMousePosition - m_OwnerEditorWindow.position.position);
            var viewMousePos = contentViewContainer.WorldToLocal(windowMousePos);
            // 创建数据节点
            var nodeData = (NodeDataBase)Activator.CreateInstance((Type)searchTreeEntry.userData);
            nodeData.Initliazation();
            if (!m_GraphData.AddNode(nodeData))
            {
                Debug.LogWarning($"事件节点[{nodeData}]只能出现一次");
                return false;
            }
            m_GraphData.UpdateNodePosition(nodeData, viewMousePos.x, viewMousePos.y);
            // 创建视图节点
            var nodeView = new NodeView();
            nodeView.Initlization(nodeData);
            nodeView.SetPosition(new Rect(viewMousePos, Vector2.zero));
            AddElement(nodeView);
            OnNodeCreated(nodeView);
            return true;
        }

        void OnNodeCreated(Node node)
        {
            //Debug.Log($"节点创建 {node}");
        }

        void OnNodeDeleted(Node node)
        {
            var nodeView = (NodeView)node;
            Debug.Log($"节点删除 {nodeView.NodeData}");
            m_GraphData.RemoveNode(nodeView.NodeData);
        }

        void OnNodeConnected(Port fromPortView, Port toPortView)
        {
            var fromPortData = (PortDataBase)fromPortView.userData;
            var toPortData = (PortDataBase)toPortView.userData;
            Debug.Log($"节点连接 {fromPortData.OwnerNodeData} {fromPortData.Name} {toPortData.OwnerNodeData} {toPortData.Name}");
            m_GraphData.ConnectNode(fromPortData.OwnerNodeData, fromPortData, toPortData.OwnerNodeData, toPortData);
            var fromNodeView = (NodeView)fromPortView.node;
            fromNodeView.OnNodeConnected(fromPortView);
            var toNodeView = (NodeView)toPortView.node;
            toNodeView.OnNodeConnected(toPortView);
        }

        void OnNodeDisconnected(Port fromPortView, Port toPortView)
        {
            var fromPortData = (PortDataBase)fromPortView.userData;
            var toPortData = (PortDataBase)toPortView.userData;
            Debug.Log($"节点断开 {fromPortData.OwnerNodeData} {fromPortData.Name} {toPortData.OwnerNodeData} {toPortData.Name}");
            m_GraphData.DisconnectNode(fromPortData.OwnerNodeData, fromPortData, toPortData.OwnerNodeData, toPortData);
            var fromNodeView = (NodeView)fromPortView.node;
            fromNodeView.OnNodeDisconnected(fromPortView);
            var toNodeView = (NodeView)toPortView.node;
            toNodeView.OnNodeDisconnected(toPortView);
        }

        void OnNodeMoved(Node node)
        {
            //Debug.Log($"表现层节点移动 {node}");
            var nodeView = (NodeView)node;
            var position = nodeView.GetPosition().position;
            m_GraphData.UpdateNodePosition(nodeView.NodeData, position[0], position[1]);
        }

        public GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // 元素被删除后，包括连线、节点等
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var element in graphViewChange.elementsToRemove)
                {
                    var elementType = element.GetType();
                    if (elementType == typeof(Node) || elementType.IsSubclassOf(typeof(Node)))
                    {
                        OnNodeDeleted((Node)element);
                    }
                    else if (elementType == typeof(Edge))
                    {
                        Edge edge = (Edge)element;
                        OnNodeDisconnected(edge.output, edge.input);
                    }
                }
            }
            // 连线被创建，发生在成功连接端口后
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    OnNodeConnected(edge.output, edge.input);
                }
            }
            // 元素位置移动
            if (graphViewChange.movedElements != null)
            {
                foreach (var element in graphViewChange.movedElements)
                {
                    var elementType = element.GetType();
                    if (elementType == typeof(Node) || elementType.IsSubclassOf(typeof(Node)))
                    {
                        OnNodeMoved((Node)element);
                    }
                }
            }
            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startViewPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            var startPortData = (PortDataBase)startViewPort.userData;
            foreach (var targetPortView in ports)
            {
                var targetPortData = (PortDataBase)targetPortView.userData;
                if (!startPortData.CanConnectTo(targetPortData))
                {
                    continue;
                }
                compatiblePorts.Add(targetPortView);
            }
            return compatiblePorts;
        }
    }
}

