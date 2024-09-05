using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteGraphFrame
{
    class LiteGraphView : GraphView
    {
        private LiteGraphEditorWindow m_OwnerEditorWindow;
        private GraphData m_GraphData;
        private CopyPasteGraphData m_CopyPasteGraphData;
        private LiteGraphSearchWindow m_SearchWindow;
        private Vector2 m_CachedMousePosition;
        private EdgeConnectorListener m_EdgeConnectorListener;
        private Dictionary<string, NodeView> m_NodeViewDict;
        private bool m_NeedResetViewTransform;

        public LiteGraphView(LiteGraphEditorWindow ownerEditorWindow)
        {
            m_OwnerEditorWindow = ownerEditorWindow;
            m_NodeViewDict = new Dictionary<string, NodeView>();
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
            this.AddManipulator(new ClickSelector());
            // 初始化搜索菜单
            m_SearchWindow = ScriptableObject.CreateInstance<LiteGraphSearchWindow>();
            m_SearchWindow.OnSelectEntryCallback = OnSearchWindowSelectEntryCallback;
            nodeCreationRequest = OnNodeCreationRequest;
            // 监听视图变化
            graphViewChanged = OnGraphViewChanged;
            // 重写删除选中
            deleteSelection = DeleteSelectionImplementation;
            // 复制粘贴功能
            serializeGraphElements = SerializeGraphElementsImplementation;
            canPasteSerializedData = CanPasteSerializedDataImplementation;
            unserializeAndPaste = UnserializeAndPasteImplementation;
            // 监听鼠标移动
            RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
            // 连线监听
            m_EdgeConnectorListener = new EdgeConnectorListener(this, graphData);
            // 设置初始位置
            UpdateViewTransform(GetGraphCenterPosition(), new Vector3(1, 1, 1));
        }

        public void OnUpdate()
        {
            if (m_GraphData == null)
            {
                return;
            }
            foreach (var edgeData in m_GraphData.RemovedEdges)
            {
                RemoveEdgeView(edgeData);
            }
            foreach (var nodeData in m_GraphData.RemovedNodes)
            {
                RemoveNodeView(nodeData);
            }
            foreach (var nodeData in m_GraphData.AddedNodes)
            {
                CreateNodeView(nodeData);
            }
            foreach (var edgeData in m_GraphData.AddedEdges)
            {
                CreateEdgeView(edgeData);
            }
            m_GraphData.ClearChanges();
        }

        public void CreateNodeView(NodeDataBase nodeData)
        {
            var nodeView = new NodeView(m_EdgeConnectorListener);
            nodeView.Initlization(nodeData);
            nodeView.SetPosition(new Rect(new Vector2(nodeData.Position[0], nodeData.Position[1]), Vector2.zero));
            AddElement(nodeView);
            m_NodeViewDict[nodeData.MyGUID] = nodeView;
        }

        public void RemoveNodeView(NodeDataBase nodeData)
        {
            var nodeView = m_NodeViewDict[nodeData.MyGUID];
            RemoveElement(nodeView);
        }

        public void CreateEdgeView(EdgeData edgeData)
        {
            var outputNodeView = m_NodeViewDict[edgeData.OutputPortData.NodeData.MyGUID];
            var outputPortView = outputNodeView.GetPortViewByGUID(edgeData.OutputPortData.MyGUID);
            var inputNodeView = m_NodeViewDict[edgeData.InputPortData.NodeData.MyGUID];
            var inputPortView = inputNodeView.GetPortViewByGUID(edgeData.InputPortData.MyGUID);
            var edgeView = new Edge()
            {
                output = outputPortView,
                input = inputPortView,
            };
            edgeView.output.Connect(edgeView);
            edgeView.input.Connect(edgeView);
            AddElement(edgeView);
            outputNodeView.OnNodeViewConnectedChange(true, outputPortView);
            inputNodeView.OnNodeViewConnectedChange(true, inputPortView);
        }

        public void RemoveEdgeView(EdgeData edgeData)
        {
            var outputNodeView = m_NodeViewDict[edgeData.OutputPortData.NodeData.MyGUID];
            var outputPortView = outputNodeView.GetPortViewByGUID(edgeData.OutputPortData.MyGUID);
            var inputNodeView = m_NodeViewDict[edgeData.InputPortData.NodeData.MyGUID];
            var inputPortView = inputNodeView.GetPortViewByGUID(edgeData.InputPortData.MyGUID);
            foreach (var edgeView in outputPortView.connections)
            {
                if (edgeView.input == inputPortView)
                {
                    edgeView.output.Disconnect(edgeView);
                    edgeView.input.Disconnect(edgeView);
                    edgeView.output = null;
                    edgeView.input = null;
                    RemoveElement(edgeView);
                    outputNodeView.OnNodeViewConnectedChange(false, outputPortView);
                    inputNodeView.OnNodeViewConnectedChange(false, inputPortView);
                    break;
                }
            }
        }

        public GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // 元素位置移动
            if (graphViewChange.movedElements != null)
            {
                foreach (var element in graphViewChange.movedElements)
                {
                    if (element is Node)
                    {
                        var nodeView = (NodeView)element;
                        var position = nodeView.GetPosition().position;
                        nodeView.NodeData.SetPosition(position[0], position[1]);
                    }
                }
            }
            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPortView, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortData = (PortDataBase)startPortView.userData;
            foreach (var targetPortView in ports)
            {
                var targetPortData = (PortDataBase)targetPortView.userData;
                if (startPortData.IsInputPort)
                {
                    if (!targetPortData.CanConnectTo(startPortData))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!startPortData.CanConnectTo(targetPortData))
                    {
                        continue;
                    }
                }
                compatiblePorts.Add(targetPortView);
            }
            return compatiblePorts;
        }

        void OnNodeCreationRequest(NodeCreationContext context)
        {
            ShowCreateNodeSearchWindow(context.screenMousePosition, isScreenMousePosition: true);
        }

        public void ShowCreateNodeSearchWindow(Vector2 position, bool isScreenMousePosition, PortDataBase toConnectPort=null)
        {
            m_SearchWindow.ToConnectPort = toConnectPort;
            if (!isScreenMousePosition)
            {
                position += m_OwnerEditorWindow.position.position;
            }
            SearchWindow.Open(new SearchWindowContext(position), m_SearchWindow);
        }

        bool OnSearchWindowSelectEntryCallback(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            // 坐标转化 
            var windowMousePos = m_OwnerEditorWindow.rootVisualElement.ChangeCoordinatesTo(m_OwnerEditorWindow.rootVisualElement.parent, context.screenMousePosition - m_OwnerEditorWindow.position.position);
            var viewMousePos = contentViewContainer.WorldToLocal(windowMousePos);
            // 创建数据节点
            var nodeData = (NodeDataBase)Activator.CreateInstance((Type)searchTreeEntry.userData);
            nodeData.Initliazation();
            nodeData.SetPosition(viewMousePos.x, viewMousePos.y);
            if (!m_GraphData.AddNode(nodeData, out var failReason))
            {
                Debug.LogWarning(failReason);
                return false;
            }
            // 连接其他端口
            var needConnectPortData = m_SearchWindow.FindNeedConnectPort(nodeData);
            if (needConnectPortData != null) 
            {
                if (needConnectPortData.IsInputPort)
                {
                    m_GraphData.ConnectNode(m_SearchWindow.ToConnectPort, needConnectPortData);
                }
                else
                {
                    m_GraphData.ConnectNode(needConnectPortData, m_SearchWindow.ToConnectPort);
                }
            }
            return true;
        }

        private string SerializeGraphElementsImplementation(IEnumerable<GraphElement> elements)
        {
            var nodes = new List<NodeDataBase>();
            var edges = new List<EdgeData>();
            foreach(var element in elements) 
            {
                if (element is Node)
                {
                    nodes.Add(((NodeView)element).NodeData);
                }
                else if (element is Edge edgeView)
                {
                    var outputPortPortData = (PortDataBase)edgeView.output.userData;
                    var inputPortPortData = (PortDataBase)edgeView.input.userData;
                    edges.Add(outputPortPortData.GetEdgeDataByConnectedPort(inputPortPortData));
                }
            }
            if (m_CopyPasteGraphData == null)
            {
                m_CopyPasteGraphData = new CopyPasteGraphData();
            }
            else
            {
                m_CopyPasteGraphData.Clear();
            }
            m_CopyPasteGraphData.AddCopyElements(nodes, edges);
            return m_CopyPasteGraphData.Serialize();
        }

        private bool CanPasteSerializedDataImplementation(string serializedData)
        {
            try
            {
                m_CopyPasteGraphData.Deserialize(serializedData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UnserializeAndPasteImplementation(string operationName, string serializedData)
        {
            var viewMousePos = contentViewContainer.WorldToLocal(m_CachedMousePosition);
            m_CopyPasteGraphData.Deserialize(serializedData);
            m_CopyPasteGraphData.PasteOffsetNodesPostion(viewMousePos.x, viewMousePos.y); 
            foreach(var nodeData in m_CopyPasteGraphData.PasteNodes)
            {
                m_GraphData.AddNode(nodeData, out _);
            }
            foreach(var edgeData in m_CopyPasteGraphData.PasteEdges)
            {
                m_GraphData.ConnectNode(edgeData.OutputPortData, edgeData.InputPortData);
            }
        }

        private void OnMouseMoveEvent(MouseMoveEvent evt)
        {
            m_CachedMousePosition = evt.mousePosition;
        }

        private void DeleteSelectionImplementation(string operationName, AskUser askUser)
        {
            // 参考GraphView的DeleteSelection()
            var toRemoveElements = new HashSet<GraphElement>();
            // CollectElements会找到相关联的元素，比如删除node时会找到edge
            CollectElements(selection.OfType<GraphElement>(), toRemoveElements, (GraphElement e) => (e.capabilities & Capabilities.Deletable) == Capabilities.Deletable);
            foreach (var element in toRemoveElements)
            {
                if (element is Node)
                {
                    m_GraphData.RemoveNode(((NodeView)element).NodeData);
                }
                else if (element is Edge edgeView)
                {
                    m_GraphData.DisconnectNode((PortDataBase)edgeView.output.userData, (PortDataBase)edgeView.input.userData);
                }
                else
                {
                    // 其他类型删除拓展
                }
            }
            selection.Clear();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // 右键菜单拓展
            base.BuildContextualMenu(evt);
        }

        private Vector3 GetGraphCenterPosition()
        {
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            foreach (var nodeData in m_GraphData.AllNodes)
            {
                minX = MathF.Min(minX, nodeData.Position[0]);
                minY = MathF.Min(minY, nodeData.Position[1]);
                maxX = MathF.Max(maxX, nodeData.Position[0]);
                maxY = MathF.Max(maxY, nodeData.Position[1]); 
            }
            var nodeCenterX = (minX + maxX) / 2;
            var nodeCenterY = (minY + maxY) / 2;
            var windowWidth = m_OwnerEditorWindow.position.width;
            var windowHeight = m_OwnerEditorWindow.position.height;
            return new Vector3(windowWidth / 2 - nodeCenterX, windowHeight / 2 - nodeCenterY, 0);
        }
    }
}

