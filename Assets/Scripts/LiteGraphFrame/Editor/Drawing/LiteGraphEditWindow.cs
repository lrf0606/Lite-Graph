using System;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace LiteGraphFrame
{
    class LiteGraphEditorWindow : EditorWindow
    {
        [SerializeField]
        private string m_AssetGUID; // 序列化字段，保证界面重载时数据存在
        private GraphData m_GraphData;
        private FunctionToolBarView m_FunctionToolBar;
        private NodeInspectorView m_NodeInspectorView;
        private LiteGraphView m_GraphView;

        public string AssetGUID => m_AssetGUID;

        public LiteGraphEditorWindow()
        {
        }

        private void Update()
        {
            if (m_GraphData == null && m_AssetGUID != null)
            {
                Initlization(m_AssetGUID);
            }
            if (m_GraphData == null)
            {
                Close();
                return;
            }
            m_GraphView.OnUpdate();
        }

        public void Initlization(string guid)
        {
            m_AssetGUID = guid;
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!LiteGraphFileUtil.IsFileExist(assetPath))
            {
                EditorUtility.DisplayDialog("Error", $"Asset({assetPath}) Not Exist", "Ok"); 
                Close();
                return;
            }
            string fileName = Path.GetFileName(assetPath);
            string fileData = LiteGraphFileUtil.SafeReadAllText(assetPath);
            m_GraphData = new GraphData(assetPath);
            m_GraphData.Deserialize(fileData);

            m_FunctionToolBar = new FunctionToolBarView(this, SaveAsset, SaveAs, ShowInProject);
            m_FunctionToolBar.Initlization();
            this.rootVisualElement.Add(m_FunctionToolBar);

            m_NodeInspectorView = new NodeInspectorView(this);
            m_NodeInspectorView.Initlization();
            this.rootVisualElement.Add(m_NodeInspectorView);

            m_GraphView = new LiteGraphView(this);
            m_GraphView.Initlization(m_GraphData);
            this.rootVisualElement.Add(m_GraphView);

            this.titleContent = new GUIContent(fileName);
        }

        void SaveAsset()
        {
            var path = AssetDatabase.GUIDToAssetPath(m_AssetGUID);
            LiteGraphFileUtil.WriteToDisk(path, m_GraphData.Serialize());
            AssetDatabase.Refresh();
        }

        void SaveAs()
        {
            var curPath = AssetDatabase.GUIDToAssetPath(m_AssetGUID);
            string directory = Path.GetDirectoryName(curPath);
            string newPath = EditorUtility.SaveFilePanel("创建蓝图文件", directory, LiteGraphEditorUtil.NewFile, LiteGraphEditorUtil.Extension);
            if (!string.IsNullOrEmpty(newPath))
            {
                LiteGraphFileUtil.WriteToDisk(newPath, m_GraphData.Serialize());
            }
            AssetDatabase.Refresh();
        }

        void ShowInProject()
        {
            if (!string.IsNullOrEmpty(m_AssetGUID))
            {
                var path = AssetDatabase.GUIDToAssetPath(m_AssetGUID);
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                EditorGUIUtility.PingObject(asset);
            }
        }
    }
}

