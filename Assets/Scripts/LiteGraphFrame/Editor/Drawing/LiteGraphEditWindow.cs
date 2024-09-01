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

        public string AssetGUID
        {
            get { return m_AssetGUID; }
            private set { m_AssetGUID = value; }
        }

        private GraphData m_GraphData;

        private FunctionToolBarView m_FunctionToolBar;

        private NodeInspectorView m_NodeInspectorView;

        private LiteGraphView m_GraphView;

        public LiteGraphEditorWindow()
        {
        }

        private void Update()
        {
            if (m_GraphData == null && AssetGUID != null)
            {
                Initlization(AssetGUID);
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
            AssetGUID = guid;
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!LiteGraphFileUtil.IsFileExist(assetPath))
            {
                EditorUtility.DisplayDialog("Error", $"Asset({assetPath}) Not Exist", "Ok"); 
                Close();
                return;
            }
            string fileName = Path.GetFileName(assetPath);
            string fileData = LiteGraphFileUtil.SafeReadAllText(assetPath);
            m_GraphData = new GraphData();
            m_GraphData.Initlization(assetPath);
            m_GraphData.Deserialize(fileData);

            m_FunctionToolBar = new FunctionToolBarView(this)
            {
                SaveCallback = SaveAsset,
                SaveAsCallback = SaveAs,
                ShowInProjectCallback = ShowInProject,
                GenerateNodeCallback = GenerateNode,
            };
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
            var path = AssetDatabase.GUIDToAssetPath(AssetGUID);
            LiteGraphFileUtil.WriteToDisk(path, m_GraphData.Encoder().ToJson());
            AssetDatabase.Refresh();
        }

        void SaveAs()
        {
            var curPath = AssetDatabase.GUIDToAssetPath(AssetGUID);
            string directory = Path.GetDirectoryName(curPath);
            string newPath = EditorUtility.SaveFilePanel("创建蓝图文件", directory, LiteGraphEditorUtil.NewFile, LiteGraphEditorUtil.Extension);
            if (!string.IsNullOrEmpty(newPath))
            {
                LiteGraphFileUtil.WriteToDisk(newPath, m_GraphData.Encoder().ToJson());
            }
            AssetDatabase.Refresh();
        }

        void ShowInProject()
        {
            if (!string.IsNullOrEmpty(AssetGUID))
            {
                var path = AssetDatabase.GUIDToAssetPath(AssetGUID);
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                EditorGUIUtility.PingObject(asset);
            }
        }

        void GenerateNode()
        {
            CodeGenerateConfig.ValidateDirectory();
            NodeTypeGenerator.Generate();
            NodeRuntimeGenerator.Generate();
            AssetDatabase.Refresh();
        }
    }
}

