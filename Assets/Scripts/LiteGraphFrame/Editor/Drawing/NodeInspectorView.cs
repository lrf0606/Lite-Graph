using UnityEditor.Experimental.GraphView;


namespace LiteGraphFrame
{
    class NodeInspectorView : GraphElement
    {
        private LiteGraphEditorWindow m_OwnerEditorWindow;
        public NodeInspectorView(LiteGraphEditorWindow ownerEditorWindow) 
        {
            m_OwnerEditorWindow = ownerEditorWindow;
        }

        public void Initlization()
        {

        }
    }
}
