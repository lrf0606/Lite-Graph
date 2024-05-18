using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace LiteGraphFrame
{
    [CustomEditor(typeof(LiteGraphImporter))]
    class LiteGraphImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open LiteGraph Editor"))
            {
                var importer = target as LiteGraphImporter;
                if (importer != null)
                {
                    ShowLiteGraphEditWindow(importer.assetPath);
                }
            }
            ApplyRevertGUI();
        }

        internal static bool ShowLiteGraphEditWindow(string path)
        {
            if (!LiteGraphCommonUtil.IsLiteGraphFile(path))
            {
                return false;
            }
            var guid = AssetDatabase.AssetPathToGUID(path);
            foreach (var windowObj in Resources.FindObjectsOfTypeAll<LiteGraphEditorWindow>())
            {
                if (windowObj.AssetGUID == guid)
                {
                    windowObj.Focus();
                    return true;
                }
            }
            var window = EditorWindow.CreateWindow<LiteGraphEditorWindow>(typeof(LiteGraphEditorWindow), typeof(SceneView));
            window.Initlization(guid);
            window.Focus();
            return true;
        }

        [OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var path = AssetDatabase.GetAssetPath(instanceID);
            return ShowLiteGraphEditWindow(path);
        }
    }
}

