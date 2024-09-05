using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


namespace LiteGraphFrame
{
    class FunctionToolBarView : VisualElement
    {
        private LiteGraphEditorWindow m_OwnerEditorWindow;
        private Action m_SaveCallback;
        private Action m_SaveAsCallback;
        private Action m_ShowInProjectCallback;
        private Action m_GenerateNodeCallback;

        public FunctionToolBarView(LiteGraphEditorWindow ownerEditorWindow, Action saveCallback, Action saveAsCallback, Action showInProjectCallback, Action generateNodeCallback)
        {
            m_OwnerEditorWindow = ownerEditorWindow;
            m_SaveCallback = saveCallback;
            m_SaveAsCallback = saveAsCallback;
            m_ShowInProjectCallback = showInProjectCallback;
            m_GenerateNodeCallback = generateNodeCallback;
        }

        public void Initlization()
        {
            var toolbar = new Toolbar();

            Button saveBtn = new Button(clickEvent: m_SaveCallback);
            saveBtn.text = "Save";
            toolbar.Add(saveBtn);

            Button saveAsBtn = new Button(clickEvent: m_SaveAsCallback);
            saveAsBtn.text = "Save As";
            toolbar.Add(saveAsBtn);

            Button showInProjectBtn = new Button(clickEvent: m_ShowInProjectCallback);
            showInProjectBtn.text = "Show In Project";
            toolbar.Add(showInProjectBtn);

            Button generateNodeBtn = new Button(clickEvent: m_GenerateNodeCallback);
            generateNodeBtn.text = "Generate Node";
            toolbar.Add(generateNodeBtn);

            Add(toolbar);
        }
    }
}