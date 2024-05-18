using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


namespace LiteGraphFrame
{
    class FunctionToolBarView : VisualElement
    {
        private LiteGraphEditorWindow m_OwnerEditorWindow;

        public Action SaveCallback { get; set; }
        public Action SaveAsCallback { get; set; }
        public Action ShowInProjectCallBack { get; set; }

        public FunctionToolBarView(LiteGraphEditorWindow ownerEditorWindow)
        {
            m_OwnerEditorWindow = ownerEditorWindow;
        }

        public void Initlization()
        {
            var toolbar = new Toolbar();

            Button saveBtn = new Button(clickEvent: SaveCallback);
            saveBtn.text = "Save";
            toolbar.Add(saveBtn);

            Button saveAsBtn = new Button(clickEvent: SaveAsCallback);
            saveAsBtn.text = "Save As";
            toolbar.Add(saveAsBtn);

            Button showInProjectBtn = new Button(clickEvent: ShowInProjectCallBack);
            showInProjectBtn.text = "Show In Project";
            toolbar.Add(showInProjectBtn);

            Add(toolbar);
        }
    }
}