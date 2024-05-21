using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace LiteGraphFrame
{
    static class LiteGraphEditorUtil
    {
        public const string Extension = "litegraph";
        public const string ExtensionWithDot = ".litegraph";
        public const string NewFile = "New Lite Graph";

        public static void CreateNewLiteGraphFile()
        {
            var endAction = ScriptableObject.CreateInstance<NewLiteGraphActoin>();
            string pathName = string.Format("{0}.{1}", NewFile, Extension);
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endAction, pathName, null, null);
        }

        public static bool IsLiteGraphFile(string assetPath)
        {
            var extension = Path.GetExtension(assetPath);
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }
            if (extension.ToLowerInvariant() != ExtensionWithDot)
            {
                return false;
            }
            return true;
        }
    }

    class NewLiteGraphActoin : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var graphData = new GraphData();
            graphData.Initlization(pathName);
            LiteGraphFileUtil.WriteToDisk(pathName, graphData.Serialize());
            AssetDatabase.Refresh();
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(obj);
        }

        public override void Cancelled(int instanceId, string pathName, string resourceFile)
        {
            base.Cancelled(instanceId, pathName, resourceFile);
        }
    }


}

